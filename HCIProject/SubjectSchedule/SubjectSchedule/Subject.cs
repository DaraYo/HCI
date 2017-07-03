using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectSchedule
{
    class Subject
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

        private Course course;

        public Course Course
        {
            get { return course; }
            set { course = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private int groupSize;

        public int GroupSize
        {
            get { return groupSize; }
            set { groupSize = value; }
        }

        private int periodNum;

        public int PeriodNum
        {
            get { return periodNum; }
            set { periodNum = value; }
        }

        private bool projector;

        public bool Projector
        {
            get { return projector; }
            set { projector = value; }
        }

        private bool board;

        public bool Board
        {
            get { return board; }
            set { board = value; }
        }

        private bool smartBoard;

        public bool SmartBoard
        {
            get { return smartBoard; }
            set { smartBoard = value; }
        }

        private string os;

        public string Os //moze biti w, l ili svejedno
        {
            get { return os; }
            set { os = value; }
        }

        private Softvare softvare;

        public Softvare Softvare
        {
            get { return softvare; }
            set { softvare = value; }
        }

        public Subject() { }





    }
}
