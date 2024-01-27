﻿using Bot.scripts;
using Configuration;
using HealthBot;
using HealthBot.handlers;
using Sql_Queries;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.code
{
    public class Bot_Code
    {
        static ITelegramBotClient bot = new TelegramBotClient(Config.token); // token
        static CancellationToken cancellationToken = new CancellationTokenSource().Token;
        static ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } }; // receive all update types

        public static void Main()
        {
            var db = new HealthBotContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Dispose();

            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

            Command.Initialize(bot);

            Console.Read();
        }

        public static async Task HandleUpdateAsync(
            ITelegramBotClient botclient,
            Update update,
            CancellationToken cancellationToken
        )
        {
            var db = new HealthBotContext();

            long chat_id;
            int message_id;

            if (update.Message != null)
            {
                var message = update.Message;

                if (message.Text == null)
                    return;

                chat_id = message.Chat.Id;
                message_id = message.MessageId;

                var user =
                    db.Users.FirstOrDefault(u => u.ChatId == chat_id)
                    ?? Command.User_create(update, chat_id);

                if (message.Text == "/start")
                {
                    (string, InlineKeyboardMarkup) tuple = Reply.Menu(user);
                    user.messageid = message.MessageId;

                    await Command.Update(user);
                    await Command.Destroy(chat_id, message_id);
                    await Command.Send(chat_id, tuple);
                }

                DateTime date_min;
                DateTime date_max;
                Biometry biometry;
                HealthBot.User observer;

                switch (user.LastAction)
                {
                    case "CaloriesByDate": // TODO: add check for which date is earlier + parsing if dates are wrong
                        date_min = Convert
                            .ToDateTime(message.Text.Replace(" ", "").Split("-")[0])
                            .ToUniversalTime();
                        date_max = Convert
                            .ToDateTime(message.Text.Replace(" ", "").Split("-")[1])
                            .ToUniversalTime();

                        await Command.Destroy(chat_id, message_id);
                        await Command.Send(
                            chat_id,
                            Reply.Stats(
                                $"In given time span you consumed average of {Query.average_calories_by_date(date_min, date_max, user)} calories"
                            ),
                            user.messageid
                        );
                        break;
                    case "LiquidByDate":
                        date_min = Convert
                            .ToDateTime(message.Text.Replace(" ", "").Split("-")[0])
                            .ToUniversalTime();
                        date_max = Convert
                            .ToDateTime(message.Text.Replace(" ", "").Split("-")[1])
                            .ToUniversalTime();

                        await Command.Destroy(chat_id, message_id);
                        await Command.Send(
                            chat_id,
                            Reply.Stats(
                                $"In given time span you consumed average of {Query.average_water_by_date(date_min, date_max, user)} ml of liquid"
                            ),
                            user.messageid
                        );
                        break;
                    case "AccountChangeAge":
                        user.Age = Convert.ToInt32(message.Text);

                        await Command.Destroy(chat_id, message_id);
                        await Command.Send(chat_id, Reply.Account(user), user.messageid);

                        user.LastAction = "";
                        await Command.Update(user);
                        break;
                    case "AccountChangeWeight":
                        var weight = Convert.ToInt32(message.Text);
                        biometry = db
                            .Biometries.Where(b => b.Author == user.Uuid)
                            .OrderBy(b => b.CreatedAt)
                            .FirstOrDefault();

                        if (biometry is not null && biometry.UpdatedAt.Date == DateTime.Today.Date)
                        {
                            biometry.Weight = weight;
                            await Command.Update(biometry);
                        }
                        else
                        {
                            db.Biometries.Add(
                                new Biometry() { Author = user.Uuid, Weight = weight }
                            );
                            await db.SaveChangesAsync();
                            db.Dispose();
                        }

                        user.LastAction = "";

                        await Command.Destroy(chat_id, message_id);
                        await Command.Send(chat_id, Reply.Account(user), user.messageid);
                        await Command.Update(user);
                        break;
                    case "AccountChangeHeight":
                        var height = Convert.ToInt32(message.Text);
                        biometry = db
                            .Biometries.Where(b => b.Author == user.Uuid)
                            .OrderBy(b => b.CreatedAt)
                            .FirstOrDefault();

                        if (biometry is not null && biometry.UpdatedAt.Date == DateTime.Today.Date)
                        {
                            biometry.Height = height;
                            await Command.Update(biometry);
                        }
                        else
                        {
                            db.Biometries.Add(
                                new Biometry() { Author = user.Uuid, Height = height }
                            );
                            await db.SaveChangesAsync();
                            db.Dispose();
                        }

                        user.LastAction = "";

                        await Command.Destroy(chat_id, message_id);
                        await Command.Send(chat_id, Reply.Account(user), user.messageid);
                        await Command.Update(user);
                        break;
                    case "AccountChangeSex":
                        user.Sex = Convert.ToString(message.Text);

                        await Command.Destroy(chat_id, message_id);
                        await Command.Send(chat_id, Reply.Account(user), user.messageid);

                        user.LastAction = "";
                        await Command.Update(user);
                        break;
                    case "AddAccount":
                        observer = db
                            .Users.Where(u =>
                                u.Alias == Convert.ToString(message.Text).Replace("@", "")
                            )
                            .FirstOrDefault();

                        if (observer != null)
                        {
                            user.Observers.Add(observer);
                            await Command.Send(chat_id, Reply.LinkedAccounts(user), user.messageid);
                        }
                        else
                        {
                            await Command.Send(
                                chat_id,
                                Reply.LinkedAccounts(
                                    user,
                                    "This account is not yes registered in the system."
                                ),
                                user.messageid
                            );
                        }

                        user.LastAction = "";
                        await Command.Update(user);
                        await Command.Destroy(chat_id, message_id);

                        break;
                    case "RemoveAccount":
                        observer = db
                            .Users.Where(u =>
                                u.Alias == Convert.ToString(message.Text).Replace("@", "")
                            )
                            .FirstOrDefault();

                        if (observer != null)
                        {
                            user.Observers.Remove(observer);
                            await Command.Send(chat_id, Reply.LinkedAccounts(user), user.messageid);
                        }
                        else
                        {
                            await Command.Send(
                                chat_id,
                                Reply.LinkedAccounts(
                                    user,
                                    "This user is not present in the system"
                                ),
                                user.messageid
                            );
                        }

                        user.LastAction = "";
                        await Command.Update(user);
                        await Command.Destroy(chat_id, message_id);

                        break;
                    case "AccountExport":
                        if (message.Text.ToLower() != "yes")
                        {
                            await Command.Destroy(chat_id, message_id);
                            break;
                        }

                        var last_export = db
                            .Exportdata.Where(e => e.Author == user.Uuid)
                            ?.OrderBy(e => e.CreatedAt)
                            .FirstOrDefault();

                        if (
                            last_export != null
                            && (last_export.CreatedAt - DateTime.Today).TotalDays < 14
                        )
                        {
                            await Command.Send(
                                user.ChatId,
                                Reply.AccountExport(
                                    "You are not elegible for account export at this time."
                                ),
                                user.messageid
                            );
                        }
                        else
                        {
                            var json = Sql_Queries.Query.user_data_export(user);
                            var path =
                                $"./{user.Alias}_{DateTime.Today.ToString("yyyy-MM-dd")}.json";
                            System.IO.File.AppendAllText(path, json);

                            db.Exportdata.Add(
                                new Exportdatum { Author = user.Uuid, ExportedData = json }
                            );

                            await Command.Destroy(chat_id, message_id);
                            await db.SaveChangesAsync();
                            db.Dispose();
                            await Command.Send(
                                user.ChatId,
                                Reply.AccountExport("Done"),
                                user.messageid,
                                path
                            );
                        }
                        break;
                    default:
                        if (!user.LastAction.Contains("DiaryForm"))
                            break;

                        Diaryentry entry;

                        switch (user.LastAction.Split('_')[0])
                        {
                            case "DiaryFormName":
                                var name = Convert.ToString(message.Text);
                                entry = db.Diaryentrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                user.LastAction = "";
                                entry.Name = name;
                                await Command.Update(entry);
                                await Command.Update(user);

                                await Command.Destroy(chat_id, message_id);
                                await Command.Send(
                                    chat_id,
                                    Reply.DiaryNewFrom(entry_uuid: entry.Uuid),
                                    user.messageid
                                );
                                break;
                            case "DiaryFormTags":
                                var tags = Convert.ToString(message.Text);
                                entry = db.Diaryentrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                user.LastAction = "";
                                entry.Tags = tags;
                                await Command.Update(entry);
                                await Command.Update(user);

                                await Command.Destroy(chat_id, message_id);
                                await Command.Send(
                                    chat_id,
                                    Reply.DiaryNewFrom(entry_uuid: entry.Uuid),
                                    user.messageid
                                );
                                break;
                            case "DiaryFormDate":
                                var date = Convert.ToDateTime(message.Text);
                                entry = db.Diaryentrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                user.LastAction = "";
                                entry.CreatedAt = date;
                                await Command.Update(entry);
                                await Command.Update(user);

                                await Command.Destroy(chat_id, message_id);
                                await Command.Send(
                                    chat_id,
                                    Reply.DiaryNewFrom(entry_uuid: entry.Uuid),
                                    user.messageid
                                );
                                break;
                            case "DiaryFormPressure":
                                var pressure = Convert.ToString(message.Text);
                                entry = db.Diaryentrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                user.LastAction = "";
                                entry.BloodPreassure = pressure;
                                await Command.Update(entry);
                                await Command.Update(user);

                                await Command.Destroy(chat_id, message_id);
                                await Command.Send(
                                    chat_id,
                                    Reply.DiaryNewFrom(entry_uuid: entry.Uuid),
                                    user.messageid
                                );
                                break;
                            case "DiaryFormSaturation":
                                var saturation = Convert.ToInt32(message.Text);
                                entry = db.Diaryentrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                user.LastAction = "";
                                entry.BloodSaturation = saturation;
                                await Command.Update(entry);
                                await Command.Update(user);

                                await Command.Destroy(chat_id, message_id);
                                await Command.Send(
                                    chat_id,
                                    Reply.DiaryNewFrom(entry_uuid: entry.Uuid),
                                    user.messageid
                                );
                                break;
                            case "DiaryFormRate":
                                var rate = Convert.ToInt32(message.Text);
                                entry = db.Diaryentrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                user.LastAction = "";
                                entry.HeartRate = rate;
                                await Command.Update(entry);
                                await Command.Update(user);

                                await Command.Destroy(chat_id, message_id);
                                await Command.Send(
                                    chat_id,
                                    Reply.DiaryNewFrom(entry_uuid: entry.Uuid),
                                    user.messageid
                                );
                                break;
                            case "DiaryFormIntakeState":
                                var state = Convert.ToString(message.Text);
                                entry = db.Diaryentrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                if (state == "solid" || state == "liquid")
                                {
                                    entry.State = state;
                                    await Command.Update(entry);
                                }

                                user.LastAction = "";
                                await Command.Update(user);
                                await Command.Destroy(chat_id, message_id);
                                await Command.Send(
                                    chat_id,
                                    Reply.DiaryNewFrom(entry_uuid: entry.Uuid),
                                    user.messageid
                                );
                                break;
                            case "DiaryFormIntakeWeight":
                                var intake_weight = Convert.ToInt32(message.Text);
                                entry = db.Diaryentrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                user.LastAction = "";
                                entry.Weight = intake_weight;
                                await Command.Update(entry);
                                await Command.Update(user);

                                await Command.Destroy(chat_id, message_id);
                                await Command.Send(
                                    chat_id,
                                    Reply.DiaryNewFrom(entry_uuid: entry.Uuid),
                                    user.messageid
                                );
                                break;
                            case "DiaryFormIntakeCalory":
                                var calory = Convert.ToInt32(message.Text);
                                entry = db.Diaryentrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                user.LastAction = "";
                                entry.CaloryAmount = calory;
                                await Command.Update(entry);
                                await Command.Update(user);

                                await Command.Destroy(chat_id, message_id);
                                await Command.Send(
                                    chat_id,
                                    Reply.DiaryNewFrom(entry_uuid: entry.Uuid),
                                    user.messageid
                                );
                                break;
                        }
                        break;
                }
                return;
            }
            else if (update.CallbackQuery != null)
            {
                chat_id = update.CallbackQuery.From.Id;
                var user =
                    db.Users.FirstOrDefault(u => u.ChatId == chat_id)
                    ?? Command.User_create(update, chat_id);

                user.messageid = update.CallbackQuery.Message.MessageId;
                await Command.Update(user);

                string callback_data = update.CallbackQuery.Data ?? "";

                switch (callback_data.Split('_')[0])
                {
                    case "To":
                        await State_Handlers.To_State_Handler(user, callback_data);
                        break;
                    case "Account":
                        await State_Handlers.Account_State_Handler(user, callback_data);
                        break;
                    case "Stats":
                        await State_Handlers.Stats_State_Handler(user, callback_data);
                        break;
                    case "Diary":
                        await State_Handlers.Diary_State_Handler(user, callback_data);
                        break;
                }
            }
            db.Dispose();
        }

        public static async Task HandleErrorAsync(
            ITelegramBotClient botclient,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("\n\n" + exception.ToString() + "\n\n");
            Console.ResetColor();
        } // exeption handling
    }
}
