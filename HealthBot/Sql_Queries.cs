using System.Data;
using System.Text.Json;
using Bot.scripts;
using HealthBot;
using User = HealthBot.User;

namespace Sql_Queries
{
    internal static class Sql_Queries
    {
        internal static double average_calories_by_date(DateTime date_max, DateTime date_min, User user)
        {
            var entrys = from a
                         in Command.db.Diaryentrys
                         where a.Author == user.Uuid && ( a.CreatedAt <= date_min || a.CreatedAt >= date_max)
                         select a;

            var counter = 0.0;
            var calories_summ = 0.0;

            foreach (var entry in entrys) 
            {
                var items = from a
                            in Command.db.IntakeItems
                            where a.DiaryEntry == entry.Uuid
                            select a;

                foreach (var item in items) calories_summ += item.CaloryAmount.GetValueOrDefault(0);
                counter++;
            }
            return calories_summ / counter;
        }
        internal static double average_water_by_date(DateTime date_max, DateTime date_min, User user)
        {
            var entrys = from a
                            in Command.db.Diaryentrys
                            where a.Author == user.Uuid && (DateTime.Compare(a.CreatedAt, date_min) == 0 || DateTime.Compare(a.CreatedAt, date_max) == 1)
                            select a;

            var counter = 0.0;
            var calories_summ = 0.0;

            foreach (var entry in entrys)
            {
                var items = from a
                            in Command.db.IntakeItems
                            where a.DiaryEntry == entry.Uuid && a.State == "liquid"
                            select a;

                foreach (var item in items) calories_summ += item.CaloryAmount.GetValueOrDefault(0);
                counter++;
            }
            return calories_summ / counter;
        }
        internal static List<IntakeItem> items_by_name(string name, User user)
        {
            var entry_ids = from entry
                            in Command.db.Diaryentrys
                            where entry.Author == user.Uuid
                            select entry.Uuid;

            return Command.db.IntakeItems.Where(item => entry_ids.Contains(item.DiaryEntry) && item.Name.ToLower().Contains(name.ToLower())).ToList();
        }
        internal static List<IntakeItem> items_by_tag(string tag, User user)
        {
            var entry_ids = from entry
                            in Command.db.Diaryentrys
                            where entry.Author == user.Uuid
                            select entry.Uuid;

            return Command.db.IntakeItems.Where(item => entry_ids.Contains(item.DiaryEntry) && item.Tags.ToLower().Contains(tag.ToLower())).ToList();
        }
        internal static List<IntakeItem> items_argument(string argument, User user)
        {
            var entry_ids = from entry
                            in Command.db.Diaryentrys
                            where entry.Author == user.Uuid
                            select entry.Uuid;

            return Command.db.IntakeItems.Where(item => entry_ids.Contains(item.DiaryEntry) && (item.Tags.ToLower().Contains(argument.ToLower()) || item.Name.ToLower().Contains(argument.ToLower()))).ToList();
        }
        internal static string user_data_export(long chat_id)
        {
            var user = Command.db.Users.SingleOrDefault(u => u.ChatId == chat_id);

            var diary_entrys = from entry 
                                in Command.db.Diaryentrys
                                where entry.Author == user.Uuid
                                select entry;

            var diary_entry_ids = diary_entrys.Select(e => e.Uuid);

            var items = from entry 
                        in Command.db.IntakeItems
                        where diary_entry_ids.Contains(entry.DiaryEntry)
                        select entry;

            var biometry = from entry
                           in Command.db.Biometries
                           where entry.Author == user.Uuid
                           select entry;

            return JsonSerializer.Serialize(user) + "\n\n" + JsonSerializer.Serialize(diary_entry_ids) + "\n\n" + JsonSerializer.Serialize(items) + "\n\n" + JsonSerializer.Serialize(biometry);
        }
    }
}
