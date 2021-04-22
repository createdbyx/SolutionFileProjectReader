using System;

namespace SolutionFileProjectReader
{
    public class ConsoleUtility
    {
        public static void Write(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
        }

        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(message);
        }

        public static void WriteInfo(string message, bool addLine = false)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            if (addLine)
            {
                Console.WriteLine();
            }
        }

        public static void WriteStep(string message, bool addLine = false)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            if (addLine)
            {
                Console.WriteLine();
            }
        }

        public static void NewLine()
        {
            Console.WriteLine();
        }

        public static void ClearLine(int top)
        {
            int currentLineCursor = top;
            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void GoToLine(int top)
        {
            Console.SetCursorPosition(0, top);
        }
    }
}
