using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot;
using Configuration;
using Bot.scripts;
using LinqToDB.Data;
using static System.Reflection.Metadata.BlobBuilder;
using System.Net.NetworkInformation;
using System.Reflection.Emit;

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
                    await Command.Send(cid, $"Welcome!", ReplyKeyboards.Menu());
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

            if (!data.ContainsKey(cid)) Command.User_load(update);

            switch (data[cid].state)
            {
                case State.Menu:
                    {
                        await HandleMenuState(cid, calldata, messageid);
                        break;
                    }
                case State.Account:
                    {
                        await HandleGame0State(cid, calldata, messageid);
                        break;
                    }
                case State.LinkedAccounts:
                    {
                        await HandleGame1State(cid, calldata, messageid);
                        break;
                    }
                case State.AccountChange:
                    {
                        await HandleConclusionState(cid, calldata, messageid);
                        break;
                    }
                case State.AccountExport:
                    {
                        await HandleStatsState(cid, calldata, messageid);
                        break;
                    }
                case State.AddAccount:
                    {
                        await HandleStatsState(cid, calldata, messageid);
                        break;
                    }
                case State.RemoveAccount:
                    {
                        await HandleStatsState(cid, calldata, messageid);
                        break;
                    }
                case State.AccountSubscription:
                    {
                        await HandleStatsState(cid, calldata, messageid);
                        break;
                    }
                case State.Stats:
                    {
                        await HandleStatsState(cid, calldata, messageid);
                        break;
                    }
                case State.Diary:
                    {
                        await HandleStatsState(cid, calldata, messageid);
                        break;
                    }
                case State.SearchDiary:
                    {
                        await HandleStatsState(cid, calldata, messageid);
                        break;
                    }
                case State.AddToDiary:
                    {
                        await HandleStatsState(cid, calldata, messageid);
                        break;
                    }
            }
        }
        public async Task HandleMenuState(long cid, string calldata, int messageid)
        {
            switch (calldata)
            {
                case $"Menu": //???
                    {
                        data[cid].state = State.Game0;
                        await Command.Send(cid, $"Select a difficulty", ReplyKeyboards.Input1d(), messageid);
                        break;
                    }
                case $"stats":
                    {
                        heatmaps = _Sql_Data.GetInf();
                        data[cid].state = State.Stats;
                        await Command.Send(cid, $"Select number you want to see stats on", ReplyKeyboards.Input1d(), messageid);
                        break;
                    }
            }
        }
        public async Task HandleGame0State(long cid, string calldata, int messageid)
        {
            int diff = Convert.ToInt32(calldata) * 4;
            data[cid].game.EnforceDifficulty(diff);
            data[cid].state = State.Game1;
            await Command.Send(cid, ReplyKeyboards.Input2d(data[cid].game.Elements), messageid);
        }
        public async Task HandleGame1State(long cid, string calldata, int messageid)
        {
            int a;

            if (int.TryParse(calldata, out a))
            {
                if (calldata.Length == 2)
                    await Command.Send(cid, $"Select number to input in a cell", ReplyKeyboards.Input1d($"{calldata}"), messageid);
                if (calldata.Length == 3 && data[cid].game.IsValid(calldata[1] - '0', calldata[2] - '0', calldata[0] - '0'))
                {
                    data[cid].game.Write(calldata[1] - '0', calldata[2] - '0', Convert.ToString(calldata[0]));
                    await Command.Send(cid, ReplyKeyboards.Input2d(data[cid].game.Elements), messageid);
                }
                else if (calldata.Length == 3 && !data[cid].game.IsValid(calldata[1], calldata[2], calldata[0] - '0'))
                {
                    await Command.Send(cid, $"Selecput input is not possible", ReplyKeyboards.Input2d(data[cid].game.Elements), messageid);
                }
                return;
            }

            switch (calldata)
            {
                case "prediction":
                    {
                        await Command.Send(cid, $"Select cell to check possible numbers", ReplyKeyboards.Input2d(data[cid].game.Elements, "p"), messageid);
                        break;
                    }
                case "exit":
                    {
                        data[cid].state = State.Menu;
                        data[cid].game.Reset();
                        await Command.Send(cid, $"Welcome!", ReplyKeyboards.Menu(), messageid);
                        break;
                    }
            }

            if (calldata.Contains("p") && int.TryParse(calldata.Replace("p", ""), out a))
                await Command.Send(cid, $"Posible inputs for cell {calldata[0]},{calldata[1]} are : {data[cid].game.Prediction(calldata[0] - '0', calldata[1] - '0')}", ReplyKeyboards.Input2d(data[cid].game.Elements), messageid);

            switch (data[cid].game.IsSolvable())
            {
                case 0:
                    {
                        data[cid].state = State.Conclusion;
                        await Command.Send(cid, "You lose, array is unsolvable now :(", ReplyKeyboards.InputBool(), messageid);
                        break;
                    }
                case 1:
                    {
                        data[cid].state = State.Conclusion;
                        await Command.Send(cid, "You won! :)", ReplyKeyboards.InputBool(), messageid);
                        break;
                    }
                case -1:
                    break;
            }
        }
        public async Task HandleConclusionState(long cid, string calldata, int messageid)
        {
            switch (calldata)
            {
                case "yes":
                    {
                        data[cid].game.Reset();
                        data[cid].state = State.Game0;
                        await Command.Send(cid, $"", ReplyKeyboards.Input2d(data[cid].game.Elements), messageid);
                        break;
                    }
                case "no":
                    {
                        data[cid].game.Reset();
                        data[cid].state = State.Menu;
                        await Command.Send(cid, $"Welcome!", ReplyKeyboards.Menu());
                        break;
                    }
            }
        }
        public async Task HandleStatsState(long cid, string calldata, int messageid)
        {
            int a;

            if (int.TryParse(calldata, out a))
            {
                string ren = "";
                foreach (var item in heatmaps[Convert.ToInt32(calldata) - 1]) ren += item;
                try { await Command.Send(cid, Command.Render(ren), ReplyKeyboards.Input1dEx(), messageid); }
                catch { }
            }

            if (calldata == "exit")
            {
                data[cid].state = State.Menu;
                await Command.Send(cid, $"Welcome!", ReplyKeyboards.Menu(), messageid);
            }
        }
        public async Task HandleErrorAsync(ITelegramBotClient botclient, Exception exception, CancellationToken cancellationToken)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("\n\n" + exception.ToString() + "\n\n");
            Console.ResetColor();
        }// exeption handling
        public override void _Ready()
        {
            _logs = (RichTextLabel)GetNode("/root/Botcode/TCon/Logs/log");
            _status = (Label)GetNode("/root/Botcode/TCon/Buttons/status");
            int[,] item = new int[9, 9];
            for (int i = 0; i < 3; i++) heatmaps.Add(item);
            Command.Initialize(data, bot, _logs);
        }
        public void _on_exit_pressed()
        {
            Command.Exit_seq();
        }
        public void _on_start_pressed()
        {
            Command.Start_seq();
        }
        public void _on_onoff_pressed()
        {
            if (!bot_running)
            {
                bot.StartReceiving
                (
                    HandleUpdateAsync,
                HandleErrorAsync,
                    receiverOptions,
                    cancellationToken
                );
                _status.Text = $"статус: включен";
                _logs.Text += $"Started bot " + bot.GetMeAsync().Result.FirstName + $"\n";
                bot_running = true;
            }
        }
        public override void _Process(double delta)
        {
        }







        /*
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
        }// exeption handling  */

    }
}