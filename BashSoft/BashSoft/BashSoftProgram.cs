using BashSoft.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BashSoft
{
    class Launcher
    {
        public static void Main()
        {
            IContentComparer tester = new Tester();
            IDirectoryManager ioManager = new IOManager();
            IDatabase studentsRepository = new StudentsRepository(new RepositoryFilter(), new RepositorySorter());

            IInterpreter commandInterpreter = new CommandInterpreter(tester, studentsRepository, ioManager);
            IReader reader = new InputReader(commandInterpreter);

            reader.StartReadingCommands();
        }
    }
}
