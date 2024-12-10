using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("BOT TOKEN", cancellationToken: cts.Token);

var isFirstNumberSended = false;
var firstNumber = 0;
var lastNumber = 0;
var isBotStarted = false;

bot.OnMessage += OnMessage;

Console.ReadLine();
cts.Cancel();

async Task OnMessage(Message msg, UpdateType type)
{
    int firstNumb = firstNumber;
    int lastNumb = lastNumber;

    bool isFirstSend = isFirstNumberSended;
    bool invalidNumb = false;
    Random rand = new Random();

    if (msg.Text != null && msg.Text.ToLower() != "/start" && msg.Text.ToLower() != "/reset" && msg.Text.ToLower() != "/stop" && isBotStarted)
    {
        Console.WriteLine($"{msg.Chat}: {msg.Text}");

        if (!isFirstSend)
        {
            if (int.TryParse(msg.Text, out _))
            {
                isFirstSend = true;
                firstNumb = Convert.ToInt32(msg.Text);
                await bot.SendMessage(msg.Chat, Convert.ToString($"Первое число: {firstNumb} \nВторое число: {lastNumb}"));
                invalidNumb = false;
            }
            else
            {
                await bot.SendMessage(msg.Chat, Convert.ToString($"Такого числа быть не может!")); // invalid number
                invalidNumb = true;
            }
        }
        else
        {
            if (int.TryParse(msg.Text, out _))
            {
                lastNumb = Convert.ToInt32(msg.Text);
                await bot.SendMessage(msg.Chat, Convert.ToString($"Первое число: {firstNumb} \nВторое число: {lastNumb}"));
            }
            else
            {
                await bot.SendMessage(msg.Chat, Convert.ToString($"Такого числа быть не может!")); // invalid number
                invalidNumb = true;
            }
        }

        if (isFirstNumberSended)
        {
            if (firstNumb > lastNumb && !invalidNumb)
            {
                firstNumb = lastNumb;
                lastNumb = firstNumber;

                int value = rand.Next(firstNumb, lastNumb + 1);
                await bot.SendMessage(msg.Chat, Convert.ToString($"Результат: {value}"));

                Reset();
            }
            else
            {
                int value = rand.Next(firstNumb, lastNumb+1);
                await bot.SendMessage(msg.Chat, Convert.ToString($"Результат: {value}"));

                Reset();
            }
        }
        else
        {
            isFirstNumberSended = isFirstSend;
            firstNumber = firstNumb; lastNumber = lastNumb;
            isFirstSend = isFirstNumberSended;
        }
    }
    if (msg.Text.ToLower() == "/start" || msg.Text.ToLower() == "/start@test_papas_csharp_bot" && !isBotStarted) // bot start command
    { 
        await bot.SendMessage(msg.Chat, Convert.ToString("Привет! Это бот-рандомайзер. Напиши любые 2 числа, отправь их мне разными сообщениями и я их зарандомлю!"));
        isBotStarted = true;
        Reset();
    }

    if (msg.Text.ToLower() == "/stop" || msg.Text.ToLower() == "/stop@test_papas_csharp_bot" && isBotStarted) // bot stop command
    {
        isBotStarted = false;
        Reset();
    }

    if (msg.Text.ToLower() == "/reset" || msg.Text.ToLower() == "/reset@test_papas_csharp_bot") // bot reset command
    { 
        await bot.SendMessage(msg.Chat, Convert.ToString("Все числа обнулены! Отправь числа заново."));
        Reset();
    }
}

async Task Reset() // reset values
{
    isFirstNumberSended = false;
    firstNumber = 0; lastNumber = 0;
}

// "bot here: @test_papas_csharp_bot (it's off)"