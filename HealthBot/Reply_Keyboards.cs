using LinqToDB;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.scripts
{
    static class Reply
    {
        public static async Task Menu(long chat_id, int message_id, string addition = "")
        {
            var user = Command.data[chat_id];
            var name = user.Name != null && user.Name != "" ? user.Name : user.Alias;

            string message = $"Welcome {name}!\n" +
                              "What do we do next?" +
                             $"{addition}";

            InlineKeyboardMarkup keyboard = new[] 
            {
                InlineKeyboardButton.WithCallbackData(text: "Diary"      , callbackData: "To_Diary"),
                InlineKeyboardButton.WithCallbackData(text: "Stats"      , callbackData: "To_Stats"),
                InlineKeyboardButton.WithCallbackData(text: "Accoun data", callbackData: "To_Account"),
            };

            await Command.Send(chat_id, message, keyboard, message_id);
        }
        public static async Task Account(long chat_id, string addition = "")
        {
            var user = Command.data[chat_id];

            string message = $"Account of {user.Alias}\n" +
                             $"Age: {user.Age?.ToString() ?? "not set"}\n" +
                             $"Weight: {user.Weight?.ToString() ?? "not set" }\n" +
                             $"Sex: {user.Sex?.ToString() ?? "not set"}\n" +
                             $"Subscription {(user.SubscriptionEnd == null ? Convert.ToDateTime(user.SubscriptionEnd - DateTime.Now).ToString("U") : "not started")}\n" +
                             $"Linked accounts: {user.DoctorIds?.Count().ToString() ?? "no linked accounts yet" }" +
                             $"{addition}";

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

            await Command.Send(chat_id, message, keyboard);
        }
        public static async Task LinkedAccounts(long chat_id, string addition = "")
        {
            var users_linked = Command.data[chat_id].DoctorIds;

            var message = $"Accounts that have access to your data:\n";

            foreach(var uuid in users_linked)
            {
                message += "@" + Command.db.Users.SingleOrDefault(u => u.Uuid.ToString() == uuid.ToString()).Alias.ToString() + "\n";
            }

            message += $"{addition}";

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

            await Command.Send(chat_id, message, keyboard);
        }
        public static async Task AccountChange(long chat_id, string addition = "")
        {
            var user = Command.data[chat_id];

            string message = $"What info you want to change?" +
                             $"Age: {user.Age?.ToString() ?? "not set"}\n" +
                             $"Weight: {user.Weight?.ToString() ?? "not set"}\n" +
                             $"Sex: {user.Sex?.ToString() ?? "not set"}\n" +
                             $"Subscription {(user.SubscriptionEnd == null ? Convert.ToDateTime(user.SubscriptionEnd - DateTime.Now).ToString("U") : "not started")}\n" +
                             $"Linked accounts: {user.DoctorIds?.Count().ToString() ?? "no linked accounts yet"}" +
                             $"{addition}";

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Age"   , callbackData: "Account_Change_Age"),
                    InlineKeyboardButton.WithCallbackData(text: "Weight", callbackData: "Account_Change_Weight"), // мляяяя
                    InlineKeyboardButton.WithCallbackData(text: "Sex"   , callbackData: "Account_Change_Sex")
                }
            };

            await Command.Send(chat_id, message, keyboard);
        }
        public static async Task AccountExport(long chat_id, string addition = "")
        {
            var message = $"You want all your user data exported?\n" +
                          $"{addition}";

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

            await Command.Send(chat_id, message, keyboard);
        }
        public static async Task AddAccount(long chat_id, string addition = "")
        {
            var message = "Type username of account you want to add.\n" +
                         $"{addition}";

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Cancel", callbackData: "To_LinkedAccounts")
                }
            };

            await Command.Send(chat_id, message, keyboard);
        }
        public static async Task RemoveAccount(long chat_id, string addition = "")
        {
            var message = "Type username of account you want to remove.\n" +
                         $"{addition}";

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Cancel", callbackData: "To_LinkedAccounts")
                }
            };

            await Command.Send(chat_id, message, keyboard);
        }
        public static async Task AccountSubsctuption(long chat_id, string addition = "")
        {
            var user = Command.data[chat_id];

            var message = $"For how long you desire to {(user.SubscriptionStart != null ? "prolong your" : "purchase")} subscription?" +
                          $"{addition}";

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

            await Command.Send(chat_id, message, keyboard);
        }
        public static async Task Stats(long chat_id, string addition = "")
        {
            string message = "What statistic info you want to see?" +
                             $"{addition}";

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]{
                 InlineKeyboardButton.WithCallbackData(text: "Menu"             , callbackData: "To_Menu")
                },
                new[]
                {
                  InlineKeyboardButton.WithCallbackData(text: "Calories by date" , callbackData: "Stats_CaloriesByDate"),
                  InlineKeyboardButton.WithCallbackData(text: "Liquid by date"   , callbackData: "Stats_LiquidByDate")
                }
            };

            await Command.Send(chat_id, message, keyboard);
        }
        public static async Task Diary(long chat_id, string addition = "")
        {
            string message = $"Welcome to your diary, what do you want to do?" +
                             $"{addition}";

            InlineKeyboardMarkup keyboard = new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Menu"         , callbackData: "To_Menu"),
                InlineKeyboardButton.WithCallbackData(text: "New entry"    , callbackData: "Diary_New"),
                InlineKeyboardButton.WithCallbackData(text: "Search entrys", callbackData: "Diary_Search")
            };

            await Command.Send(chat_id, message, keyboard);
        }
        public static async Task SearchDiary(long chat_id, string addition = "")
        {
            string message = $"Welcome to your diary, what do you want to do?" +
                 $"{addition}";

            InlineKeyboardMarkup keyboard = new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Cancel"        , callbackData: "To_Diary"),
                InlineKeyboardButton.WithCallbackData(text: "Search by type", callbackData: "Diary_Search_By_Type"), // мляяяя
                InlineKeyboardButton.WithCallbackData(text: "Search by name", callbackData: "Diary_Search_By_Name"),
                InlineKeyboardButton.WithCallbackData(text: "Search by tags", callbackData: "Diary_Search_By_Tags")
            };

            await Command.Send(chat_id, message, keyboard);
        }
        public static async Task AddToDiary(long chat_id, string addition = "")
        {
            string message = $"What type of entry you want to make?" +
                             $"{addition}";

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

            await Command.Send(chat_id, message, keyboard);
        }
    }
}