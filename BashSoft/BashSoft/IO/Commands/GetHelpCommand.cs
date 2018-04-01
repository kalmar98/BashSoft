using BashSoft.Attributes;
using BashSoft.Exceptions;

namespace BashSoft.IO.Commands
{
    [Alias("help")]
    class GetHelpCommand:Command
    {

        public GetHelpCommand(string input, string[] data)
               : base(input, data)
        {
        }

        public override void Execute()
        {
            if (Data.Length != 1)
            {
                throw new InvalidCommandException(this.Input);
            }

            string helpPath = SessionData.Path + "/Help/getHelp.txt";
            OutputWriter.DisplayHelp(helpPath);
        }


    }
}
