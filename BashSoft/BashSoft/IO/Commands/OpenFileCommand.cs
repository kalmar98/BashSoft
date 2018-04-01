using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;
using System;
using System.Diagnostics;

namespace BashSoft.IO.Commands
{
    [Alias("open")]
    class OpenFileCommand : Command
    {
        public OpenFileCommand(string input, string[] data)
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 2)
            {
                throw new InvalidCommandException(this.Input);
            }
            try
            {
                string filename = Data[1];
                Process.Start(SessionData.Path + "\\" + filename);
            }
            catch (Exception)
            {
                OutputWriter.DisplayException("Invalid file");
            }
        }
    }

}
