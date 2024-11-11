using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using telegram;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramBotController : ControllerBase
    {
        private readonly ITelegramBotClient _botClient;
        private readonly QuizGame _quizGame;
        private readonly Dictionary<long, Question> _activeQuestions;

        public TelegramBotController(ITelegramBotClient botClient)
        {
            _botClient = botClient;
            _quizGame = new QuizGame();
            _activeQuestions = new Dictionary<long, Question>();
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update.Message?.Text != null)
            {
                var message = update.Message;
                long chatId = message.Chat.Id;

                if (message.Text.ToLower() == "/start")
                {
                    await _botClient.SendMessage(chatId, "Добро пожаловать в викторину! Начнём?");
                    await AskNextQuestion(chatId);
                }
                else if (_activeQuestions.ContainsKey(chatId))
                {
                    var question = _activeQuestions[chatId];
                    if (int.TryParse(message.Text, out int userAnswerIndex) && _quizGame.CheckAnswer(question, userAnswerIndex - 1))
                    {
                        await _botClient.SendMessage(chatId, "Правильно!");
                    }
                    else
                    {
                        await _botClient.SendMessage(chatId, "Неправильно. Попробуйте еще раз.");
                    }

                    await AskNextQuestion(chatId);
                }
            }

            return Ok();
        }

        private async Task AskNextQuestion(long chatId)
        {
            var question = _quizGame.GetNextQuestion();
            if (question == null)
            {
                await _botClient.SendMessage(chatId, "Викторина окончена! Спасибо за участие.");
                return;
            }

            _activeQuestions[chatId] = question;

            var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(new[]
            {
            new Telegram.Bot.Types.ReplyMarkups.KeyboardButton[] { "1", "2", "3", "4" }
        })
            {
                ResizeKeyboard = true
            };

            await _botClient.SendMessage(chatId,
                $"{question.Text}\n1. {question.Options[0]}\n2. {question.Options[1]}\n3. {question.Options[2]}\n4. {question.Options[3]}",
                replyMarkup: keyboard);
        }
    }
}
