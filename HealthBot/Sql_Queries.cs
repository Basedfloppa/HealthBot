
using DataModel;
using System.Data;
using Telegram.Bot.Types;

namespace Sql_Queries
{
    internal static class Sql_Queries
    {
        internal static double calories_by_date(DateTime date_max, DateTime date_min, DataModel.User user)
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
        internal static double water_by_date(DateTime date_max, DateTime date_min, DataModel.User user)
        {
            using (var db = new HealthBotDB())
            {
                var entrys = from a
                in db.DiaryEntrys
                             where a.Author == user.Uuid && (DateTime.Compare(a.CreatedAt.DateTime,date_min) == 0 || DateTime.Compare(a.CreatedAt.DateTime, date_max) == 1)
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
    }
}
