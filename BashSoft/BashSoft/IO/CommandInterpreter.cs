using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;
using BashSoft.IO.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace BashSoft
{
    public class CommandInterpreter : IInterpreter
    {

        private IContentComparer judge;
        private IDatabase repository;
        private IDirectoryManager ioManager;

        public CommandInterpreter(IContentComparer judge, IDatabase repository, IDirectoryManager ioManager)
        {
            this.judge = judge;
            this.repository = repository;
            this.ioManager = ioManager;
        }



        public void InterpretCommand(string input)
        {
            string[] data = input.Split();
            string commandName = data[0];
            try
            {
                IExecutable command = this.ParseCommand(input, data, commandName);
                command.Execute();
            }
            catch (Exception e)
            {
                OutputWriter.DisplayException(e.Message);
            }

        }
        private IExecutable ParseCommand(string input, string[] data, string command)
        {
            object[] parametersForConstruction = new object[]
            {
                input,data
            };

            Type typeOfCommand =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .First(t => t.GetCustomAttributes(typeof(AliasAttribute))
                        .Where(atr => atr.Equals(command))
                        .ToArray().Length > 0);
            Type typeOfInterpreter = typeof(CommandInterpreter);
            Command exe = (Command)Activator.CreateInstance(typeOfCommand, parametersForConstruction);

            FieldInfo[] fieldsOfCommand = typeOfCommand
                                            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo[] fieldsOfInterpreter = typeOfInterpreter
                                                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fieldsOfCommand)
            {
                Attribute atr = field.GetCustomAttribute(typeof(InjectAttribute));
                if (atr != null)
                {
                    if(fieldsOfInterpreter.Any(x=>x.FieldType == field.FieldType))
                    {
                        field.SetValue(exe, fieldsOfInterpreter
                            .First(x => x.FieldType == field.FieldType)
                            .GetValue(this));
                    }
                }
            }

            return exe;
        }

    }
}
