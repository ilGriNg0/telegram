namespace telegram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите токен бота: ");
            //string token = Console.ReadLine();

            var quizBot = new QuizBot("7432101128:AAFHcX5aM7CJBXGToKOE0FxfSif0zIvZt3o");
            quizBot.Start();

            Console.ReadLine();
        }
    }
}
