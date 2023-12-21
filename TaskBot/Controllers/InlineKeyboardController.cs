using TaskBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TaskBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            _memoryStorage.GetSession(callbackQuery.From.Id).TypeFunction = callbackQuery.Data;

            string typeFunction = callbackQuery.Data switch
            {
                "string" => " Посчитать символы",
                "number" => " Вычислить сумму",
                _ => String.Empty
            };

            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Выбранная операция - {typeFunction}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
            if(callbackQuery.Data == "string")
                await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"Введите строку для подсчета:", cancellationToken: ct);
            else
                await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"Введите числа через пробел для подсчета:", cancellationToken: ct);

            Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку");
        }
    }
}
