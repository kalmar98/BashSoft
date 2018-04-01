using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;


namespace BashSoft.IO.Commands
{
    [Alias("mkdir")]
    class MakeDirectoryCommand : Command
    {
        [Inject]
        private IDirectoryManager ioManager;

        public MakeDirectoryCommand(string input, string[] data)
               : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 2)
            {
                throw new InvalidCommandException(this.Input);

            }

            string folderName = Data[1];
            this.ioManager.CreateDirectoryInCurrentFolder(folderName);

        }
    }
}
