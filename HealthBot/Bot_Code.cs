using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot;
using Configuration;
using Bot.scripts;

namespace Bot.code
{
    enum State
    {
        Menu, // menu
        Game0, // difficulty selection (input1d)
        Game1, // basic game array (input2d)
        Conclusion, // conclusion (inputbool)
        Stats // statistics (input1d)
    } // little simplification for menu navigation

    public class Bot_Code
    {
        bool bot_running = false;

        ITelegramBotClient bot = new TelegramBotClient(Config.token); // token
        List<DataModel.User> data = new List<DataModel.User> { }; // enables work for multiple user
        static CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken cancellationToken = cts.Token;
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } }; // receive all update types

        public void Main()
        {
            bot.StartReceiving
            (
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botclient, Update update, CancellationToken cancellationToken)
        {
            long chat_id;
            int message_id;

            Message message;

            if (update.Message != null)
            {
                message = update.Message;
                chat_id = update.Message.Chat.Id;

                if (data.Find(u => u.ChatId == chat_id) == null) Command.User_load(update);

                if (message.Text != null && message.Text == $"/start")
                {
                    data.Find(u => u.ChatId == chat_id).State = State.Menu.ToString();

                    await Command.Send(chat_id, $"Welcome!", Reply_Keyboards.Menu());
                }
                return;
            }
            else if (update.CallbackQuery != null)
            {
                string callback_data;

                chat_id = update.CallbackQuery.From.Id;

                if (data.Find(u => u.ChatId == chat_id) == null) Command.User_load(update);

                callback_data = update.CallbackQuery.Data;
                message_id = update.CallbackQuery.Message.MessageId;
            }
            else return;
        }

        public async Task HandleErrorAsync(ITelegramBotClient botclient, Exception exception, CancellationToken cancellationToken)
        {
        }// exeption handling
    }
}