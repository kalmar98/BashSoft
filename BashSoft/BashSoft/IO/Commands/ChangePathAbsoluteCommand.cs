using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;

namespace BashSoft.IO.Commands
{
    [Alias("cdAbs")]
    class ChangePathAbsoluteCommand : Command
    {
        [Inject]
        private IDirectoryManager ioManager;


        public ChangePathAbsoluteCommand(string input, string[] data)
               : base(input, data)
        {
        }

        public override void Execute()
        {
            if (Data.Length == 2)
            {
                string absolutePath = Data[1];
                this.ioManager.ChangeCurrentDirectoryAbsolute(absolutePath);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
