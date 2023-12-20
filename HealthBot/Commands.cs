using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Bot.code;
using HealthBot;
using User = HealthBot.User;

namespace Bot.scripts
{
    class Command
    {
        public static Dictionary<long, User> data = new Dictionary<long, User> { };
        public static ITelegramBotClient bot_client;
        public static HealthBotContext db;
        public static void Initialize(Dictionary<long, User> _data, ITelegramBotClient _bot_client)
        {
            data = _data;
            bot_client = _bot_client;
            db = new HealthBotContext();
        }
        public static void User_load(Update upd) // loads user info from database
        {
            long chat_id = 0;

            if (upd.Message != null) chat_id = upd.Message.Chat.Id;
            if (upd.CallbackQuery != null) chat_id = upd.CallbackQuery.From.Id;

            var query = db.Users.Where(u => u.ChatId == chat_id);

            if (query.Count() == 1)
            {
                data.Add(chat_id, query.First());
            }
            else
            {
                var instance = new User();
                instance.State = State.Menu.ToString();
                instance.Name = upd.Message.From.FirstName + " " + upd.Message.From.LastName;
                instance.Alias = upd.Message.From.Username;
                instance.ChatId = chat_id;
                instance.CreatedAt = DateTime.Now;
                data.Add(chat_id, instance);
            }
        } 
        public static async void Exit_seq() // safe exit, saves all current user data and notifies them about bot going down
        {
            foreach (var user in data)
            {
                if (db.Users.Find(user.Value.Uuid) == null) 
                {
                    db.Add(user);
                }
                else if (user.Value != db.Users.Find(user.Value.Uuid)) 
                {
                    user.Value.UpdatedAt = DateTime.Now;
                    db.Update(user);
                }
                    
                await bot_client.SendTextMessageAsync(user.Value.ChatId, $"For now bot is going offline, sorry for the inconvenience.");
            }
        }
        public static void Start_seq() // safe start command, loads all user data and notifies them that bot is up
        {
            var users = db.Users.ToList();

            if (users != null)
            {
                foreach (var user in users)
                {
                    data.Add(user.ChatId, user);
                }
            } 
        }
        public static async Task Send(long chat_id, (string, InlineKeyboardMarkup) tuple)
        {
            await bot_client.SendTextMessageAsync(
                chatId: chat_id,
                text: tuple.Item1,
                replyMarkup: tuple.Item2);
        }
        public static async Task Send(long chat_id, (string, InlineKeyboardMarkup) tuple, int message_id)
        {
            await bot_client.EditMessageTextAsync(
                chatId: chat_id,
                messageId: message_id,
                text: tuple.Item1,
                replyMarkup: tuple.Item2);
        }
        public static async Task Destroy(long chat_id, int message_id)
        {
            await bot_client.DeleteMessageAsync(chat_id, message_id);
        }
    }
}