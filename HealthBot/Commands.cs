using Bot.code;
using HealthBot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = HealthBot.User;

namespace Bot.scripts
{
    class Command
    {
        public static ITelegramBotClient bot_client;

        public static void Initialize(ITelegramBotClient _bot_client)
        {
            bot_client = _bot_client;
        }

        public static User User_create(Update upd, long chat_id) // creates user for database
        {
            Telegram.Bot.Types.User user = new Telegram.Bot.Types.User();

            if (upd.Message != null)
            {
                user = upd.Message.From; // cannot be null so no problem
            }
            if (upd.CallbackQuery != null)
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
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка во время отправки: {ex.Message}");
               
                throw;
            }
        }

        public static async Task Send(
            long chat_id,
            (string, InlineKeyboardMarkup) tuple,
            int message_id
        )
        {
            await bot_client.EditMessageTextAsync(
                chatId: chat_id,
                messageId: message_id,
                text: tuple.Item1,
                replyMarkup: tuple.Item2
            );
        }

        public static async Task Send(
            long chat_id,
            (string, InlineKeyboardMarkup) tuple,
            int message_id,
            string path
        )
        {
            try
            {
                await bot_client.EditMessageTextAsync(
                    chatId: chat_id,
                    messageId: message_id,
                    text: tuple.Item1,
                    replyMarkup: tuple.Item2
            );

            await using Stream stream = System.IO.File.OpenRead(path);

            await bot_client.SendDocumentAsync(
                chatId: chat_id,
                document: InputFile.FromStream(stream: stream, fileName: path.Split("/").Last())
            );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка во время отправки измененного сообщения: {ex.Message}");
                throw;
            }
        }

        public static async Task Destroy(long chat_id, int message_id)
        {
            await bot_client.DeleteMessageAsync(chat_id, message_id);
        }

        public static async Task Update<T>(T entry)
            where T : notnull, Generic
        {
            var db = new HealthBotContext();

            entry.UpdatedAt = DateTime.Now.ToUniversalTime();

            db.Update(entry);
            db.SaveChanges();
            db.Dispose();
        }
    }
}
