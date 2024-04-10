using System.Globalization;
using Bot.scripts;
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

        public static async Task HandleUpdateAsync(ITelegramBotClient botclient, Update update, CancellationToken cancellationToken)
        {
            HealthBotContext db = new();
            long chat_id;
            int message_id;

            if (update.Message != null)
            {
                var message = update.Message;

                if (message.Text == null) return;

                chat_id = message.Chat.Id;
                message_id = message.MessageId;

                HealthBot.User user =
                    db.Users.FirstOrDefault(u => u.ChatId == chat_id)
                    ?? Command.Database.User_create(update, chat_id);

                if (message.Text == "/start")
                {
                    (string, InlineKeyboardMarkup) tuple = Reply.Menu(user);
                    user.MessageId = message.MessageId;

                    await Command.Database.Update(user);
                    await Command.Message.Destroy(chat_id, message_id);
                    await Command.Message.Send(chat_id, tuple);
                }
                
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine($"User: {user.Alias} , sent: {message.Text} , from: {chat_id}");
                Console.ResetColor();

                DateTime date_min;
                DateTime date_max;
                Biometry? biometry;
                HealthBot.User? observer;

                switch (user.LastAction)
                {
                    case "CaloriesByDate":
                        try
                        {
                            date_min = Convert.ToDateTime(message.Text.Replace(" ", "").Split("-")[0]).ToUniversalTime();
                            date_max = Convert.ToDateTime(message.Text.Replace(" ", "").Split("-")[1]).ToUniversalTime();

                            if (date_max.Subtract(date_min).TotalDays < 0) (date_min, date_max) = (date_max, date_min);

                            var (dates, values) = Sql_Queries.Query.calories_by_date(date_max, date_min, user);

                            string graphicsFilePath = await Command.Database.Generate_graphics(dates, values, "Calories");

                            if (!string.IsNullOrEmpty(graphicsFilePath))
                            {
                                await Command.Message.Destroy(chat_id, message_id);
                                using (var fileStream = System.IO.File.OpenRead(graphicsFilePath))
                                {
                                    await Command.Message.Send(chat_id, Reply.Stats("Your caption here") , user.MessageId, graphicsFilePath);
                                }
                                System.IO.File.Delete(graphicsFilePath);
                            }
                            else
                            {
                                await Command.Message.Send(chat_id, ("Failed to generate graphics.", null), user.MessageId);
                            }
                
                        } 
                        catch (Exception e)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Date conversion method encountered {e.Message}");
                            Console.ResetColor();
                        } 
                        break;
                    case "LiquidByDate":
                        try
                        {
                            date_min = Convert
                                    .ToDateTime(message.Text.Replace(" ", "").Split("-")[0])
                                    .ToUniversalTime();
                            date_max = Convert
                                    .ToDateTime(message.Text.Replace(" ", "").Split("-")[1])
                                    .ToUniversalTime();

                            if (date_max.Subtract(date_min).TotalDays < 0) (date_min, date_max) = (date_max, date_min);

                            var (dates, values) = Sql_Queries.Query.water_by_date(date_min, date_max, user);

                            string graphicsFilePath = await Command.Database.Generate_graphics(dates, values, "Liquid");

                            if (!string.IsNullOrEmpty(graphicsFilePath))
                            {
                                await Command.Message.Destroy(chat_id, message_id);
                                using (var fileStream = System.IO.File.OpenRead(graphicsFilePath))
                                {
                                    await Command.Message.Send(chat_id, Reply.Stats("Your caption here"), user.MessageId, graphicsFilePath);
                                }
                                System.IO.File.Delete(graphicsFilePath);
                            }
                            else
                            {
                                await Command.Message.Send(chat_id, ("Failed to generate graphics.", null), user.MessageId);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Date conversion method encountered {e.Message}");
                            Console.ResetColor();
                        }
                        break;
                    case "AccountChangeAge":
                        try
                        {
                            user.Age = Convert.ToInt32(message.Text);
                        }
                        catch (Exception e)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Int conversion method encountered {e.Message} in AccountChangeAge");
                            Console.ResetColor();
                        }

                        await Command.Message.Destroy(chat_id, message_id);
                        await Command.Message.Send(chat_id, Reply.AccountChange(user), user.MessageId);

                        user.LastAction = "";
                        await Command.Database.Update(user);
                        break;
                    case "AccountChangeWeight":
                        try
                        {
                            float weight = float.Parse(message.Text.Replace(",","."), CultureInfo.InvariantCulture);

                            biometry = db
                            .Biometries.Where(b => b.Author == user.ChatId)
                            .OrderBy(b => b.CreatedAt)
                            .FirstOrDefault();

                            if (biometry != null && biometry.UpdatedAt.Date == DateTime.Today.Date)
                            {
                                biometry.Weight = weight;
                                await Command.Database.Update(biometry);
                            }
                            else
                            {
                                db.Biometries.Add(
                                    new Biometry() { Author = user.ChatId, Weight = weight }
                                );
                                await db.SaveChangesAsync();
                                db.Dispose();
                            }

                            user.LastAction = "";

                            await Command.Message.Destroy(chat_id, message_id);
                            await Command.Message.Send(chat_id, Reply.AccountChange(user), user.MessageId);
                            await Command.Database.Update(user);
                        }
                        catch (Exception e)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Foat conversion method encountered {e.Message} in AccountChangeWeight");
                            Console.ResetColor();
                        }

                        break;
                    case "AccountChangeHeight":
                        try
                        {
                            float height = float.Parse(message.Text.Replace(",","."), CultureInfo.InvariantCulture);
                            biometry = db
                                .Biometries.Where(b => b.Author == user.ChatId)
                                .OrderBy(b => b.CreatedAt)
                                .FirstOrDefault();

                            if (biometry != null && biometry.UpdatedAt.Date == DateTime.Today.Date)
                            {
                                biometry.Height = height;
                                await Command.Database.Update(biometry);
                            }
                            else
                            {
                                db.Biometries.Add(
                                    new Biometry() { Author = user.ChatId, Height = height }
                                );
                                await db.SaveChangesAsync();
                                db.Dispose();
                            }

                            user.LastAction = "";

                            await Command.Message.Destroy(chat_id, message_id);
                            await Command.Message.Send(chat_id, Reply.AccountChange(user), user.MessageId);
                            await Command.Database.Update(user);
                        }
                        catch (Exception e)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Foat conversion method encountered {e.Message} in AccountChangeHeight");
                            Console.ResetColor();
                        }
                        break;
                    case "AccountChangeSex":
                        user.Sex = Convert.ToString(message.Text);

                        await Command.Message.Destroy(chat_id, message_id);
                        await Command.Message.Send(chat_id, Reply.AccountChange(user), user.MessageId);

                        user.LastAction = "";
                        await Command.Database.Update(user);
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
                            await Command.Message.Send(chat_id, Reply.LinkedAccounts(user), user.MessageId);
                        }
                        else
                        {
                            await Command.Message.Send(
                                chat_id,
                                Reply.LinkedAccounts(
                                    user,
                                    "This account is not yes registered in the system."
                                ),
                                user.MessageId
                            );
                        }

                        user.LastAction = "";
                        await Command.Database.Update(user);
                        await Command.Message.Destroy(chat_id, message_id);
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
                            await Command.Message.Send(chat_id, Reply.LinkedAccounts(user), user.MessageId);
                        }
                        else
                        {
                            await Command.Message.Send(
                                chat_id,
                                Reply.LinkedAccounts(
                                    user,
                                    "This user is not present in the system"
                                ),
                                user.MessageId
                            );
                        }

                        user.LastAction = "";
                        await Command.Database.Update(user);
                        await Command.Message.Destroy(chat_id, message_id);

                        break;
                    case "AccountExport":
                        if (message.Text.ToLower() != "yes")
                        {
                            await Command.Message.Destroy(chat_id, message_id);
                            break;
                        }

                        Exportdatum? last_export = db
                            .ExportData.Where(e => e.Author == user.ChatId)
                            ?.OrderBy(e => e.CreatedAt)
                            .FirstOrDefault();

                        if (
                            last_export != null
                            && (last_export.CreatedAt - DateTime.Today).TotalDays < 14
                        )
                        {
                            await Command.Message.Send(
                                user.ChatId,
                                Reply.AccountExport(
                                    "You are not elegible for account export at this time."
                                ),
                                user.MessageId
                            );
                        }
                        else
                        {
                            var json = Sql_Queries.Query.user_data_export(user);
                            var path =
                                $"./{user.Alias}_{DateTime.Today.ToString("yyyy-MM-dd")}.json";
                            System.IO.File.AppendAllText(path, json);

                            db.ExportData.Add(
                                new Exportdatum { Author = user.ChatId, ExportedData = json }
                            );

                            await Command.Message.Destroy(chat_id, message_id);
                            await db.SaveChangesAsync();
                            db.Dispose();
                            await Command.Message.Send(
                                user.ChatId,
                                Reply.AccountExport("Done"),
                                user.MessageId,
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
                                entry = db.DiaryEntrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                user.LastAction = "";
                                entry.Name = name;
                                await Command.Database.Update(entry);
                                await Command.Database.Update(user);

                                await Command.Message.Destroy(chat_id, message_id);
                                await Command.Message.Send(
                                    chat_id,
                                    Reply.DiaryEntryForm(entry_uuid: entry.Uuid),
                                    user.MessageId
                                );
                                break;
                            case "DiaryFormTags":
                                var tags = Convert.ToString(message.Text);
                                entry = db.DiaryEntrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                user.LastAction = "";
                                entry.Tags = tags;
                                await Command.Database.Update(entry);
                                await Command.Database.Update(user);

                                await Command.Message.Destroy(chat_id, message_id);
                                await Command.Message.Send(
                                    chat_id,
                                    Reply.DiaryEntryForm(entry_uuid: entry.Uuid),
                                    user.MessageId
                                );
                                break;
                            case "DiaryFormDate":
                                try
                                {
                                    var date = Convert.ToDateTime(message.Text).ToUniversalTime();
                                    entry = db.DiaryEntrys.Find(
                                        Guid.Parse(user.LastAction.Split('_')[1])
                                    );

                                    user.LastAction = "";
                                    entry.CreatedAt = date;
                                    await Command.Database.Update(entry);
                                    await Command.Database.Update(user);

                                    await Command.Message.Destroy(chat_id, message_id);
                                    await Command.Message.Send(
                                        chat_id,
                                        Reply.DiaryEntryForm(entry_uuid: entry.Uuid),
                                        user.MessageId
                                    );
                                }
                                catch (Exception e)
                                {
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Date conversion method encountered {e.Message} in DiaryFormDate");
                                    Console.ResetColor();
                                }

                                break;
                            case "DiaryFormPressure":
                                var pressure = Convert.ToString(message.Text);
                                entry = db.DiaryEntrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                user.LastAction = "";
                                entry.BloodPreassure = pressure;
                                await Command.Database.Update(entry);
                                await Command.Database.Update(user);

                                await Command.Message.Destroy(chat_id, message_id);
                                await Command.Message.Send(
                                    chat_id,
                                    Reply.DiaryEntryForm(entry_uuid: entry.Uuid),
                                    user.MessageId
                                );
                                break;
                            case "DiaryFormSaturation":
                                try
                                {
                                    float saturation = float.Parse(message.Text.Replace(",","."), CultureInfo.InvariantCulture);
                                    entry = db.DiaryEntrys.Find(
                                        Guid.Parse(user.LastAction.Split('_')[1])
                                    );

                                    user.LastAction = "";
                                    entry.BloodSaturation = saturation;
                                    await Command.Database.Update(entry);
                                    await Command.Database.Update(user);

                                    await Command.Message.Destroy(chat_id, message_id);
                                    await Command.Message.Send(
                                        chat_id,
                                        Reply.DiaryEntryForm(entry_uuid: entry.Uuid),
                                        user.MessageId
                                    );
                                }
                                catch (Exception e)
                                {
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Float conversion method encountered {e.Message} in DiaryFormSaturation");
                                    Console.ResetColor();
                                }
                                
                                break;
                            case "DiaryFormRate":
                                try
                                {
                                    float rate = float.Parse(message.Text.Replace(",","."), CultureInfo.InvariantCulture);
                                    entry = db.DiaryEntrys.Find(
                                        Guid.Parse(user.LastAction.Split('_')[1])
                                    );

                                    user.LastAction = "";
                                    entry.HeartRate = rate;
                                    await Command.Database.Update(entry);
                                    await Command.Database.Update(user);

                                    await Command.Message.Destroy(chat_id, message_id);
                                    await Command.Message.Send(
                                        chat_id,
                                        Reply.DiaryEntryForm(entry_uuid: entry.Uuid),
                                        user.MessageId
                                    );
                                }
                                catch (Exception e)
                                {
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Float conversion method encountered {e.Message} in DiaryFormSaturation");
                                    Console.ResetColor();
                                }
                                
                                break;
                            case "DiaryFormIntakeState":
                                var state = Convert.ToString(message.Text);
                                entry = db.DiaryEntrys.Find(
                                    Guid.Parse(user.LastAction.Split('_')[1])
                                );

                                if (state == "solid" || state == "liquid")
                                {
                                    entry.State = state;
                                    await Command.Database.Update(entry);
                                }

                                user.LastAction = "";
                                await Command.Database.Update(user);
                                await Command.Message.Destroy(chat_id, message_id);
                                await Command.Message.Send(
                                    chat_id,
                                    Reply.DiaryEntryForm(entry_uuid: entry.Uuid),
                                    user.MessageId
                                );
                                break;
                            case "DiaryFormIntakeWeight":
                                try
                                {
                                    float intake_weight = float.Parse(message.Text.Replace(",","."), CultureInfo.InvariantCulture);
                                    entry = db.DiaryEntrys.Find(
                                        Guid.Parse(user.LastAction.Split('_')[1])
                                    );

                                    user.LastAction = "";
                                    entry.Weight = intake_weight;
                                    await Command.Database.Update(entry);
                                    await Command.Database.Update(user);

                                    await Command.Message.Destroy(chat_id, message_id);
                                    await Command.Message.Send(
                                        chat_id,
                                        Reply.DiaryEntryForm(entry_uuid: entry.Uuid),
                                        user.MessageId
                                    );
                                }
                                catch (Exception e)
                                {
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Float conversion method encountered {e.Message} in DiaryFormIntakeWeight");
                                    Console.ResetColor();
                                }

                                break;
                            case "DiaryFormIntakeCalory":
                                try
                                {
                                    float calory = float.Parse(message.Text.Replace(",","."), CultureInfo.InvariantCulture);
                                    entry = db.DiaryEntrys.Find(
                                        Guid.Parse(user.LastAction.Split('_')[1])
                                    );

                                    user.LastAction = "";
                                    entry.CaloryAmount = calory;
                                    await Command.Database.Update(entry);
                                    await Command.Database.Update(user);

                                    await Command.Message.Destroy(chat_id, message_id);
                                    await Command.Message.Send(
                                        chat_id,
                                        Reply.DiaryEntryForm(entry_uuid: entry.Uuid),
                                        user.MessageId
                                    );
                                }
                                catch (Exception e)
                                {
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Float conversion method encountered {e.Message} in DiaryFormIntakeWeight");
                                    Console.ResetColor();
                                }

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
                    ?? Command.Database.User_create(update, chat_id);

                user.MessageId = update.CallbackQuery.Message.MessageId;
                await Command.Database.Update(user);

                string callback_data = update.CallbackQuery.Data ?? "";

                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine($"User: {user.Alias} , pressed button: {callback_data} , from: {chat_id}");
                Console.ResetColor();

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
                    case "Admin":
                        await State_Handlers.Admin_State_Handler(user, callback_data);
                        break;
                }
            }
            db.Dispose();
        }

        public static async Task HandleErrorAsync( ITelegramBotClient botclient, Exception exception, CancellationToken cancellationToken)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("\n\n" + exception.ToString() + "\n\n");
            Console.ResetColor();
        } // exeption handling
    }
}
