using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;

namespace BashSoft.IO.Commands
{
    [Alias("order")]
    class PrintOrderedStudentsCommand:Command
    {
        [Inject]
        private IDatabase repository;

        public PrintOrderedStudentsCommand(string input, string[] data)
               : base(input, data)
        {
        }

        public override void Execute()
        {
            if (Data.Length == 5)
            {
                string course = Data[1];
                string filter = Data[2].ToLower();
                string command = Data[3].ToLower();
                string quantity = Data[4].ToLower();
                ParseParametersForOrderAndTake(command, quantity, course, filter);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }

        private void ParseParametersForOrderAndTake(string command, string quantity, string course, string filter)
        {
            if (command == "take")
            {
                if (quantity == "all")
                {
                    this.repository.OrderAndTake(course, filter);
                }
                else
                {
                    int studentsToTake;
                    bool hasParsed = int.TryParse(quantity, out studentsToTake);
                    if (hasParsed)
                    {
                        this.repository.OrderAndTake(course, filter, studentsToTake);
                    }
                    else
                    {
                        OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
                    }
                }
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
            }
        }
    }
}
