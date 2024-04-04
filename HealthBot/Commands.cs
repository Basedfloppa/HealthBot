using Bot.code;
using HealthBot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = HealthBot.User;
using ScottPlot;

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
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Command.Send method encountered {e.Message} , chat_id: {chat_id}");
                    Console.ResetColor();
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
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Command.Send method encountered {e.Message} , chat_id: {chat_id} , message_id: {message_id}");
                    Console.ResetColor();
                }
            }

            public static async Task Send(long chat_id, (string, InlineKeyboardMarkup) tuple, int message_id, string path)
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
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Command.Send method encountered {e.Message} , chat_id: {chat_id} , message_id: {message_id}");
                    Console.ResetColor();
                }

                await using Stream stream = System.IO.File.OpenRead(path);

                

                try
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    await bot_client.SendDocumentAsync(
                    chatId: chat_id,
                    document: InputFile.FromStream(stream: stream, fileName: path.Split("/").Last())
                    );
                    
                }
                catch (Exception ex)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Command.Send encountered: {ex.Message} , path: {path}");
                    Console.ResetColor();
                }
                try
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    await bot_client.SendPhotoAsync(
                    chatId: chat_id,
                    photo: InputFile.FromStream(stream: stream, fileName: path.Split("/").Last())
                    );
                }
                catch (Exception ex)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Command.Send encountered: {ex.Message} , path: {path}");
                    Console.ResetColor();
                }
            }

            public static async Task Destroy(long chat_id, int message_id)
            {
                try
                {
                    await bot_client.DeleteMessageAsync(chat_id, message_id);
                }
                catch(Exception e)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Command.Destroy method encountered {e.Message} , chat_id: {chat_id} , message_id: {message_id}");
                    Console.ResetColor();
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
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Command.Update method encountered {e.Message} , entry: {entry}");
                    Console.ResetColor();
                }
                finally
                {
                    await db.DisposeAsync();
                }
            }

            public static async Task<string> Generate_graphics(List<DateTime> dates, List<Double> values, string name)
            {
                try
                {
                    var plt = new ScottPlot.Plot();

                    double[] timestamps = dates.Select(date => date.ToOADate()).ToArray();
                    plt.PlotScatter(timestamps, values.ToArray()); // adding data to chart

                    plt.XTicks(timestamps, dates.Select(date => date.ToString("dd.MM.yyyy")).ToArray());
                    plt.XAxis.TickLabelStyle(rotation: 45);
                    plt.XAxis.SetSizeLimit(min: 50);

                    string filepath = "line.png";
                    plt.SaveFig(filepath);

                    return filepath;
                }
                catch(Exception e)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Command.Generate_graphics method encountered {e.Message} , values: {values}, dates: {dates}, name: {name}");
                    Console.ResetColor();

                    return "";
                }
            }
        }
    }
}
