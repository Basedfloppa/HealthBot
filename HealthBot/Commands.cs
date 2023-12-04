using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Bot.code;
using DataModel;
using LinqToDB;
using Configuration;
using LinqToDB.Data;


namespace Bot.scripts
{
    class Command
    {
        public static Dictionary<long, DataModel.User> data = new Dictionary<long, DataModel.User> { };
        public static ITelegramBotClient bot_client;
        public static HealthBotDB db;
        public static void Initialize(Dictionary<long, DataModel.User> _data, ITelegramBotClient _bot_client)
        {
            data = _data;
            bot_client = _bot_client;
            DataConnection.DefaultSettings = new MySettings();
            db = new HealthBotDB("HealthBot");
        }
        public static void User_load(Update upd) // loads user info from database
        {
            long chat_id = 0;
            Telegram.Bot.Types.User user = new Telegram.Bot.Types.User();

            if (upd.Message != null) 
            {
                chat_id = upd.Message.Chat.Id;
                user = upd.Message.From;
            }
            if (upd.CallbackQuery != null) 
            {
                chat_id = upd.CallbackQuery.From.Id;
                user = upd.CallbackQuery.From;
            }

            var query = db.Users.SingleOrDefault(u => u.ChatId == chat_id);

            if (query != null) { data.Add(chat_id, query); }
            else
            {
                var instance = new DataModel.User
                {
                    State = State.Menu.ToString(),
                    Name = $"{user.FirstName} {user.LastName}",
                    Alias = user.Username,
                    ChatId = chat_id,
                    CreatedAt = DateTime.Now
                };
                data.Add(chat_id, instance);
            }
        } 
        public static async void Exit_seq() // safe exit, saves all current user data and notifies them about bot going down
        {
            foreach (var user in data)
            {
                if (db.Users.Find(user.Value.Uuid) == null) 
                {
                    db.Insert(user);
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