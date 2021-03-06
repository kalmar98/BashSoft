﻿using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;

namespace BashSoft.IO.Commands
{
    [Alias("dropDb")]
    class DropDatabaseCommand:Command
    {
        [Inject]
        private IDatabase repository;

        public DropDatabaseCommand(string input, string[] data)
               : base(input, data)
        {
        }

        public override void Execute()
        {
            if (Data.Length != 1)
            {
                throw new InvalidCommandException(this.Input);
            }
            else
            {
                this.repository.UnloadData();
                OutputWriter.WriteMessageOnNewLine("Database dropped!");
            }
        }
    }
}
