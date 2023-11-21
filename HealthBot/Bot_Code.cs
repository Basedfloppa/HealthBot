using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot;
using Configuration;
using Bot.scripts;
using HealthBot.handlers;
using Telegram.Bot.Types.ReplyMarkups;
using System.Security.AccessControl;

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
        static Dictionary<long, DataModel.User> data = new Dictionary<long, DataModel.User> { }; // enables work for multiple user
        static CancellationToken cancellationToken = new CancellationTokenSource().Token;
        static ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } }; // receive all update types

        public static void Main()
        {
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

                    await Command.Send(chat_id, tuple);

                    await bot.DeleteMessageAsync(chat_id, message_id);
                }

                switch (Command.data[chat_id].LastAction)
                {
                    case "CaloriesByDate":
                        break;
                    case "LiquidByDate":
                        break;
                }

                return;
            }
            else if (update.CallbackQuery != null)
            {
                chat_id = update.CallbackQuery.From.Id;

                if (data[chat_id] == null) Command.User_load(update);

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