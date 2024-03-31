using System.Data;
using System.Text.Json;
using HealthBot;
using User = HealthBot.User;

namespace Sql_Queries
{
    public static class Query
    {
        internal static IEnumerable<Diaryentry> average_calories_by_date(DateTime date_max, DateTime date_min, User user)
        {
            var db = new HealthBotContext();
            var entrys = db.DiaryEntrys.Where(e =>
                e.Author == user.ChatId
                && (
                    DateTime.Compare(e.UpdatedAt, date_min) == 0
                    || DateTime.Compare(e.UpdatedAt, date_max) == 1
                )
                && e.State == "solid"
            );

            return entrys;


            //  var calories_summ = 0.0;
            /*
                foreach (var entry in entrys)
                    calories_summ += entry.CaloryAmount.GetValueOrDefault(0);

                return calories_summ / entrys.Count();
            */
        }

        internal static IEnumerable<Diaryentry> average_water_by_date(DateTime date_max, DateTime date_min, User user)
        {
            var db = new HealthBotContext();
            var entrys = db.DiaryEntrys.Where(e =>
                e.Author == user.ChatId
                && (
                    DateTime.Compare(e.UpdatedAt, date_min) == 0
                    || DateTime.Compare(e.UpdatedAt, date_max) == 1
                )
                && e.State == "liquid"
            );

            return entrys;
            // var calories_summ = 0.0;
            /*
            foreach (var entry in entrys)
                calories_summ += entry.CaloryAmount.GetValueOrDefault(0);

            return calories_summ / entrys.Count(); */
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
