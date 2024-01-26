using System.Data;
using System.Text.Json;
using HealthBot;
using User = HealthBot.User;

namespace Sql_Queries
{
    public static class Query
    {
        internal static double average_calories_by_date(DateTime date_max, DateTime date_min, User user)
        {
            var db = new HealthBotContext();
            var calories_summ = 0.0;
            var entrys = db.Diaryentrys.Where(e =>
                            e.Author == user.Uuid &&
                            (
                                DateTime.Compare(e.UpdatedAt, date_min) == 0 ||
                                DateTime.Compare(e.UpdatedAt, date_max) == 1
                            ) &&
                            e.State == "solid"
                         );

            foreach (var entry in entrys) calories_summ += entry.CaloryAmount.GetValueOrDefault(0);

            return calories_summ / entrys.Count();
        }
        internal static double average_water_by_date(DateTime date_max, DateTime date_min, User user)
        {
            var db = new HealthBotContext();
            var calories_summ = 0.0;
            var entrys = db.Diaryentrys.Where(e =>
                            e.Author == user.Uuid &&
                            (
                                DateTime.Compare(e.UpdatedAt, date_min) == 0 ||
                                DateTime.Compare(e.UpdatedAt, date_max) == 1
                            ) &&
                            e.State == "liquid"
                         );

            foreach (var entry in entrys) calories_summ += entry.CaloryAmount.GetValueOrDefault(0);

            return calories_summ / entrys.Count();
        }
        internal static List<Diaryentry> items_by_name(string name, User user)
        {
            var db = new HealthBotContext();
            var entrys = db.Diaryentrys.Where(e =>
                            e.Author == user.Uuid &&
                            e.Name.ToLower().Contains(name.ToLower())
                         ).ToList();

            return entrys;
        }
        internal static List<Diaryentry> items_by_tag(string tag, User user)
        {
            var db = new HealthBotContext();
            var entrys = db.Diaryentrys.Where(e =>
                            e.Author == user.Uuid &&
                            e.Tags.ToLower().Contains(tag.ToLower())
                         ).ToList();

            return entrys;
        }
        internal static List<Diaryentry> items_by_argument(string argument, User user)
        {
            var db = new HealthBotContext();
            var entrys = db.Diaryentrys.Where(e => 
                            e.Author == user.Uuid &&
                            (
                                e.Tags.ToLower().Contains(argument.ToLower()) ||
                                e.Name.ToLower().Contains(argument.ToLower())
                            )
                         )
                         .ToList();

            return entrys;
        }
        public static string user_data_export(User user)
        {
            var db = new HealthBotContext();
            var diary_entrys = db.Diaryentrys.Where(e => e.Author == user.Uuid);
            var biometry = db.Biometries.Where(b => b.Author == user.Uuid);
            
            return JsonSerializer.Serialize(user) + "\n\n" + JsonSerializer.Serialize(diary_entrys) + "\n\n" + JsonSerializer.Serialize(biometry);
        }
    }
}
