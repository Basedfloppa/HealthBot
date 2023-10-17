using _Config;
using Bot.code;
using Telegram.Bot.Types;
using Microsoft.Data.SqlClient;

namespace Bot.scripts
{
    internal class _Sql_Data
    {
        public static void user_load(Message message, long chat_id)
        {
            var connection = new SqlConnection(Config.connection);
            connection.Open();
            SqlCommand resultinf = new SqlCommand($"SELECT * FROM user_base WHERE chat_id = '{chat_id}'", connection);
            SqlDataReader reader = resultinf.ExecuteReader();
            if (reader.Depth == 1)
            {
                State state = new State();
                switch (Convert.ToInt32(reader.GetValue(1)))
                {
                    case 0: state = State.Menu; break;
                    case 1: state = State.Game0; break;
                    case 2: state = State.Game1; break;
                    case 3: state = State.Conclusion; break;
                    case 4: state = State.Stats; break;
                    default: state = State.Menu; break;
                }
                int difficulty = Convert.ToInt32(reader.GetValue(2));
                var inst = new User_Instance(state);
                Command._data.Add(chat_id, inst);
                reader.Close();
            }
            else
            {
                User_Instance a = new User_Instance(State.Menu);
                Command._data.Add(chat_id, a);
            }
            connection.Close();
        }
    }
}