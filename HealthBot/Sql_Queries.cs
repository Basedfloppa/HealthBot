using DataModel;
using LinqToDB;
using System.Data;
using System.Text.Json;

namespace Sql_Queries
{
    internal static class Sql_Queries
    {
        internal static double average_calories_by_date(DateTime date_max, DateTime date_min, DataModel.User user)
        {
            using (var db = new HealthBotDB()) {
                var entrys = from a
                             in db.DiaryEntrys
                             where a.Author == user.Uuid && (DateTime.Compare(a.CreatedAt.DateTime, date_min) == 0 || DateTime.Compare(a.CreatedAt.DateTime, date_max) == 1)
                             select a;

                var counter = 0.0;
                var calories_summ = 0.0;

                foreach (var entry in entrys) 
                {
                    var items = from a
                                in db.IntakeItems
                                where a.DiaryEntry == entry.Uuid
                                select a;

                    foreach (var item in items) calories_summ += item.CaloryAmount.GetValueOrDefault(0);
                    counter++;
                }
                return calories_summ / counter;
            }
        }
        internal static double average_water_by_date(DateTime date_max, DateTime date_min, DataModel.User user)
        {
            using (var db = new HealthBotDB())
            {
                var entrys = from a
                             in db.DiaryEntrys
                             where a.Author == user.Uuid && (DateTime.Compare(a.CreatedAt.DateTime, date_min) == 0 || DateTime.Compare(a.CreatedAt.DateTime, date_max) == 1)
                             select a;

                var counter = 0.0;
                var calories_summ = 0.0;

                foreach (var entry in entrys)
                {
                    var items = from a
                                in db.IntakeItems
                                where a.DiaryEntry == entry.Uuid && a.State == "liquid"
                                select a;

                    foreach (var item in items) calories_summ += item.CaloryAmount.GetValueOrDefault(0);
                    counter++;
                }
                return calories_summ / counter;
            }
        }
        internal static List<IntakeItem> items_by_name(string name, DataModel.User user)
        {
            using (var db = new HealthBotDB())
            {
                var entry_ids = from entry
                                in db.DiaryEntrys
                                where entry.Author == user.Uuid
                                select entry.Uuid;

                return db.IntakeItems.Where(item => entry_ids.Contains(item.DiaryEntry) && item.Name.ToLower().Contains(name.ToLower())).ToList();
            }
        }
        internal static List<IntakeItem> items_by_tag(string tag, DataModel.User user)
        {
            using (var db = new HealthBotDB())
            {
                var entry_ids = from entry
                                in db.DiaryEntrys
                                where entry.Author == user.Uuid
                                select entry.Uuid;

                return db.IntakeItems.Where(item => entry_ids.Contains(item.DiaryEntry) && item.Tags.ToLower().Contains(tag.ToLower())).ToList();
            }
        }
        internal static List<IntakeItem> items_argument(string argument, DataModel.User user)
        {
            using (var db = new HealthBotDB())
            {
                var entry_ids = from entry
                                in db.DiaryEntrys
                                where entry.Author == user.Uuid
                                select entry.Uuid;

                return db.IntakeItems.Where(item => entry_ids.Contains(item.DiaryEntry) && (item.Tags.ToLower().Contains(argument.ToLower()) || item.Name.ToLower().Contains(argument.ToLower()))).ToList();
            }
        }
        internal static string user_data_export(long chat_id)
        {
            using (var db = new HealthBotDB())
            {
                var user = db.Users.SingleOrDefault(u => u.ChatId == chat_id);

                var diary_entrys = from entry 
                                   in db.DiaryEntrys
                                   where entry.Author == user.Uuid
                                   select entry;

                var diary_entry_ids = diary_entrys.Select(e => e.Uuid);

                var items = from entry 
                            in db.IntakeItems
                            where diary_entry_ids.Contains(entry.DiaryEntry)
                            select entry;

                return JsonSerializer.Serialize(user) + "\n\n" + JsonSerializer.Serialize(diary_entry_ids) + "\n\n" + JsonSerializer.Serialize(items);
            }
        }
    }
}
