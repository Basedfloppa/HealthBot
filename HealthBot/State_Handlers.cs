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
                    break;
                case "AccountChange":
                    break;
                case "AccountExport":
                    break;
                case "AccountSubscription":
                    break;
                case "LinkedAccounts":
                    break;
                case "AddAccount":
                    break;
                case "RemoveAccount":
                    break;
                case "Diary":
                    break;
                case "SearchDiary":
                    break;
                case "AddToDiary":
                    break;
                case "Stats":
                    break;
                default: 
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
