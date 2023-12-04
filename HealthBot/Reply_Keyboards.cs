using LinqToDB;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.scripts
{
    static class Reply
    {
        public static (string, InlineKeyboardMarkup) Menu(long chat_id, string addition_text = "")
        {
            DataModel.User user = Command.data[chat_id];
            string name = user.Name != null && user.Name != "" ? user.Name : user.Alias;

            StringBuilder message = new StringBuilder();
            message.AppendLine($"Welcome {name}!");
            message.AppendLine("What do we do next?");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[] 
            {
                InlineKeyboardButton.WithCallbackData(text: "Diary"      , callbackData: "To_Diary"),
                InlineKeyboardButton.WithCallbackData(text: "Stats"      , callbackData: "To_Stats"),
                InlineKeyboardButton.WithCallbackData(text: "Accoun data", callbackData: "To_Account"),
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) Account(long chat_id, string addition_text = "")
        {
            DataModel.User user = Command.data[chat_id];
            DataModel.Biometry biometry = Command.db.Biometries
                                                    .Where(entry => entry.Author == user.Uuid)
                                                    .OrderByDescending(entry => entry.CreatedAt)
                                                    .FirstOrDefault();
            int linked_accounts = Command.db.Obresvers.Count(entry => entry.Observee == user.Uuid);

            StringBuilder message = new StringBuilder();
            message.AppendLine($"Account of {user.Alias}");
            message.AppendLine($"Age: {user.Age?.ToString() ?? "not set"}");
            message.AppendLine($"Weight: {biometry?.Weight?.ToString() ?? "not set"}");
            message.AppendLine($"Height: {biometry?.Height?.ToString() ?? "not set"}");
            message.AppendLine($"Sex: {user.Sex?.ToString() ?? "not set"}");
            message.AppendLine($"Subscription {(user.SubscriptionEnd == null ? Convert.ToDateTime(user.SubscriptionEnd - DateTime.Now).ToString("U") : "not started")}");
            message.AppendLine($"Linked accounts: {(linked_accounts > 0 ? linked_accounts : "no linked accounts yet")}");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Manage linked accounts" , callbackData: "To_LinkedAccounts"),
                    InlineKeyboardButton.WithCallbackData(text: "Manage subscription"    , callbackData: "To_Subscriprion")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Change info"            , callbackData: "To_AccountChange"),
                    InlineKeyboardButton.WithCallbackData(text: "Menu"                   , callbackData: "To_Menu"),
                    InlineKeyboardButton.WithCallbackData(text: "Export data"            , callbackData: "To_AccountExport")
                }
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) LinkedAccounts(long chat_id, string addition_text = "")
        {
            DataModel.User user = Command.data[chat_id];
            var observers_id =  from entry
                                in Command.db.Obresvers
                                where entry.Observee == user.Uuid
                                select entry.Observer;
            var observers = from entry
                            in Command.db.Users
                            where observers_id.Contains(entry.Uuid)
                            select entry.Alias;

            StringBuilder message = new StringBuilder();
            message.AppendLine("Accounts that have access to your data:\n");

            foreach(var observer in observers)
            {
                message.AppendLine($"@{observer}");
            }

            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Add account"   , callbackData: "To_AddAccount"),
                    InlineKeyboardButton.WithCallbackData(text: "Remove account", callbackData: "To_RemoveAccount")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Menu"          , callbackData: "To_Menu"),
                    InlineKeyboardButton.WithCallbackData(text: "Account"       , callbackData: "To_Account")
                }
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) AccountChange(long chat_id, string addition_text = "")
        {
            DataModel.User user = Command.data[chat_id];
            DataModel.Biometry biometry = Command.db.Biometries.LastOrDefault(entry => entry.Author == user.Uuid);
            int linked_accounts = Command.db.Obresvers.Count(entry => entry.Observee == user.Uuid);

            StringBuilder message = new StringBuilder();
            message.AppendLine($"Account of {user.Alias}");
            message.AppendLine($"Age: {user.Age?.ToString() ?? "not set"}");
            message.AppendLine($"Weight: {biometry?.Weight?.ToString() ?? "not set"}");
            message.AppendLine($"Height: {biometry?.Height?.ToString() ?? "not set"}");
            message.AppendLine($"Sex: {user.Sex?.ToString() ?? "not set"}");
            message.AppendLine($"Subscription {(user.SubscriptionEnd == null ? Convert.ToDateTime(user.SubscriptionEnd - DateTime.Now).ToString("U") : "not started")}");
            message.AppendLine($"Linked accounts: {(linked_accounts > 0 ? linked_accounts : "no linked accounts yet")}");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Age"   , callbackData: "Account_Change_Age"),
                    InlineKeyboardButton.WithCallbackData(text: "Weight", callbackData: "Account_Change_Weight"),
                    InlineKeyboardButton.WithCallbackData(text: "Sex"   , callbackData: "Account_Change_Sex")
                }
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) AccountExport(string addition_text = "")
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("You want all your user data exported?");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Cancel", callbackData: "To_Account"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Yes"   , callbackData: "Export_All"),
                }
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) AddAccount(string addition_text = "")
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Type username of account you want to add.");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Cancel", callbackData: "To_LinkedAccounts")
                }
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) RemoveAccount(string addition_text = "")  
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Type username of account you want to remove.");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Cancel", callbackData: "To_LinkedAccounts")
                }
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) AccountSubsctuption(long chat_id, string addition_text = "")
        {
            DataModel.User user = Command.data[chat_id];

            StringBuilder message = new StringBuilder();
            message.AppendLine($"For how long you desire to {(user.SubscriptionStart != null ? "prolong your" : "purchase")} subscription?");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Cancel", callbackData: "To_Account")
                },
                new[]
                {
                    InlineKeyboardButton.WithPayment(text: "1 Month"),
                    InlineKeyboardButton.WithPayment(text: "3 Months")
                },
                new[]
                {
                    InlineKeyboardButton.WithPayment(text: "6 Months"),
                    InlineKeyboardButton.WithPayment(text: "1 Year")
                }
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) Stats(string addition_text = "", string addition_tags = "")
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("What statistic info you want to see?");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]{
                 InlineKeyboardButton.WithCallbackData(text: "Menu"             , callbackData: "To_Menu")
                },
                new[]
                {
                  InlineKeyboardButton.WithCallbackData(text: "Calories by date" , callbackData: $"Stats_CaloriesByDate{addition_tags}"),
                  InlineKeyboardButton.WithCallbackData(text: "Liquid by date"   , callbackData: $"Stats_LiquidByDate{addition_tags}")
                }
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) Diary(string addition_text = "")
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine($"Welcome to your diary, what do you want to do?");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Menu"         , callbackData: "To_Menu"),
                InlineKeyboardButton.WithCallbackData(text: "New entry"    , callbackData: "Diary_New"),
                InlineKeyboardButton.WithCallbackData(text: "Search entrys", callbackData: "Diary_Search")
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) SearchDiary(string addition_text = "")
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Welcome to your diary, what do you want to do?");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Cancel"        , callbackData: "To_Diary"),
                InlineKeyboardButton.WithCallbackData(text: "Search by type", callbackData: "Diary_Search_All_Type"),
                InlineKeyboardButton.WithCallbackData(text: "Search by name", callbackData: "Diary_Search_All_Name"),
                InlineKeyboardButton.WithCallbackData(text: "Search by tags", callbackData: "Diary_Search_All_Tags")
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) AddToDiary(string addition_text = "")
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("What type of entry you want to make?");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Cancel"           , callbackData: "To_Diary")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Consumption entry", callbackData: "Diary_Add_Intake"),
                    InlineKeyboardButton.WithCallbackData(text: "Biometry Entry"   , callbackData: "Diary_Add_Biometry"),

                }
            };

            return (message.ToString(), keyboard);
        }
    }
}