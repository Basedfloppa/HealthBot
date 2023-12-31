﻿using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using User = HealthBot.User;
using HealthBot;

namespace Bot.scripts
{
    static class Reply
    {
        public static (string, InlineKeyboardMarkup) Menu(long chat_id, string addition = "")
        {
            
            User user = Command.data[chat_id];
            string name = user.Name != null && user.Name != "" ? user.Name : user.Alias;

            StringBuilder message = new StringBuilder();
            message.Append($"Welcome {name}!\n");
            message.Append("What do we do next?");
            message.Append($"{addition}");

            InlineKeyboardMarkup keyboard = new[] 
            {
                InlineKeyboardButton.WithCallbackData(text: "Diary"      , callbackData: "To_Diary"),
                InlineKeyboardButton.WithCallbackData(text: "Stats"      , callbackData: "To_Stats"),
                InlineKeyboardButton.WithCallbackData(text: "Accoun data", callbackData: "To_Account"),
            };

            return (message.ToString(), keyboard);
            
        }
        public static (string, InlineKeyboardMarkup) Account(long chat_id, string addition = "")
        {
            
            User user = Command.data[chat_id];
            Biometry biometry = Command.db.Biometries
                                                    .Where(entry => entry.Author == user.Uuid)
                                                    .OrderByDescending(entry => entry.CreatedAt)
                                                    .FirstOrDefault();
            int linked_accounts = user.Observers.Count();

            StringBuilder message = new StringBuilder();
            message.Append($"Account of {user.Alias}\n");
            message.Append($"Age: {user.Age?.ToString() ?? "not set"}\n");
            message.Append($"Weight: {biometry?.Weight?.ToString() ?? "not set"}\n");
            message.Append($"Height: {biometry?.Height?.ToString() ?? "not set"}\n");
            message.Append($"Sex: {user.Sex?.ToString() ?? "not set"}\n");
            message.Append($"Subscription {(user.SubscriptionEnd == null ? Convert.ToDateTime(user.SubscriptionEnd - DateTime.Now).ToString("U") : "not started")}\n");
            message.Append($"Linked accounts: {(linked_accounts > 0 ? linked_accounts : "no linked accounts yet")}");
            message.Append($"{addition}");

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
        public static (string, InlineKeyboardMarkup) LinkedAccounts(long chat_id, string addition = "")
        {
            User user = Command.data[chat_id];
            var observers = user.Observers;

            StringBuilder message = new StringBuilder();
            message.Append("Accounts that have access to your data:\n");

            foreach(var observer in observers)
            {
                message.Append($"@{observer}\n");
            }

            message.Append($"{addition}");

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
        public static (string, InlineKeyboardMarkup) AccountChange(long chat_id, string addition = "")
        {
            User user = Command.data[chat_id];
            Biometry biometry = Command.db.Biometries.LastOrDefault(entry => entry.Author == user.Uuid);
            int linked_accounts = user.Observers.Count();

            StringBuilder message = new StringBuilder();
            message.Append($"Account of {user.Alias}\n");
            message.Append($"Age: {user.Age?.ToString() ?? "not set"}\n");
            message.Append($"Weight: {biometry?.Weight?.ToString() ?? "not set"}\n");
            message.Append($"Height: {biometry?.Height?.ToString() ?? "not set"}\n");
            message.Append($"Sex: {user.Sex?.ToString() ?? "not set"}\n");
            message.Append($"Subscription {(user.SubscriptionEnd == null ? Convert.ToDateTime(user.SubscriptionEnd - DateTime.Now).ToString("U") : "not started")}\n");
            message.Append($"Linked accounts: {(linked_accounts > 0 ? linked_accounts : "no linked accounts yet")}");
            message.Append($"{addition}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Age"   , callbackData: "Account_Change_Age"),
                    InlineKeyboardButton.WithCallbackData(text: "Weight", callbackData: "Account_Change_Weight"),
                    InlineKeyboardButton.WithCallbackData(text: "Sex"   , callbackData: "Account_Chainge_Sex")
                }
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) AccountExport(string addition = "")
        {
            StringBuilder message = new StringBuilder();
            message.Append("You want all your user data exported?\n");
            message.Append($"{addition}");

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
        public static (string, InlineKeyboardMarkup) AddAccount(string addition = "")
        {
            StringBuilder message = new StringBuilder();
            message.Append("Type username of account you want to add.\n");
            message.Append($"{addition}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Cancel", callbackData: "To_LinkedAccounts")
                }
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) RemoveAccount(string addition = "")  
        {
            StringBuilder message = new StringBuilder();
            message.Append("Type username of account you want to remove.\n");
            message.Append($"{addition}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Cancel", callbackData: "To_LinkedAccounts")
                }
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) AccountSubsctuption(long chat_id, string addition = "")
        {
            User user = Command.data[chat_id];

            StringBuilder message = new StringBuilder();
            message.Append($"For how long you desire to {(user.SubscriptionStart != null ? "prolong your" : "purchase")} subscription?");
            message.Append($"{addition}");

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
        public static (string, InlineKeyboardMarkup) Stats(string addition = "")
        {
            StringBuilder message = new StringBuilder();
            message.Append("What statistic info you want to see?");
            message.Append($"{addition}");

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

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) Diary(string addition = "")
        {
            StringBuilder message = new StringBuilder();
            message.Append($"Welcome to your diary, what do you want to do?");
            message.Append($"{addition}");

            InlineKeyboardMarkup keyboard = new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Menu"         , callbackData: "To_Menu"),
                InlineKeyboardButton.WithCallbackData(text: "New entry"    , callbackData: "Diary_New"),
                InlineKeyboardButton.WithCallbackData(text: "Search entrys", callbackData: "Diary_Search")
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) SearchDiary(string addition = "")
        {
            StringBuilder message = new StringBuilder();
            message.Append("Welcome to your diary, what do you want to do?");
            message.Append($"{addition}");

            InlineKeyboardMarkup keyboard = new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Cancel"        , callbackData: "To_Diary"),
                InlineKeyboardButton.WithCallbackData(text: "Search by type", callbackData: "Diary_Search_All_Type"),
                InlineKeyboardButton.WithCallbackData(text: "Search by name", callbackData: "Diary_Search_All_Name"),
                InlineKeyboardButton.WithCallbackData(text: "Search by tags", callbackData: "Diary_Search_All_Tags")
            };

            return (message.ToString(), keyboard);
        }
        public static (string, InlineKeyboardMarkup) AddToDiary(string addition = "")
        {
            StringBuilder message = new StringBuilder();
            message.Append("What type of entry you want to make?");
            message.Append($"{addition}");

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