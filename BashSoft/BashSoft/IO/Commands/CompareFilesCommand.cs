using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;

namespace BashSoft.IO.Commands
{
    [Alias("cmp")]
    class CompareFilesCommand : Command
    {

        [Inject]
        private IContentComparer judge;

        public CompareFilesCommand(string input, string[] data)
               : base(input, data)
        {
        }

        public override void Execute()
        {
            if (Data.Length == 3)
            {
                string firstPath = Data[1];
                string secondPath = Data[2];

                this.judge.CompareContent(firstPath, secondPath);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
