using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;

namespace telegram
{
    public class QuizBot
    {
        private readonly ITelegramBotClient botClient;
        private readonly QuizGame quizGame;
        private readonly Dictionary<long, Question> activeQuestions;

        public QuizBot(string token)
        {
            botClient = new TelegramBotClient(token);
            quizGame = new QuizGame();
            activeQuestions = new Dictionary<long, Question>();
        }

        public void Start()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // получать все типы обновлений
            };

            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

            Console.WriteLine("Бот запущен...");
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
                return;

            var message = update.Message;

            if (message.Text!.ToLower() == "/start")
            {
                await botClient.SendMessage(message.Chat.Id, "Добро пожаловать в викторину! Начнём?", cancellationToken: cancellationToken);
                await AskNextQuestion(message.Chat.Id, cancellationToken);
            }
            else if (activeQuestions.ContainsKey(message.Chat.Id))
            {
                var question = activeQuestions[message.Chat.Id];
                if (int.TryParse(message.Text, out int userAnswerIndex) && quizGame.CheckAnswer(question, userAnswerIndex - 1))
                {
                    await botClient.SendMessage(message.Chat.Id, "Правильно!", cancellationToken: cancellationToken);
                }
                else
                {
                    await botClient.SendMessage(message.Chat.Id, "Неправильно. Попробуйте еще раз.", cancellationToken: cancellationToken);
                }

                await AskNextQuestion(message.Chat.Id, cancellationToken);
            }
        }

        private async Task AskNextQuestion(long chatId, CancellationToken cancellationToken)
        {
            var question = quizGame.GetNextQuestion();
            if (question == null)
            {
                await botClient.SendMessage(chatId, "Викторина окончена! Спасибо за участие.", cancellationToken: cancellationToken);
                return;
            }

            activeQuestions[chatId] = question;

            var keyboard = new ReplyKeyboardMarkup(new[]
            {
            new KeyboardButton[] { "1", "2", "3", "4" }
        })
            {
                ResizeKeyboard = true
            };

            await botClient.SendMessage(chatId,
                $"{question.Text}\n1. {question.Options[0]}\n2. {question.Options[1]}\n3. {question.Options[2]}\n4. {question.Options[3]}",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Произошла ошибка: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
