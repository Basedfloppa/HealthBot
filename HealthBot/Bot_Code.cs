using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot;
using _Config;
using Bot.scripts;
using static System.Reflection.Metadata.BlobBuilder;
using System.Net.NetworkInformation;

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
        Dictionary<long, User_Instance> data = new Dictionary<long, User_Instance> { }; // enables work for multiple user
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
            long cid;
            string calldata;
            int messageid;
            Message message;
            if (update.Message != null)
            {
                message = update.Message;
                cid = update.Message.Chat.Id;
                if (!data.ContainsKey(cid)) Command.User_load(update);
                
                if (update.Message.Text != null && update.Message.Text == $"/start")
                {
                    data[cid].state = State.Menu;
                    await Command.Send(cid, $"Welcome!", Reply_Keyboards.Menu());
                }
                return;
            }
            else if (update.CallbackQuery != null)
            {
                cid = update.CallbackQuery.From.Id;
                calldata = update.CallbackQuery.Data;
                messageid = update.CallbackQuery.Message.MessageId;
            }
            else return;

        }

        public async Task HandleErrorAsync(ITelegramBotClient botclient, Exception exception, CancellationToken cancellationToken)
        {
        }// exeption handling
    }
}