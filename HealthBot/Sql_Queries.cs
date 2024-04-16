using System.ComponentModel;
using System.Data;
using System.Text.Json;
using HealthBot;
using User = HealthBot.User;

namespace Sql_Queries
{
    public static class Query
    {
        internal static (List<DateTime>, List<Double>) calories_by_date((DateTime max, DateTime min) date,  User user)
        {
            var db = new HealthBotContext();
            var entrys = db.DiaryEntrys.Where(e =>
                e.Author == user.ChatId
                && (
                    DateTime.Compare(e.UpdatedAt, date.min) == 0
                    || DateTime.Compare(e.UpdatedAt, date.max) == 1
                )
                && e.State == "solid"
            ).ToList();

            List<DateTime> dates = (from e in entrys select e.UpdatedAt).ToList();
            List<Double> values = (from e in entrys select Convert.ToDouble(e.CaloryAmount)).ToList();
            return (dates,values);
        }

        internal static (List<DateTime>, List<Double>) water_by_date((DateTime max, DateTime min) date, User user)
        {
            var db = new HealthBotContext();
            var entrys = db.DiaryEntrys.Where(e =>
                e.Author == user.ChatId
                && (
                    DateTime.Compare(e.UpdatedAt, date.min) == 0
                    || DateTime.Compare(e.UpdatedAt, date.max) == 1
                )
                && e.State == "liquid"
            ).ToList();

            List<DateTime> dates = (from e in entrys select e.UpdatedAt).ToList();
            List<Double> values = (from e in entrys select Convert.ToDouble(e.CaloryAmount)).ToList();
            return (dates,values);
        }

        internal static List<Diaryentry> items_by_name(string name, User user)
        {
            var db = new HealthBotContext();
            var entrys = db
                .DiaryEntrys.Where(e =>
                    e.Author == user.ChatId && e.Name.ToLower().Contains(name.ToLower())
                )
                .ToList();

            return entrys;
        }

        internal static List<Diaryentry> items_by_tag(string tag, User user)
        {
            var db = new HealthBotContext();
            var entrys = db
                .DiaryEntrys.Where(e =>
                    e.Author == user.ChatId && e.Tags.ToLower().Contains(tag.ToLower())
                )
                .ToList();

            return entrys;
        }

        internal static List<Diaryentry> items_by_argument(string argument, User user)
        {
            var db = new HealthBotContext();
            var entrys = db
                .DiaryEntrys.Where(e =>
                    e.Author == user.ChatId
                    && (
                        e.Tags.ToLower().Contains(argument.ToLower())
                        || e.Name.ToLower().Contains(argument.ToLower())
                    )
                )
                .ToList();

            return entrys;
        }

        public static string user_data_export(User user)
        {
            var db = new HealthBotContext();
            var diary_entrys = db.DiaryEntrys.Where(e => e.Author == user.ChatId);
            var biometry = db.Biometries.Where(b => b.Author == user.ChatId);

            return JsonSerializer.Serialize(user)
                + "\n\n"
                + JsonSerializer.Serialize(diary_entrys)
                + "\n\n"
                + JsonSerializer.Serialize(biometry);
        }
    }
}
