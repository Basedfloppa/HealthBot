using Bot.scripts;
using Bot.code;
using Telegram.Bot.Types.ReplyMarkups;

namespace HealthBot.handlers
{
    internal class State_Handlers
    {
        public static async Task To_State_Handler(long chat_id, string callback_data, int message_id)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch (callback_data.Split('_')[1])
            {
                case "Menu":
                    tuple = Reply.Menu(chat_id);

                    Command.data[chat_id].State = State.Menu.ToString();

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "Account":
                    tuple = Reply.Account(chat_id);

                    Command.data[chat_id].State = State.Account.ToString();

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "AccountChange":
                    tuple = Reply.AccountChange(chat_id);

                    Command.data[chat_id].State = State.AccountChange.ToString();

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "AccountExport":
                    tuple = Reply.AccountExport();

                    Command.data[chat_id].State = State.AccountExport.ToString();

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "AccountSubscription":
                    Command.data[chat_id].State = State.AccountSubscription.ToString(); // TODO: Chainge to reply keyboard with payment
                    break;
                case "LinkedAccounts":
                    tuple = Reply.LinkedAccounts(chat_id);

                    Command.data[chat_id].State = State.LinkedAccounts.ToString();

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "AddAccount":
                    tuple = Reply.AddAccount();

                    Command.data[chat_id].State = State.AddAccount.ToString();

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "RemoveAccount":
                    tuple = Reply.RemoveAccount();

                    Command.data[chat_id].State = State.RemoveAccount.ToString();

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "Diary":
                    tuple = Reply.Diary();

                    Command.data[chat_id].State = State.Diary.ToString();

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "SearchDiary":
                    tuple = Reply.SearchDiary();

                    Command.data[chat_id].State = State.SearchDiary.ToString();

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "AddToDiary":
                    tuple = Reply.AddToDiary();

                    Command.data[chat_id].State = State.AddToDiary.ToString();

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "Stats":
                    tuple = Reply.Stats();

                    Command.data[chat_id].State = State.Stats.ToString();

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                default:
                    break;
            }
        }
        public static async Task Account_State_Handler(long chat_id, string callback_data, int message_id)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch (callback_data.Split('_')[1])
            {
                case "Change":
                    tuple = Reply.AccountChange(chat_id);

                    Command.data[chat_id].State = State.AccountChange.ToString();

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                default:
                    break;
            }
        }
        public static async Task Account_Change_State_Handler(long chat_id, string callback_data, int message_id)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch (callback_data.Split('_')[2])
            {
                case "Age":
                    tuple = Reply.AccountChange(chat_id, "Input your current age.");

                    Command.data[chat_id].LastAction = "AccountChangeAge";

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "Weight":
                    tuple = Reply.AccountChange(chat_id, "Input your current weight.");

                    Command.data[chat_id].LastAction = "AccountChangeWeight";

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "Sex":
                    tuple = Reply.AccountChange(chat_id, "Input your sex.");

                    Command.data[chat_id].LastAction = "AccountChangeSex";

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                default:
                    break;
            }
        }
        public static async Task Stats_State_Handler(long chat_id, string callback_data, int message_id)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch (callback_data.Split('_')[3])
            {
                case "CaloriesByDate":
                    tuple = Reply.Stats("Input two dates in format dd.mm.yy-dd.mm.yy where first date is less than second.");

                    Command.data[chat_id].LastAction = "CaloriesByDate";

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                case "LiquidByDate":
                    tuple = Reply.Stats("Input two dates in format dd.mm.yy-dd.mm.yy where first date is less than second.");

                    Command.data[chat_id].LastAction = "LiquidByDate";

                    await Command.Send(chat_id, tuple, message_id);
                    break;
                default:
                    break;
            }
        }
    }
}
