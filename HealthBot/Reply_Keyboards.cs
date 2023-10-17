using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.scripts
{
    class Reply_Keyboards
    {
        public static InlineKeyboardMarkup InputBool()
        {
            InlineKeyboardMarkup result = new(new[]
            {
                InlineKeyboardButton.WithCallbackData(text: $"yes", callbackData: $"yes"),
                InlineKeyboardButton.WithCallbackData(text: $"no", callbackData: $"no")
            });
            return result;
        }
        public static InlineKeyboardMarkup Menu()
        {
        }
    }
}