﻿using Telegram.Bot;
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
        public static void Initialize(Dictionary<long, User> _data, ITelegramBotClient _bot_client)
        {
            data = _data;
            bot_client = _bot_client;
        }
        public static async void User_load(Update upd) // loads user info from database
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

            var db = new HealthBotContext();
            var query = db.Users.SingleOrDefault(u => u.ChatId == chat_id);

            if (query != null) { data.Add(chat_id, query); }
            else
            {
                var instance = new User
                {
                    State = State.Menu.ToString(),
                    Name = $"{user.FirstName} {user.LastName}",
                    Alias = user.Username,
                    ChatId = chat_id,
                    CreatedAt = DateTime.Now
                };

                data.Add(chat_id, instance);
                db.Users.Add(instance);
                await db.SaveChangesAsync();
                db.Dispose();
            }
        } 
        public static async void Exit_seq() // safe exit, saves all current user data and notifies them about bot going down
        {
            var db = new HealthBotContext();

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

            db.Dispose();
        }
        public static void Start_seq() // safe start command, loads all user data and notifies them that bot is up
        {
            var db = new HealthBotContext();
            var users = db.Users.ToList();

            if (users != null)
            {
                foreach (var user in users)
                {
                    data.Add(user.ChatId, user);
                }
            } 

            db.Dispose();
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