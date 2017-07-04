using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectSchedule
{
    public class Course
    {
        private string label;

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private DateTime dateOfCreating;

        public DateTime DateOfCreating
        {
            get { return dateOfCreating; }
            set { dateOfCreating = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private List<Subject> subjects;

        public List<Subject> Subjects
        {
            get { return subjects; }
            set { subjects = value; }
        }
        public Course() { }



    }
}
