using Telegram.Bot;
using Telegram.Bot.Types;
using Configuration;
using Telegram.Bot.Types.ReplyMarkups;
using Bot.code;
using DataModel;
using LinqToDB;

namespace Bot.scripts
{
    class Command
    {
        public static List<DataModel.User> data = new List<DataModel.User> { };
        public static ITelegramBotClient bot_client;
        public static void Initialize(List<DataModel.User> _data, ITelegramBotClient _bot_client)
        {
            data = _data;
            bot_client = _bot_client;
        }
        public static void User_load(Update upd) // loads user info from database
        {
            long chat_id = 0;

            if (upd.Message != null) chat_id = upd.Message.Chat.Id;
            if (upd.CallbackQuery != null) chat_id = upd.CallbackQuery.From.Id;

            using (var db = new HealthBotDB("HealthBot"))
            {
                var query = db.Users.Where(u => u.ChatId == chat_id);

                if (query.Count() == 1)
                {
                    data.Add(query.First());
                }
                else
                {
                    var instance = new DataModel.User();
                    instance.State = State.Menu.ToString();
                    instance.ChatId = chat_id;
                    instance.CreatedAt = DateTime.Now;
                    data.Add(instance);
                }
            }
        } 
        public static async void Exit_seq() // safe exit, saves all current user data and notifies them about bot going down
        {
            await using (var db = new HealthBotDB("HealthBotDB"))
            {
                foreach (var user in data)
                {
                    if (db.Users.Find(user.Uuid) == null) 
                    {
                        db.Insert(user);
                    }
                    else if (user != db.Users.Find(user.Uuid)) 
                    {
                        user.UpdatedAt = DateTime.Now;
                        db.Update(user);
                    }
                    
                    await bot_client.SendTextMessageAsync(user.ChatId, $"For now bot is going offline, sorry for the inconvenience.");
                }
            }
        }
        public static async void Start_seq() // safe start command, loads all user data and notifies them that bot is up
        {
            using (var db = new HealthBotDB("HealthBotDB"))
            {
                var users = from u
                            in db.Users
                            select u;

                if (users != null) data = users.ToList(); 
            }
        }
        public static async Task Send(long chat_id, string message, InlineKeyboardMarkup keyboard)
        {
            await bot_client.SendTextMessageAsync(
                chatId: chat_id,
                text: message,
                replyMarkup: keyboard);
        }
        public static async Task Send(long chat_id, string message, InlineKeyboardMarkup keyboard, int message_id)
        {
            await bot_client.EditMessageTextAsync(
                chatId: chat_id,
                messageId: message_id,
                text: message,
                replyMarkup: keyboard);
        }
        public static async Task Send(long chat_id, InlineKeyboardMarkup keyboard, int message_id)
        {
            await bot_client.EditMessageReplyMarkupAsync(
                chatId: chat_id,
                messageId: message_id,
                replyMarkup: keyboard);
        }
    }
}