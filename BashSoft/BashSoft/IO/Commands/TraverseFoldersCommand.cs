using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;

namespace BashSoft.IO.Commands
{
    [Alias("ls")]
    class TraverseFoldersCommand:Command
    {
        [Inject]
        private IDirectoryManager ioManager;

        public TraverseFoldersCommand(string input, string[] data)
               : base(input, data)
        {
        }

        public override void Execute()
        {
            if (Data.Length == 1)
            {
                this.ioManager.TraverseDirectory(0);
            }
            else if (Data.Length == 2)
            {
                int depth;
                bool hasParsed = int.TryParse(Data[1], out depth);

                if (hasParsed)
                {
                    this.ioManager.TraverseDirectory(depth);
                }
                else
                {
                    OutputWriter.DisplayException(ExceptionMessages.UnableToParseNumber);
                }
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
