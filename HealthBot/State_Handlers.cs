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
                case "Change":
                    Command.data[chat_id].State = State.Change.ToString();
                    Reply.AccountChange(chat_id, message_id.ToString()); //?? string
                    break;
                case "CaloriesByDate":
                    Command.data[chat_id].State = State.CaloriesByDate.ToString();
                    Reply.Stats(chat_id, message_id.ToString()); //?? string
                    break;
                case "LiquidByDate":
                    Command.data[chat_id].State = State.LiquidByDate.ToString();
                    Reply.Stats(chat_id, message_id.ToString()); //?? string
                    break;
                case "New":
                    Command.data[chat_id].State = State.New.ToString();
                    Reply.Diary(chat_id, message_id.ToString()); //?? string
                    break;
                case "Search":
                    Command.data[chat_id].State = State.Search.ToString();
                    Reply.Diary(chat_id, message_id.ToString()); //?? string
                    break;
                default:
                    break;
            }
            return Task.CompletedTask;
            
        }
        
        public static Task Account_State_Handler(long chat_id, string callback_data, int message_id)
        {
            switch (callback_data.Split('_')[2])
            {
                case "Age":
                    Command.data[chat_id].State = State.Age.ToString();
                    Reply.AccountChange(chat_id, message_id.ToString());
                    break;
                case "Weight":
                    Command.data[chat_id].State = State.Weight.ToString();
                    Reply.AccountChange(chat_id, message_id.ToString()); //?? string
                    break;
                case "Sex":
                    Command.data[chat_id].State = State.Sex.ToString();
                    Reply.AccountChange(chat_id, message_id.ToString()); //?? string
                    break;
                default:
                    break;
            }
            return Task.CompletedTask; //я хз как после аккаует чейндж можно реализовать
        }

        public static Task Search_State_Handler(long chat_id, string callback_data, int message_id)
        {
            switch (callback_data.Split('_')[3])
            {
                case "Type":
                    Command.data[chat_id].State = State.Type.ToString();
                    Reply.Diary(chat_id, message_id.ToString());
                    break;
                case "Name":
                    Command.data[chat_id].State = State.Name.ToString();
                    Reply.Diary(chat_id, message_id.ToString()); //?? string
                    break;
                case "Tags":
                    Command.data[chat_id].State = State.Tags.ToString();
                    Reply.Diary(chat_id, message_id.ToString()); //?? string
                    break;
                default:
                    break;
            }
            return Task.CompletedTask; //я хз как после аккаует чейндж можно реализовать
        }

        public static Task AddToDiary_State_Handler(long chat_id, string callback_data, int message_id)
        {
            switch (callback_data.Split('_')[2])
            {
                case "Type":
                    Command.data[chat_id].State = State.Type.ToString();
                    Reply.Diary(chat_id, message_id.ToString());
                    break;
                case "Biometry":
                    Command.data[chat_id].State = State.Biometry.ToString();
                    Reply.Diary(chat_id, message_id.ToString()); //?? string
                    break;
                default:
                    break;
            }
            return Task.CompletedTask; //я хз как после аккаует чейндж можно реализовать
        }
    }
}
