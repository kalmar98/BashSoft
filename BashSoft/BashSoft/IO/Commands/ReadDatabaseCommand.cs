﻿using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;

namespace BashSoft.IO.Commands
{
    [Alias("readDb")]
    class ReadDatabaseCommand:Command
    {
        [Inject]
        private IDatabase repository;

        public ReadDatabaseCommand(string input, string[] data)
               : base(input, data)
        {
        }

        public override void Execute()
        {
            if (Data.Length == 2)
            {
                string fileName = Data[1];
                this.repository.LoadData(fileName);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
