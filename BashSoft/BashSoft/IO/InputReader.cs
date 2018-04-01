using BashSoft.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BashSoft
{
    public class InputReader:IReader
    {
        private const string endCommand = "quit";
        private IInterpreter commandInterpreter;

        public InputReader(IInterpreter commandInterpreter)
        {
            this.commandInterpreter = commandInterpreter;
        }

        public void StartReadingCommands()
        {

            while (true)
            {
                OutputWriter.WriteMessage($"{SessionData.Path}> ");
                string input = Console.ReadLine();
                input = input.Trim();
                if (input == endCommand)
                {
                    break;
                }
                commandInterpreter.InterpretCommand(input);

            }

        }
    }
}
