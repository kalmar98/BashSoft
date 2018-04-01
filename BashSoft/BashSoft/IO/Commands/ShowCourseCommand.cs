using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;

namespace BashSoft.IO.Commands
{
    [Alias("show")]
    class ShowCourseCommand : Command
    {
        [Inject]
        private IDatabase repository;

        public ShowCourseCommand(string input, string[] data)
               : base(input, data)
        {
        }

        public override void Execute()
        {
            if (Data.Length == 2)
            {
                string course = Data[1];
                this.repository.GetAllStudentsFromCourse(course);
            }
            else if (Data.Length == 3)
            {
                string course = Data[1];
                string user = Data[2];
                this.repository.GetStudentScoresFromCourse(course, user);
            }
            else
            {
                throw new InvalidCommandException(this.Input);

            }
        }
    }
}
