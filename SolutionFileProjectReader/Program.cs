using System;

namespace SolutionFileProjectReader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ReferenceFinder.Run();
            }
            catch (Exception exp)
            {
                ConsoleUtility.WriteError(exp.Message);
            }

            ConsoleUtility.Write("Press any key to close...");
            Console.Read();
        }
    }
}
