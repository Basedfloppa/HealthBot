using Bot.code;
using HealthBot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = HealthBot.User;
using ScottPlot;
using System.Data.Common;
using Configuration;

namespace Bot.scripts
{
    static class Command
    {
        public static ITelegramBotClient? bot_client;

        public static void Initialize(ITelegramBotClient _bot_client)
        {
            bot_client = _bot_client;
        }

        public static class Message
        {
            public static async Task Send(long chat_id, (string, InlineKeyboardMarkup) tuple)
            {
                try
                {
                    await bot_client.SendTextMessageAsync(
                        chatId: chat_id,
                        text: tuple.Item1,
                        replyMarkup: tuple.Item2
                );
                }
                catch(Exception e)
                {
                    await Help.Warn($"Command.Send method encountered {e.Message} , chat_id: {chat_id}");
                }
            }

            public static async Task Send(long chat_id, (string, InlineKeyboardMarkup) tuple, int message_id )
            {
                try
                {
                    await bot_client.EditMessageTextAsync(
                        chatId: chat_id,
                        messageId: message_id,
                        text: tuple.Item1,
                        replyMarkup: tuple.Item2
                    );
                }
                catch(Exception e)
                {
                    await Help.Warn($"Command.Send method encountered {e.Message} , chat_id: {chat_id} , message_id: {message_id}");
                }
            }

            public static async Task Send(long chat_id, (string, InlineKeyboardMarkup) tuple, int message_id, string path)
            {      
                await using Stream stream = System.IO.File.OpenRead(path);

                try
                {
                    var type = path.Split('.').Last();
                    Telegram.Bot.Types.Message msg = new();
                    var db = new HealthBotContext();
                    var media = new Media();

                    if ( type == "png" ) {
                        msg = await bot_client.SendPhotoAsync(
                        chatId: chat_id,
                        photo: InputFile.FromStream(stream: stream, fileName: path.Split("/").Last())
                        );
                        media.MessageId = msg.MessageId;
                        media.ChatId = chat_id;
                        db.Media.Add(media);
                        await db.SaveChangesAsync();
                    }
                    else if ( type == "json" ) {
                        msg = await bot_client.SendDocumentAsync(
                        chatId: chat_id,
                        document: InputFile.FromStream(stream: stream, fileName: path.Split("/").Last())
                        );
                        media.MessageId = msg.MessageId;
                        media.ChatId = chat_id;
                        db.Media.Add(media);
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    await Help.Warn($"Command.Send encountered: {ex.Message} , path: {path}");
                }

                try
                {
                    await bot_client.EditMessageTextAsync(
                        chatId: chat_id,
                        messageId: message_id,
                        text: tuple.Item1,
                        replyMarkup: tuple.Item2
                    );
                }
                catch(Exception e)
                {
                    await Help.Warn($"Command.Send method encountered {e.Message} , chat_id: {chat_id} , message_id: {message_id}");
                }
            }

            public static async Task ClearMedia(long chat_id)
            {
                var db = new HealthBotContext();
                var media = db.Media.Where(x => x.ChatId == chat_id);
                foreach (var entry in media){
                    Destroy(entry.ChatId, entry.MessageId);
                }
                db.RemoveRange(media);
                await db.SaveChangesAsync();
            }

            public static async Task Destroy(long chat_id, int message_id)
            {
                try
                {
                    await bot_client.DeleteMessageAsync(chat_id, message_id);
                }
                catch(Exception e)
                {
                    await Help.Warn($"Command.Destroy method encountered {e.Message} , chat_id: {chat_id} , message_id: {message_id}");
                }
            }
        }
        public static class Database
        {
            public static User User_create(Update upd, long chat_id) // creates user for database
            {
                Telegram.Bot.Types.User user = new Telegram.Bot.Types.User();

                if (upd != null && upd.Message != null )
                {
                    user = upd.Message.From;
                }
                else if (upd.CallbackQuery != null)
                {
                    user = upd.CallbackQuery.From;
                }

                var db = new HealthBotContext();

                User instance = new User
                {
                    Name = $"{user?.FirstName} {user?.LastName}",
                    Alias = user?.Username,
                    ChatId = chat_id,
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };

                db.Users.Add(instance);
                db.SaveChanges();
                db.Dispose();

                return instance;
            }
            public static async Task Update<T>(T entry) where T : notnull, Generic
            {
                var db = new HealthBotContext();

                entry.UpdatedAt = DateTime.Now.ToUniversalTime();

                try
                {
                    db.Update(entry);
                    await db.SaveChangesAsync();
                }
                catch(Exception e)
                {
                    await Help.Warn($"Command.Update method encountered {e.Message} , entry: {entry}");
                }
            }
            public static async Task<string> Generate_graphics(List<DateTime> dates, List<Double> values, string name)
            {
                string filepath = string.Empty;
                try
                {
                    var plt = new ScottPlot.Plot();

                    double[] timestamps = dates.Select(date => date.ToOADate()).ToArray();
                    plt.PlotScatter(timestamps, values.ToArray()); // adding data to chart

                    plt.XTicks(timestamps, dates.Select(date => date.ToString("dd.MM.yyyy")).ToArray());
                    plt.XAxis.TickLabelStyle(rotation: 45);
                    plt.XAxis.SetSizeLimit(min: 50);

                    filepath = "line.png";
                    plt.SaveFig(filepath);

                    
                }
                catch(Exception e)
                {
                    await Help.Warn($"Command.Generate_graphics method encountered {e.Message} , values: {values}, dates: {dates}, name: {name}");
                }
                return filepath;
            }
            public static async Task Seed()
            {
                var db = new HealthBotContext();

                for(int i = 0; i < 10; i++)
                {
                    var entry_solid = new Diaryentry{ Uuid = Guid.NewGuid(),
                                                    Author = Config.admin_alias, 
                                                    CaloryAmount = Faker.RandomNumber.Next(1, 200), 
                                                    State = "solid", 
                                                    Weight = Faker.RandomNumber.Next(1,200), 
                                                    Type = "IntakeItem"};
                    db.Add(entry_solid);
                    var entry_liquid = new Diaryentry{ Uuid = Guid.NewGuid(),
                                                     Author = Config.admin_alias, 
                                                     CaloryAmount = Faker.RandomNumber.Next(1, 200), 
                                                     State = "liquid", 
                                                     Weight = Faker.RandomNumber.Next(1,200), 
                                                     Type = "IntakeItem"};
                    db.Add(entry_solid);
                }
                await db.SaveChangesAsync();
            }
        }
        public static class Help
        {
            public static Task Warn(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(message);
                Console.ResetColor();
                Console.Write("\n");

                return Task.CompletedTask;
            }

            public static Task Info(string message)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(message);
                Console.ResetColor();
                Console.Write("\n");

                return Task.CompletedTask;
            }

            public static Task Success(string message)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(message);
                Console.ResetColor();
                Console.Write("\n");

                return Task.CompletedTask;
            }

            public static (DateTime max, DateTime min) ParseDates(string input)
            {
                var date_min = Convert.ToDateTime(input.Replace(" ", "").Split("-")[0]).ToUniversalTime();
                var date_max = Convert.ToDateTime(input.Replace(" ", "").Split("-")[1]).ToUniversalTime();

                if (date_max.Subtract(date_min).TotalDays < 0) (date_min, date_max) = (date_max, date_min);

                return (date_max, date_min);
            }
        }
    }
}
