using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SubjectSchedule
{
    public class Subject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string Label)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(Label));
            }
        }

        private string label;
        public string Label
        {
            get { return label; }
            set
            {
                if (value != label)
                {
                    label = value;
                    OnPropertyChanged("Label");
                }
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string course;

        public string Course
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

        private string softvare;

        public string Softvare
        {
            get { return softvare; }
            set { softvare = value; }
        }

        public Subject() { }





    }
}
