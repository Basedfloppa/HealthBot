using Bot.scripts;
using Bot.code;
using Telegram.Bot.Types.ReplyMarkups;
using System.Data.Common;

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
                    tuple = Reply.Menu(user);

                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "Account":
                    tuple = Reply.Account(user);

                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "AccountChange":
                    tuple = Reply.AccountChange(user);

                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "AccountExport":
                    tuple = Reply.AccountExport();

                    user.LastAction = "AccountExport";
                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "AccountSubscription":
                    break;
                case "LinkedAccounts":
                    tuple = Reply.LinkedAccounts(user);

                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "Diary":
                    tuple = Reply.Diary();

                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "Stats":
                    tuple = Reply.Stats();

                    await Command.Send(user.ChatId, tuple, user.messageid);
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
                    tuple = Reply.LinkedAccounts(user,"Input handle of user that you want to remove");
                    user.LastAction = "RemoveAccount";

                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "AddAccount":
                    tuple = Reply.LinkedAccounts(user,"Input handle of user that you want to add");
                    user.LastAction = "AddAccount";

                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
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

                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "Weight":
                    tuple = Reply.AccountChange(user, "Input your current weight.");

                    user.LastAction = "AccountChangeWeight";

                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "Sex":
                    tuple = Reply.AccountChange(user, "Input your sex.");

                    user.LastAction = "AccountChangeSex";

                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "Height":
                    tuple = Reply.AccountChange(user, "input your height.");

                    user.LastAction = "AccountChangeHeight";

                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
            }
        }
        public static async Task Stats_State_Handler(User user, string callback_data)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch (callback_data.Split('_')[1])
            {
                case "CaloriesByDate":
                    tuple = Reply.Stats("Input two dates in format dd.mm.yy-dd.mm.yy where first date is less than second.");

                    user.LastAction = "CaloriesByDate";

                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "LiquidByDate":
                    tuple = Reply.Stats("Input two dates in format dd.mm.yy-dd.mm.yy where first date is less than second.");

                    user.LastAction = "LiquidByDate";

                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
            }
        }
        public static async Task Diary_State_Handler(User user, string callback_data)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch(callback_data.Split('_')[1])
            {
                case "New":
                    tuple = Reply.DiaryNew();

                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "Search":
                    break;
                case "Add":
                    await Diary_Add_State_Handler(user, callback_data);
                    break;
            }
        }
        public static async Task Diary_Add_State_Handler(User user, string callback_data)
        {
            (string, InlineKeyboardMarkup) tuple;

            switch(callback_data.Split('_')[2])
            {
                case "BloodPressure":
                    tuple = Reply.DiaryNew("Input your current blood pressure in format top_value/bottom_balue");
                    user.LastAction = "BloodPressure";

                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "BloodSaturation":
                    tuple = Reply.DiaryNew("Input your current blood oxygen saturation in numeric format");
                    user.LastAction = "BloodSaturation";

                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "HeartRate":
                    tuple = Reply.DiaryNew("Input your current heart rate in numeric format");
                    user.LastAction = "HeartRate";

                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
                case "IntakeItem":
                    tuple = Reply.IntakeItem("Input your current heart rate in numeric format");
                    user.LastAction = "HeartRate";

                    await Command.Update(user);
                    await Command.Send(user.ChatId, tuple, user.messageid);
                    break;
            }
        }
    }
}
