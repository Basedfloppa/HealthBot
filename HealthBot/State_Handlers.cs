using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using Bot.scripts;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.ReplyMarkups;

namespace HealthBot.handlers
{
    internal class State_Handlers
    {
        public static async Task To_State_Handler(User user, string callback_data)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch (callback_data.Split('_')[1])
            {
                case "Menu":
                    var db = new HealthBotContext();
                    if (db.Admins.Where( a => a.User == user.ChatId).Count() > 0)  tuple = Reply.MenuAdmin(user);
                    else tuple = Reply.Menu(user);

                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Account":
                    tuple = Reply.Account(user);

                    user.LastAction = "";

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "AccountChange":
                    tuple = Reply.AccountChange(user);

                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "AccountExport":
                    tuple = Reply.AccountExport();

                    user.LastAction = "AccountExport";
                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "AccountSubscription":
                    break;
                case "LinkedAccounts":
                    tuple = Reply.LinkedAccounts(user);

                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Diary":
                    tuple = Reply.Diary();

                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Stats":
                    tuple = Reply.Stats();

                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Analytics":
                    tuple = Reply.Analytics();

                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Admin":
                    tuple = Reply.Admin(user);

                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
            }
        }
        public static async Task Account_State_Handler(User user, string callback_data)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch (callback_data.Split('_')[1])
            {
                case "Change":
                    await Account_Change_State_Handler(user, callback_data);
                    break;
                case "RemoveAccount":
                    tuple = Reply.LinkedAccounts(
                        user,
                        "Input handle of user that you want to remove"
                    );
                    user.LastAction = "RemoveAccount";

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "AddAccount":
                    tuple = Reply.LinkedAccounts(user, "Input handle of user that you want to add");
                    user.LastAction = "AddAccount";

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
            }
        }
        public static async Task Account_Change_State_Handler(User user, string callback_data)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch (callback_data.Split('_')[2])
            {
                case "Age":
                    tuple = Reply.AccountChange(user, "Input your age.");

                    user.LastAction = "AccountChangeAge";

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Weight":
                    tuple = Reply.AccountChange(user, "Input your current weight.");

                    user.LastAction = "AccountChangeWeight";

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Sex":
                    tuple = Reply.AccountChange(user, "Input your sex.");

                    user.LastAction = "AccountChangeSex";

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Height":
                    tuple = Reply.AccountChange(user, "input your height.");

                    user.LastAction = "AccountChangeHeight";

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
            }
        }
        public static async Task Stats_State_Handler(User user, string callback_data)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch (callback_data.Split('_')[1])
            {
                case "CaloriesByDate":
                    tuple = Reply.Stats(
                        "Input two dates in format dd.mm.yy-dd.mm.yy where first date is less than second."
                    );

                    user.LastAction = "CaloriesByDate";

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "LiquidByDate":
                    tuple = Reply.Stats(
                        "Input two dates in format dd.mm.yy-dd.mm.yy where first date is less than second."
                    );

                    user.LastAction = "LiquidByDate";

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
            }
        }
        public static async Task Diary_State_Handler(User user, string callback_data)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch (callback_data.Split('_')[1])
            {
                case "New":
                    tuple = Reply.DiaryNew();

                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Search":
                    if (callback_data.Split('_').Count() == 3)
                    {
                        tuple = Reply.DiarySearch(
                            user.ChatId,
                            page: Convert.ToInt32(callback_data.Split('_')[2])
                        );
                    }
                    else
                    {
                        tuple = Reply.DiarySearch(user.ChatId);
                    }

                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Add":
                    await Diary_Add_State_Handler(user, callback_data);
                    break;
                case "Form":
                    await Diary_Form_State_Handler(user, callback_data);
                    break;
            }
        }
        public static async Task Diary_Add_State_Handler(User user, string callback_data)
        {
            HealthBotContext db = new HealthBotContext();
            (string, InlineKeyboardMarkup) tuple;
            Diaryentry entry;
            string entry_uuid = "";

            switch (callback_data.Split('_')[2])
            {
                case "BloodPressure":
                    user.LastAction = "BloodPressure";

                    if (callback_data.Split('_').Count() > 3)
                    {
                        entry_uuid = callback_data.Split('_')[3];
                    }
                    if (entry_uuid == "")
                    {
                        entry = new Diaryentry() { Author = user.ChatId, Type = "BloodPressure" };
                        db.DiaryEntrys.Add(entry);
                        db.SaveChanges();
                        entry_uuid = entry.Uuid.ToString();
                    }

                    tuple = Reply.DiaryEntryForm(entry_uuid: Guid.Parse(entry_uuid));
                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "BloodSaturation":
                    user.LastAction = "BloodSaturation";

                    if (callback_data.Split('_').Count() > 3)
                    {
                        entry_uuid = callback_data.Split('_')[3];
                    }
                    if (entry_uuid == "")
                    {
                        entry = new Diaryentry() { Author = user.ChatId, Type = "BloodSaturation" };
                        db.DiaryEntrys.Add(entry);
                        db.SaveChanges();
                        entry_uuid = entry.Uuid.ToString();
                    }

                    tuple = Reply.DiaryEntryForm(entry_uuid: Guid.Parse(entry_uuid));
                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "HeartRate":
                    user.LastAction = "HeartRate";

                    if (callback_data.Split('_').Count() > 3)
                    {
                        entry_uuid = callback_data.Split('_')[3];
                    }
                    if (entry_uuid == "")
                    {
                        entry = new Diaryentry() { Author = user.ChatId, Type = "HeartRate" };
                        db.DiaryEntrys.Add(entry);
                        db.SaveChanges();
                        entry_uuid = entry.Uuid.ToString();
                    }

                    tuple = Reply.DiaryEntryForm(entry_uuid: Guid.Parse(entry_uuid));
                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "IntakeItem":
                    user.LastAction = "IntakeItem";

                    if (callback_data.Split('_').Count() > 3)
                    {
                        entry_uuid = callback_data.Split('_')[3];
                    }
                    if (entry_uuid == "")
                    {
                        entry = new Diaryentry() { Author = user.ChatId, Type = "IntakeItem" };
                        db.DiaryEntrys.Add(entry);
                        db.SaveChanges();
                        entry_uuid = entry.Uuid.ToString();
                    }

                    tuple = Reply.DiaryEntryForm(entry_uuid: Guid.Parse(entry_uuid));
                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
            }
        }
        public static async Task Diary_Form_State_Handler(User user, string callback_data)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch (callback_data.Split('_')[2])
            {
                case "Cancel":
                    tuple = Reply.Diary();
                    user.LastAction = "";

                    var db = new HealthBotContext();
                    var entry = db.DiaryEntrys.Find(Guid.Parse(callback_data.Split('_')[3]));
                    db.Remove(entry);
                    db.SaveChanges();
                    db.Dispose();

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Name":
                    user.LastAction = $"DiaryFormName_{callback_data.Split('_')[3]}";
                    tuple = Reply.DiaryEntryForm(
                        addition_text: "Enter name of this entry",
                        entry_uuid: Guid.Parse(callback_data.Split('_')[3])
                    );

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Tags":
                    user.LastAction = $"DiaryFormTags_{callback_data.Split('_')[3]}";
                    tuple = Reply.DiaryEntryForm(
                        addition_text: "Enter tags of this entry in format of tag1,tag2,etc",
                        entry_uuid: Guid.Parse(callback_data.Split('_')[3])
                    );

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Date":
                    user.LastAction = $"DiaryFormDate_{callback_data.Split('_')[3]}";
                    tuple = Reply.DiaryEntryForm(
                        addition_text: "Enter date of this entry in format dd.mm.yyyy",
                        entry_uuid: Guid.Parse(callback_data.Split('_')[3])
                    );

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Pressure":
                    user.LastAction = $"DiaryFormPressure_{callback_data.Split('_')[3]}";
                    tuple = Reply.DiaryEntryForm(
                        addition_text: "Enter blood pressure for this entry",
                        entry_uuid: Guid.Parse(callback_data.Split('_')[3])
                    );

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Saturation":
                    user.LastAction = $"DiaryFormSarutation_{callback_data.Split('_')[3]}";
                    tuple = Reply.DiaryEntryForm(
                        addition_text: "Enter blood oxygen saturation for this entry",
                        entry_uuid: Guid.Parse(callback_data.Split('_')[3])
                    );

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Rate":
                    user.LastAction = $"DiaryFormRate_{callback_data.Split('_')[3]}";
                    tuple = Reply.DiaryEntryForm(
                        addition_text: "Enter heart rate for this entry",
                        entry_uuid: Guid.Parse(callback_data.Split('_')[3])
                    );

                    await Command.Database.Update(user);
                    await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                    break;
                case "Intake":
                    switch (callback_data.Split('_')[3])
                    {
                        case "State":
                            user.LastAction = $"DiaryFormIntakeState_{callback_data.Split('_')[4]}";
                            tuple = Reply.DiaryEntryForm(
                                addition_text: "Enter state of matter for this entry, liquid or solid",
                                entry_uuid: Guid.Parse(callback_data.Split('_')[4])
                            );

                            await Command.Database.Update(user);
                            await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                            break;
                        case "Weight":
                            user.LastAction =
                                $"DiaryFormIntakeWeight_{callback_data.Split('_')[4]}";
                            tuple = Reply.DiaryEntryForm(
                                addition_text: "Enter weight of matter for this entry",
                                entry_uuid: Guid.Parse(callback_data.Split('_')[4])
                            );

                            await Command.Database.Update(user);
                            await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                            break;
                        case "Calory":
                            user.LastAction =
                                $"DiaryFormIntakeCalory_{callback_data.Split('_')[4]}";
                            tuple = Reply.DiaryEntryForm(
                                addition_text: "Enter amount of calories for this entry",
                                entry_uuid: Guid.Parse(callback_data.Split('_')[4])
                            );

                            await Command.Database.Update(user);
                            await Command.Message.Send(user.ChatId, tuple, user.MessageId);
                            break;
                    }
                    break;
            }
        }
        public static async Task Admin_State_Handler(User user, string callback_data)
        {
            switch(callback_data.Split('_')[1])
            {
                case "NukeDb":
                    HealthBotContext db = new();

                    var bio = db.Biometries.Where(b => true);
                    foreach(var b in bio)
                    {
                        db.Remove(b);
                    }

                    var dio = db.DiaryEntrys.Where(e => true);
                    foreach(var e in dio)
                    {
                        db.Remove(e);
                    }

                    var users = db.Users.Where(u => true);
                    foreach(var u in users)
                    {
                        db.Remove(u);
                    }

                    var exp = db.ExportData.Where(d => true);
                    foreach(var d in exp)
                    {
                        db.Remove(d);
                    }
                    
                    var adm = db.Admins.Where(a => true);
                    foreach(var a in adm)
                    {
                        db.Remove(a);
                    }

                    db.Dispose();
                    break;
                case "Shutdown":
                    Environment.Exit(0);
                    break;
                case "SeedData":
                    await Command.Database.Seed();
                    break;                
            }
        }
    }
}
