using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;

namespace BashSoft.IO.Commands
{
    [Alias("cdRel")]
    class ChangePathRelativelyCommand:Command
    {
        [Inject]
        private IDirectoryManager ioManager;

        public ChangePathRelativelyCommand(string input, string[] data)
               : base(input, data)
        {
        }

        public override void Execute()
        {
            if (Data.Length == 2)
            {
                string relPath = Data[1];
                this.ioManager.ChangeCurrentDirectoryRelative(relPath);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
