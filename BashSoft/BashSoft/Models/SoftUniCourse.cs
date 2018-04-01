using BashSoft.Contracts;
using BashSoft.Exceptions;
using System.Collections.Generic;

namespace BashSoft.Models
{
    public class SoftUniCourse: ICourse
    {
        public const int NumberOfTasksOnExam = 5;
        public const int MaxScoreOnExamTask = 100;

        private string name;
        private Dictionary<string, IStudent> studentsByName;

        public string Name
        {
            get { return this.name; }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {

                    throw new InvalidStringException();
                }

                this.name = value;
            }
        }

        public IReadOnlyDictionary<string, IStudent> StudentsByName
        {
            get
            {
                return this.studentsByName;
            }
        }

        public SoftUniCourse (string name)
        {
            this.Name = name;
            this.studentsByName = new Dictionary<string, IStudent>();
        }

        public void EnrollStudent(IStudent student)
        {
            if (this.studentsByName.ContainsKey(student.Username))
            {
                throw new DuplicateEntryInStructureException(student.Username,this.Name);
            }

            this.studentsByName.Add(student.Username, student);
        }

        public int CompareTo(ICourse other)
        {
            return this.Name.CompareTo(other.Name);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
