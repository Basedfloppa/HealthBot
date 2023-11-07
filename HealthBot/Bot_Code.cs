using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot;
using Configuration;
using Bot.scripts;
using LinqToDB.Data;

namespace Bot.code
{
    enum State
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
        static CancellationTokenSource cts = new CancellationTokenSource();
        static CancellationToken cancellationToken = cts.Token;
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

                if (!data.TryGetValue(chat_id, out var a )) Command.User_load(update);

                if (message.Text != null && message.Text == "/start")
                {
                    data[chat_id].State = State.Menu.ToString();

                    Reply.Account(chat_id);
                    await bot.DeleteMessageAsync(chat_id, message_id);
                }
                return;
            }
            else if (update.CallbackQuery != null)
            {
                string callback_data;

                chat_id = update.CallbackQuery.From.Id;

                if (data[chat_id] == null) Command.User_load(update);

                callback_data = update.CallbackQuery.Data;
            }
            else return;
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botclient, Exception exception, CancellationToken cancellationToken)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("\n\n" + exception.ToString() + "\n\n");
            Console.ResetColor();
        }// exeption handling
    }
}