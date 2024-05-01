using System.Text;
using HealthBot;
using Telegram.Bot.Types.ReplyMarkups;
using User = HealthBot.User;

namespace Bot.scripts
{
    static class Reply
    {
        public static (string, InlineKeyboardMarkup) Menu(User user, string addition_text = "")
        {
            string name = user.Name != null && user.Name != "" ? user.Name : user.Alias;

            StringBuilder message = new StringBuilder();
            message.AppendLine($"Welcome {name}!");
            message.AppendLine("What do we do next?");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "📓Diary📓", callbackData: "To_Diary"),
                InlineKeyboardButton.WithCallbackData(text: "📈Stats📈", callbackData: "To_Stats"),
                InlineKeyboardButton.WithCallbackData(text: "🧑Account🧑", callbackData: "To_Account"),
            };

            return (message.ToString(), keyboard);
        }

        public static (string, InlineKeyboardMarkup) MenuAdmin(User user, string addition_text = "")
        {
            string name = user.Name != null && user.Name != "" ? user.Name : user.Alias;

            StringBuilder message = new StringBuilder();
            message.AppendLine($"Welcome {name}!");
            message.AppendLine("What do we do next?");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "📓Diary📓", callbackData: "To_Diary"),
                InlineKeyboardButton.WithCallbackData(text: "📈Stats📈", callbackData: "To_Stats"),
                InlineKeyboardButton.WithCallbackData(text: "🧑Account🧑", callbackData: "To_Account"),
                InlineKeyboardButton.WithCallbackData(text: "🤖Admin🤖", callbackData: "To_Admin")
            };

            return (message.ToString(), keyboard);
        }

        public static (string, InlineKeyboardMarkup) Admin(User user, string addition_text = "")
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine($"Welcome sillyman, its time to admin admin");

            InlineKeyboardMarkup keyboard = new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Shutdown", callbackData: "Admin_Shutdown"),
                InlineKeyboardButton.WithCallbackData(text: "SeedData", callbackData: "Admin_SeedData"),
                InlineKeyboardButton.WithCallbackData(text: "NukeDB", callbackData: "Admin_NukeDb"),
                InlineKeyboardButton.WithCallbackData(text: "🧾Menu🧾", callbackData: "To_Menu")
            };

            return (message.ToString(), keyboard);
        }

        public static (string, InlineKeyboardMarkup) Account(User user, string addition_text = "")
        {
            var db = new HealthBotContext();
            var height = db
                .Biometries.Where(b => b.Author == user.ChatId)
                .Where(b => b.Height != null)
                ?.OrderBy(b => b.CreatedAt)
                .FirstOrDefault()
                ?.Height;
            var weight = db
                .Biometries.Where(b => b.Author == user.ChatId)
                .Where(b => b.Weight != null)
                ?.OrderBy(b => b.CreatedAt)
                .FirstOrDefault()
                ?.Weight;
            int linked_accounts = user.Observers.Count();
            db.Dispose();

            StringBuilder message = new StringBuilder();
            message.AppendLine($"Account of {user.Alias}");
            message.AppendLine($"Age: {user.Age?.ToString() ?? "not set"}");
            message.AppendLine($"Weight: {weight?.ToString() ?? "not set"}");
            message.AppendLine($"Height: {height?.ToString() ?? "not set"}");
            message.AppendLine($"Sex: {user.Sex?.ToString() ?? "not set"}");
            message.AppendLine($"Subscription {(user.SubscriptionEnd == null ? Convert.ToDateTime(user.SubscriptionEnd - DateTime.Now).ToString("U") : "not started")}");
            message.AppendLine($"Linked accounts: {(linked_accounts > 0 ? linked_accounts : "no linked accounts yet")}");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "🤝Manage linked accounts🤝", callbackData: "To_LinkedAccounts"),
                    InlineKeyboardButton.WithCallbackData( text: "💰Manage subscription💰", callbackData: "To_Subscriprion")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "📝Change info📝", callbackData: "To_AccountChange"),
                    InlineKeyboardButton.WithCallbackData(text: "🧾Menu🧾", callbackData: "To_Menu"),
                    InlineKeyboardButton.WithCallbackData( text: "📦Export data📦", callbackData: "To_AccountExport"
                    )
                }
            };

            return (message.ToString(), keyboard);
        }

        public static (string, InlineKeyboardMarkup) LinkedAccounts(User user, string addition_text = "")
        {
            var db = new HealthBotContext();
            var observers = db.Users.Find(user.ChatId)?.Observers;
            var observees = db.Users.Find(user.ChatId)?.Observees;
            StringBuilder message = new StringBuilder();

            message.AppendLine("Accounts that have access to your data:\n");

            foreach (var observer in observers)
            {
                message.AppendLine($"@{observer} can see your data");
            }

            foreach (var observee in observees)
            {
                message.AppendLine($"@{observee} you can see their data");
            }

            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "✔️Add account✔️", callbackData: "Account_AddAccount"),
                    InlineKeyboardButton.WithCallbackData(text: "❌Remove account❌", callbackData: "Account_RemoveAccount")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "🧑Accoun🧑", callbackData: "To_Account"),
                }
            };

            return (message.ToString(), keyboard);
        }

        public static (string, InlineKeyboardMarkup) AccountChange(User user, string addition_text = "")
        {
            var db = new HealthBotContext();
            var height = db
                .Biometries.Where(b => b.Author == user.ChatId)
                .Where(b => b.Height != null)
                ?.OrderBy(b => b.CreatedAt)
                .FirstOrDefault()
                ?.Height;
            var weight = db
                .Biometries.Where(b => b.Author == user.ChatId)
                .Where(b => b.Weight != null)
                ?.OrderBy(b => b.CreatedAt)
                .FirstOrDefault()
                ?.Weight;
            int linked_accounts = user.Observers.Count();

            StringBuilder message = new StringBuilder();
            message.AppendLine($"Account of {user.Alias}");
            message.AppendLine($"Age: {user.Age?.ToString() ?? "not set"}");
            message.AppendLine($"Weight: {weight?.ToString() ?? "not set"}");
            message.AppendLine($"Height: {height?.ToString() ?? "not set"}");
            message.AppendLine($"Sex: {user.Sex?.ToString() ?? "not set"}");
            message.AppendLine(
                $"Subscription {(user.SubscriptionEnd == null ? Convert.ToDateTime(user.SubscriptionEnd - DateTime.Now).ToString("U") : "not started")}"
            );
            message.AppendLine(
                $"Linked accounts: {(linked_accounts > 0 ? linked_accounts : "no linked accounts yet")}"
            );
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "👶Age🧓", callbackData: "Account_Change_Age"),
                    InlineKeyboardButton.WithCallbackData( text: "⚖️Weight⚖️", callbackData: "Account_Change_Weight"),
                    InlineKeyboardButton.WithCallbackData(text: "♂️Sex♀️", callbackData: "Account_Change_Sex"),
                    InlineKeyboardButton.WithCallbackData(text: "📏Height📏", callbackData: "Account_Change_Height")
                },
                new[]
                {
                   InlineKeyboardButton.WithCallbackData(text: "🧑Accoun🧑", callbackData: "To_Account"),
                }
            };

            return (message.ToString(), keyboard);
        }

        public static (string, InlineKeyboardMarkup) AccountExport(string addition_text = "")
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("You want all your user data exported?");
            message.AppendLine("Type 'yes' to proceed");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "🧑Accoun🧑", callbackData: "To_Account"),
                }
            };

            return (message.ToString(), keyboard);
        }

        public static (string, InlineKeyboardMarkup) AccountSubsctuption(User user, string addition_text = "")
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(
                $"For how long you desire to {(user.SubscriptionStart != null ? "prolong your" : "purchase")} subscription?"
            );
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "🧑Accoun🧑", callbackData: "To_Account"),
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
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "🧾Menu🧾", callbackData: "To_Menu")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Calories by date", callbackData: $"Stats_CaloriesByDate{addition_tags}"),
                    InlineKeyboardButton.WithCallbackData(text: "Liquid by date", callbackData: $"Stats_LiquidByDate{addition_tags}")
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
                InlineKeyboardButton.WithCallbackData(text: "🧾Menu🧾", callbackData: "To_Menu"),
                InlineKeyboardButton.WithCallbackData(text: "➕New entry➕", callbackData: "Diary_New"),
                InlineKeyboardButton.WithCallbackData(text: "Search entrys", callbackData: "Diary_Search")
            };

            return (message.ToString(), keyboard);
        }

        public static (string, InlineKeyboardMarkup) DiaryNew(string addition_text = "")
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine($"What type of entry you want to create?");
            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Blood pressure", callbackData: "Diary_Add_BloodPressure"),
                    InlineKeyboardButton.WithCallbackData(text: "Blood oxygen saturation", callbackData: "Diary_Add_BloodSaturation")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Heart rate", callbackData: "Diary_Add_HeartRate"),
                    InlineKeyboardButton.WithCallbackData(text: "Intake item",callbackData: "Diary_Add_Intake")
                },
                new[]
                {
                InlineKeyboardButton.WithCallbackData(text: "📓Diary📓", callbackData: "To_Diary"),
                }
            };

            return (message.ToString(), keyboard);
        }

        public static (string, InlineKeyboardMarkup) DiaryEntryForm(string addition_text = "", Guid entry_uuid = new Guid())
        {
            StringBuilder message = new StringBuilder();
            InlineKeyboardMarkup keyboard;

            HealthBotContext db = new HealthBotContext();
            Diaryentry entry = db.DiaryEntrys.Find(entry_uuid);

            if (entry.Name != null)
                message.AppendLine($"Name: {entry.Name}");
            else
                message.AppendLine("Name: not set");
            if (entry.Tags != null)
                message.AppendLine($"Tags: {entry.Tags.Replace(" ", " ,")}");
            else
                message.AppendLine("Tags: not set");

            switch (entry.Type)
            {
                case "BloodPressure":
                    message.AppendLine($"Blood pressure: {entry.BloodPreassure}");

                    keyboard = new InlineKeyboardMarkup(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Entry name",
                                    callbackData: $"Diary_Form_Name_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Entry tags",
                                    callbackData: $"Diary_Form_Tags_{entry_uuid}"
                                )
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Blood pressure value",
                                    callbackData: $"Diary_Form_Pressure_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Date",
                                    callbackData: $"Diary_Form_Date_{entry_uuid}"
                                )
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Cancel",
                                    callbackData: $"Diary_Form_Cancel_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Back",
                                    callbackData: $"Diary_New"
                                )
                            }
                        }
                    );
                    break;
                case "BloodSaturation":
                    message.AppendLine($"Blood oxygen saturation: {entry.BloodSaturation}");

                    keyboard = new InlineKeyboardMarkup(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Entry name",
                                    callbackData: $"Diary_Form_Name_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Entry tags",
                                    callbackData: $"Diary_Form_Tags_{entry_uuid}"
                                )
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Blood oxygen saturation value",
                                    callbackData: $"Diary_Form_Saturation_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Date",
                                    callbackData: $"Diary_Form_Date_{entry_uuid}"
                                )
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Cancel",
                                    callbackData: $"Diary_Form_Cancel_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Back",
                                    callbackData: $"Diary_New"
                                )
                            }
                        }
                    );
                    break;
                case "HeartRate":
                    message.AppendLine($"Heart rate: {entry.HeartRate}");

                    keyboard = new InlineKeyboardMarkup(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Entry name",
                                    callbackData: $"Diary_Form_Name_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Entry tags",
                                    callbackData: $"Diary_Form_Tags_{entry_uuid}"
                                )
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Heart rate value",
                                    callbackData: $"Diary_Form_Rate_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Date",
                                    callbackData: $"Diary_Form_Date_{entry_uuid}"
                                )
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Cancel",
                                    callbackData: $"Diary_Form_Cancel_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Back",
                                    callbackData: $"Diary_New"
                                )
                            }
                        }
                    );
                    break;
                case "Intake":
                    if (entry.Type == "solid")
                        message.AppendLine("Type: Solid");
                    if (entry.Type == "liquid")
                        message.AppendLine("Type: Liquid");

                    if (entry.Weight != null)
                        message.AppendLine($"Proguct weight: {entry.Weight} gramms");
                    if (entry.CaloryAmount != null)
                        message.AppendLine($"Product calory amount: {entry.CaloryAmount}");

                    keyboard = new InlineKeyboardMarkup(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Entry name",
                                    callbackData: $"Diary_Form_Name_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Entry tags",
                                    callbackData: $"Diary_Form_Tags_{entry_uuid}"
                                )
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Intake item state",
                                    callbackData: $"Diary_Form_Intake_State_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Date",
                                    callbackData: $"Diary_Form_Date_{entry_uuid}"
                                )
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Intake item weight",
                                    callbackData: $"Diary_Form_Intake_Weight_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Intake item calory amount",
                                    callbackData: $"Diary_Form_Intake_Calory_{entry_uuid}"
                                )
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Cancel",
                                    callbackData: $"Diary_Form_Cancel_{entry_uuid}"
                                ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Back",
                                    callbackData: $"Diary_New"
                                )
                            }
                        }
                    );
                    break;
                default:
                    keyboard = new InlineKeyboardMarkup(
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Cancel",
                                    callbackData: $"Diary_Form_Cancel_{entry_uuid}"
                                )
                            }
                        }
                    );
                    break;
            }

            message.AppendLine($"{addition_text}");

            return (message.ToString(), keyboard);
        }

        public static (string, InlineKeyboardMarkup) DiarySearch(long chat_id, string addition_text = "", int page = 0)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine($"Entrys:");

            var db = new HealthBotContext();
            var entrys = db.DiaryEntrys.Where(e => e.Author == chat_id).Skip(page * 10).Take(10);
            var pages = Math.Ceiling(db.DiaryEntrys.Where(e => e.Author == chat_id).Count() / 10.0);

            foreach (var entry in entrys)
            {
                if (entry.Name == null)
                    message.AppendLine($"Name: {entry.Name}");
                if (entry.Tags == null)
                    message.AppendLine($"Tags: {entry.Tags?.Replace(" ", " ,")}");

                switch (entry.Type)
                {
                    case "BloodPressure":
                        message.AppendLine($"Blood pressure: {entry.BloodPreassure}");
                        break;
                    case "BloodSaturation":
                        message.AppendLine($"Blood oxygen saturation: {entry.BloodSaturation}");
                        break;
                    case "HeartRate":
                        message.AppendLine($"Heart rate: {entry.HeartRate}");
                        break;
                    case "Intake":
                        if (entry.Type == "solid")
                            message.AppendLine("Type: Solid");
                        if (entry.Type == "liquid")
                            message.AppendLine("Type: Liquid");

                        if (entry.Weight != null)
                            message.AppendLine($"Proguct weight: {entry.Weight} gramms");
                        if (entry.CaloryAmount != null)
                            message.AppendLine($"Product calory amount: {entry.CaloryAmount}");
                        break;
                }

                message.AppendLine("");
            }

            message.AppendLine($"{addition_text}");

            InlineKeyboardMarkup keyboard = new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text: "first",
                        callbackData: "Diary_Search_0"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        text: "<",
                        callbackData: page > 0 ? $"Diary_Search_{page - 1}" : $"Diary_Search_{page}"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        text: $"{page}",
                        callbackData: $"Diary_Search_{page}"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        text: ">",
                        callbackData: page < pages
                            ? $"Diary_Search_{page + 1}"
                            : $"Diary_Search_{page}"
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        text: "last",
                        callbackData: $"Diary_Search_{pages}"
                    )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "back", callbackData: "To_Diary")
                }
            };

            return (message.ToString(), keyboard);
        }
    }
}
