using Telegram.Bot;
using Telegram.Bot.Types;
using _Config;
using Telegram.Bot.Types.ReplyMarkups;
using Bot.code;
using Microsoft.Data.SqlClient;

namespace Bot.scripts
{
    class Command
    {
        public static Dictionary<long, User_Instance> _data;
        public static ITelegramBotClient _botclient;
        public static void Initialize(Dictionary<long, User_Instance> data, ITelegramBotClient botclient)
        {
            _data = data;
            _botclient = botclient;
        }
        public static void User_load(Update _upd)
        {
            long id = 0;
            if (_upd.Message != null) id = _upd.Message.Chat.Id;
            if (_upd.CallbackQuery != null) id = _upd.CallbackQuery.From.Id;
            _Sql_Data.user_load(_upd.Message, id);
        } // loads user info from database
        public static async void Exit_seq()
        {
            var connection = new SqlConnection(Config.connection);
            connection.Open();
            foreach (var a in _data)
            {
                long chat_id = a.Key;
                SqlCommand insert = new SqlCommand(
                $"INSERT INTO User_Base (chat_id, state, difficulty elements) $" +
                $"VALUES (@chat_id, @state, @difficulty, @elements) $" +
                $"ON CONFLICT (Chat_Id) DO UPDATE $" +
                $"SET State = @state, Difficulty = @difficulty,Elements = @elements",
                connection);

                insert.Parameters.AddWithValue("chat_id", chat_id);
                insert.Parameters.AddWithValue("state", Convert.ToInt32(_data[chat_id].state));

                insert.ExecuteNonQuery();

                await _botclient.SendTextMessageAsync(chat_id, $"For now bot is going offline, sorry for inconvenience.");
            }
            connection.Close();
        } // safe exit, saves all current user data and notifies them about bot going down
        public static async void Start_seq()
        {
            var connection = new SqlConnection(Config.connection);
            connection.Open();
            SqlCommand allinfo = new SqlCommand($"SELECT * FROM User_Base", connection);
            SqlDataReader reader = allinfo.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                connection.Close();
                return;
            }
            while (reader.Read())
            {
                long cid = reader.GetInt32(0);
                if (!_data.ContainsKey(cid))
                {
                    State state;
                    switch (Convert.ToInt32(reader.GetValue(1)))
                    {
                        case 0: state = State.Menu; break;
                        case 1: state = State.Game0; break;
                        case 2: state = State.Game1; break;
                        case 3: state = State.Conclusion; break;
                        case 4: state = State.Stats; break;
                        default: state = State.Menu; break;
                    }
                    int difficulty = Convert.ToInt32(reader.GetValue(2)); // difficulty
                    var inst = new User_Instance(state);
                    _data.Add(cid, inst);
                    await _botclient.SendTextMessageAsync(cid, $"Bot is once again online!");
                }
            }
            reader.Close();
            connection.Close();
        }// safe start command, loads all user data and notifies them that bot is up
        public static async Task Send(long cid, string message, InlineKeyboardMarkup keyboard)
        {
            await _botclient.SendTextMessageAsync(
                chatId: cid,
                text: message,
                replyMarkup: keyboard);
        }
        public static async Task Send(long cid, string message, InlineKeyboardMarkup keyboard, int _messageId)
        {
            await _botclient.EditMessageTextAsync(
                chatId: cid,
                messageId: _messageId,
                text: message,
                replyMarkup: keyboard);
        }
        public static async Task Send(long cid, InlineKeyboardMarkup keyboard, int _messageId)
        {
            await _botclient.EditMessageReplyMarkupAsync(
                chatId: cid,
                messageId: _messageId,
                replyMarkup: keyboard);
        }
    }
}