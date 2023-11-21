using Bot.scripts;
using Bot.code;

namespace HealthBot.handlers
{
    internal class State_Handlers
    {
        public static Task To_State_Handler(long chat_id, string callback_data, int message_id)
        {
            switch (callback_data.Split('_')[1])
            {
                case "Menu":
                    Command.data[chat_id].State = State.Menu.ToString();
                    Reply.Menu(chat_id, message_id);
                    break;
                case "Account":
                    Command.data[chat_id].State = State.Account.ToString();
                    Reply.Account(chat_id, message_id.ToString()); //?? string
                    break;
                case "AccountChange":
                    Command.data[chat_id].State = State.AccountChange.ToString();
                    Reply.AccountChange(chat_id, message_id.ToString()); //?? string
                    break;
                case "AccountExport":
                    Command.data[chat_id].State = State.AccountExport.ToString();
                    Reply.AccountExport(chat_id, message_id.ToString()); //?? string
                    break;
                case "AccountSubscription":
                    Command.data[chat_id].State = State.AccountSubscription.ToString();
                    //??  Reply клава
                    break;
                case "LinkedAccounts":
                    Command.data[chat_id].State = State.LinkedAccounts.ToString();
                    Reply.LinkedAccounts(chat_id, message_id.ToString()); //?? string
                    break;
                case "AddAccount":
                    Command.data[chat_id].State = State.AddAccount.ToString();
                    Reply.AddAccount(chat_id, message_id.ToString()); //?? string
                    break;
                case "RemoveAccount":
                    Command.data[chat_id].State = State.RemoveAccount.ToString();
                    Reply.RemoveAccount(chat_id, message_id.ToString()); //?? string
                    break;
                case "Diary":
                    Command.data[chat_id].State = State.Diary.ToString();
                    Reply.Diary(chat_id, message_id.ToString()); //?? string
                    break;
                case "SearchDiary":
                    Command.data[chat_id].State = State.SearchDiary.ToString();
                    Reply.SearchDiary(chat_id, message_id.ToString()); //?? string
                    break;
                case "AddToDiary":
                    Command.data[chat_id].State = State.AddToDiary.ToString();
                    Reply.AddToDiary(chat_id, message_id.ToString()); //?? string
                    break;
                case "Stats":
                    Command.data[chat_id].State = State.Stats.ToString();
                    Reply.Stats(chat_id, message_id.ToString()); //?? string
                    break;
                default:
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
