using Telegram.Bot.Types;

namespace TaskBot.Services
{
    internal static class Transformation
    {
        private static string Calculate(Message message, IStorage _memoryStorage)
        {
            int result = 0;
            string[] words = message.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                try
                {
                    result += int.Parse(word);
                }
                catch (Exception)
                {
                    return "Неверный ввод!";
                } 
            }

            return result.ToString();
        }
        public static string Run(Message message, IStorage _memoryStorage)
        {
            switch (_memoryStorage.GetSession(message.Chat.Id).TypeFunction)
            {
                case "string":
                    return message.Text.Length.ToString();
                case "number":
                    return Calculate(message, _memoryStorage);
                default:
                    return "Выберите нужную операцию в главном меню!";
            }
        }
    }
}
