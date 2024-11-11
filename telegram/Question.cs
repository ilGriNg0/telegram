using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace telegram
{
    public class Question
    {
        public string Text { get; set; }
        public string[] Options { get; set; }
        public int CorrectOptionIndex { get; set; }

        public Question(string text, string[] options, int correctOptionIndex)
        {
            Text = text;
            Options = options;
            CorrectOptionIndex = correctOptionIndex;
        }

        public bool IsCorrectAnswer(int optionIndex) => optionIndex == CorrectOptionIndex;
    }
}
