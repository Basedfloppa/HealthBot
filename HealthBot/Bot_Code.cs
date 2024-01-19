using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot;
using Configuration;
using Bot.scripts;
using HealthBot.handlers;
using Telegram.Bot.Types.ReplyMarkups;
using User = HealthBot.User;
using HealthBot;
using System.Data.Common;
using Sql_Queries;
using System.Security.Cryptography;
using System.Data;
using System.Net;

namespace Bot.code
{  
    public enum State
    {
        Menu,
        Account,
        AccountChange,
        AccountExport,
        AccountSubscription,
        LinkedAccounts,
        AddAccount,
        RemoveAccount,
        Diary,
        SearchDiary,
        AddToDiary,
        Stats
    }
    
    public class Bot_Code
    {
        static ITelegramBotClient bot = new TelegramBotClient(Config.token); // token
        static Dictionary<long, User> data = new Dictionary<long, User> { }; // enables work for multiple user
        static CancellationToken cancellationToken = new CancellationTokenSource().Token;
        static ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } }; // receive all update types

        public static void Main()
        {
            var db = new HealthBotContext();
            db.Database.EnsureCreated();
            db.Dispose();

            bot.StartReceiving
            (
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            
            Command.Initialize(data, bot);

            Console.Read();
        }
        public static async Task HandleUpdateAsync(ITelegramBotClient botclient, Update update, CancellationToken cancellationToken)
        {
            long chat_id;
            int message_id;
            Message message;
            
            if (update.Message != null)
            {
                message = update.Message;
                message_id = update.Message.MessageId;
                chat_id = update.Message.Chat.Id;

                if (!data.TryGetValue(chat_id, out var a)) Command.User_load(update);

                if (message.Text != null && message.Text == "/start")
                {
                    data[chat_id].State = State.Menu.ToString();

                    (string, InlineKeyboardMarkup) tuple = Reply.Menu(chat_id);
                    data[chat_id].messageid = message.MessageId;

                    await Command.Destroy(chat_id, message_id);
                    await Command.Send(chat_id, tuple);
                }

                if (message.Text == null) return;  

                DateTime date_min;
                DateTime date_max;
                var db = new HealthBotContext();

                switch (Command.data[chat_id].LastAction)
                {
                    case "CaloriesByDate": // TODO: add check for which date is earlier + parsing if dates are wrong
                        date_min = Convert.ToDateTime(message.Text.Replace(" ", "").Split("-")[0]);
                        date_max = Convert.ToDateTime(message.Text.Replace(" ", "").Split("-")[1]);

                        await Command.Destroy(chat_id, message_id);
                        await Command.Send(chat_id, Reply.Stats($"In given time span you consumed average of {Query.average_calories_by_date(date_min, date_max, data[chat_id])} calories"), data[chat_id].messageid);
                        break;
                    case "LiquidByDate":
                        date_min = Convert.ToDateTime(message.Text.Replace(" ", "").Split("-")[0]);
                        date_max = Convert.ToDateTime(message.Text.Replace(" ", "").Split("-")[1]);

                        await Command.Destroy(chat_id, message_id);
                        await Command.Send(chat_id, Reply.Stats($"In given time span you consumed average of {Query.average_calories_by_date(date_min, date_max, data[chat_id])} ml of liquid"), data[chat_id].messageid);
                        break;
                    case "AccountChangeAge":
                        data[chat_id].Age = Convert.ToInt32(message.Text);

                        await Command.Destroy(chat_id, message_id);
                        await Command.Send(chat_id, Reply.Account(chat_id), data[chat_id].messageid);

                        db.Update(data[chat_id]);
                        db.SaveChangesAsync();
                        data[chat_id].LastAction = "";
                        break;
                    case "AccountChangeWeight":
                        var weight = Convert.ToInt32(message.Text); 
                        
                        if (data[chat_id].Biometries.Count > 0 && data[chat_id].Biometries.LastOrDefault().ChangedAt == DateTime.Today.ToUniversalTime()) 
                        {
                            data[chat_id].Biometries.LastOrDefault().Weight = weight;
                        } 
                        else
                        {
                            db.Biometries.Add(new Biometry() { Author = data[chat_id].Uuid, Weight = weight});
                        }

                        await Command.Destroy(chat_id, message_id);
                        await Command.Send(chat_id, Reply.Account(chat_id), data[chat_id].messageid);

                        await db.SaveChangesAsync();
                        data[chat_id].LastAction = "";
                        break;
                    case "AccountChangeHeight":
                        var height = Convert.ToInt32(message.Text); 
                        
                        if (data[chat_id].Biometries.Last().ChangedAt == DateTime.Today.ToUniversalTime()) 
                        {
                            data[chat_id].Biometries.Last().Weight = height;
                        } 
                        else
                        {
                            db.Biometries.Add(new Biometry() { Author = data[chat_id].Uuid, Weight = height});
                        }

                        await Command.Destroy(chat_id, message_id);
                        await Command.Send(chat_id, Reply.Account(chat_id), data[chat_id].messageid);

                        await db.SaveChangesAsync();
                        data[chat_id].LastAction = "";
                        break;
                    case "AccountChangeSex":
                        data[chat_id].Sex = Convert.ToString(message.Text);

                        await Command.Destroy(chat_id, message_id);
                        await Command.Send(chat_id, Reply.Account(chat_id), data[chat_id].messageid);
                        
                        db.Update(data[chat_id]);
                        db.SaveChangesAsync();
                        data[chat_id].LastAction = "";
                        break;
                }
                return;
            }
            else if (update.CallbackQuery != null)
            {
                chat_id = update.CallbackQuery.From.Id;

                if (!data.TryGetValue(chat_id, out var a)) Command.User_load(update);

                data[chat_id].messageid = update.CallbackQuery.Message.MessageId;

                string callback_data = update.CallbackQuery.Data ?? "";

                switch (callback_data.Split('_')[0])
                {
                    case "To":
                        await State_Handlers.To_State_Handler(chat_id, callback_data, update.CallbackQuery.Message.MessageId);
                        break;
                    case "Account":
                        await State_Handlers.Account_State_Handler(chat_id, callback_data, update.CallbackQuery.Message.MessageId);
                        break;
                    case "Stats":
                        await State_Handlers.Stats_State_Handler(chat_id, callback_data, update.CallbackQuery.Message.MessageId);
                        break;
                    case "Diary":
                        break;
                    default:
                        break;
                }
                
            }
            else return;
        }
        public static async Task HandleErrorAsync(ITelegramBotClient botclient, Exception exception, CancellationToken cancellationToken)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("\n\n" + exception.ToString() + "\n\n");
            Console.ResetColor();
        } // exeption handling

    }
}