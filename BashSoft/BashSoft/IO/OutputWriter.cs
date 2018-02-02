using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BashSoft
{
    public static class OutputWriter
    {
        public static void WriteMessage(string message)
        {
            Console.Write(message);
        }
        public static void WriteMessageOnNewLine(string message)
        {
            Console.WriteLine(message);
        }
        public static void WriteEmptyLine()
        {
            Console.WriteLine();
        }
        public static void DisplayException(string message)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = currentColor;
        }
        public static void DisplayStudent(KeyValuePair <string,List<int>> student)
        {
            WriteMessageOnNewLine(string.Format($"{student.Key} - {string.Join(", ",student.Value)}"));
        }
        public static void DisplayHelp(string helpPath)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            string[] helpLines = File.ReadAllLines(helpPath);

            for (int i = 0; i < helpLines.Length; i++)
            {
                Console.WriteLine(helpLines[i]);
            }
            
            Console.ForegroundColor = currentColor;
        }

    }
}
