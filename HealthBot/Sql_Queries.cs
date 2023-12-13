using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Bot.scripts;
using DataModel;

namespace Sql_Queries
{
    internal static class Sql_Queries
    {
        internal static double AverageCaloriesByDate(DateTime dateMax, DateTime dateMin, User user)
        {
            var entrys = Command.db.DiaryEntrys
                .Where(a => a.Author == user.Uuid && (a.CreatedAt >= dateMin && a.CreatedAt <= dateMax));

            var counter = 0.0;
            var caloriesSumm = 0.0;

            foreach (var entry in entrys)
            {
                var items = Command.db.IntakeItems
                    .Where(a => a.DiaryEntry == entry.Uuid);

                foreach (var item in items) caloriesSumm += item.CaloryAmount.GetValueOrDefault(0);
                counter++;
            }
            return caloriesSumm / counter;
        }

        internal static double AverageWaterByDate(DateTime dateMax, DateTime dateMin, User user)
        {
            var entrys = Command.db.DiaryEntrys
                .Where(a => a.Author == user.Uuid && (a.CreatedAt >= dateMin && a.CreatedAt <= dateMax));

            var counter = 0.0;
            var caloriesSumm = 0.0;

            foreach (var entry in entrys)
            {
                var items = Command.db.IntakeItems
                    .Where(a => a.DiaryEntry == entry.Uuid && a.State == "liquid");

                foreach (var item in items) caloriesSumm += item.CaloryAmount.GetValueOrDefault(0);
                counter++;
            }
            return caloriesSumm / counter;
        }

        internal static List<IntakeItem> ItemsByName(string name, User user)
        {
            var entryIds = Command.db.DiaryEntrys
                .Where(entry => entry.Author == user.Uuid)
                .Select(entry => entry.Uuid);

            return Command.db.IntakeItems
                .Where(item => entryIds.Contains(item.DiaryEntry) && item.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }

        internal static List<IntakeItem> ItemsByTag(string tag, User user)
        {
            var entryIds = Command.db.DiaryEntrys
                .Where(entry => entry.Author == user.Uuid)
                .Select(entry => entry.Uuid);

            return Command.db.IntakeItems
                .Where(item => entryIds.Contains(item.DiaryEntry) && item.Tags.ToLower().Contains(tag.ToLower()))
                .ToList();
        }

        internal static List<IntakeItem> ItemsByArgument(string argument, User user)
        {
            var entryIds = Command.db.DiaryEntrys
                .Where(entry => entry.Author == user.Uuid)
                .Select(entry => entry.Uuid);

            return Command.db.IntakeItems
                .Where(item => entryIds.Contains(item.DiaryEntry) && (item.Tags.ToLower().Contains(argument.ToLower()) || item.Name.ToLower().Contains(argument.ToLower())))
                .ToList();
        }

        internal static string UserDataExport(long chatId)
        {
            var user = Command.db.Users.SingleOrDefault(u => u.ChatId == chatId);

            var diaryEntrys = Command.db.DiaryEntrys
                .Where(entry => entry.Author == user.Uuid);

            var diaryEntryIds = diaryEntrys.Select(entry => entry.Uuid);

            var items = Command.db.IntakeItems
                .Where(entry => diaryEntryIds.Contains(entry.DiaryEntry));

            var biometry = Command.db.Biometries
                .Where(entry => entry.Author == user.Uuid);

            return JsonSerializer.Serialize(user) + "\n\n" + JsonSerializer.Serialize(diaryEntryIds) + "\n\n" + JsonSerializer.Serialize(items) + "\n\n" + JsonSerializer.Serialize(biometry);
        }
    }
}
