using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using TaskBot.Services;

namespace TaskBot.Controllers
{
    public class TextMessageController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Посчитать символы" , $"string"),
                        InlineKeyboardButton.WithCallbackData($" Вычислить сумму" , $"number")
                    });

                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b> Наш бот умеет считать символы в строке, а так же вычислять сумму чисел.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Можно пользоваться как калькулятором.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;

                default:
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, Transformation.Run(message, _memoryStorage), cancellationToken: ct);
                    break;
            }

            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
        }
    }
}
