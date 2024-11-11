using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace telegram
{

    public class QuizGame
    {
        private readonly List<Question> questions;
        private int currentQuestionIndex;

        public QuizGame()
        {
            questions = new List<Question>
        {
            new Question("Какой цвет у неба?", new[] { "Синий", "Зелёный", "Красный", "Жёлтый" }, 0),
            new Question("Сколько дней в неделе?", new[] { "5", "6", "7", "8" }, 2),
            // Добавьте больше вопросов здесь
        };
            currentQuestionIndex = 0;
        }

        public Question GetNextQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                return questions[currentQuestionIndex++];
            }
            return null;
        }

        public bool CheckAnswer(Question question, int answerIndex) => question.IsCorrectAnswer(answerIndex);
    }

}
