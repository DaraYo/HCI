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
            set {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private Course course;

        public Course Course
        {
            get { return course; }
            set {
                if (value != course)
                {
                    course = value;
                    OnPropertyChanged("Course");
                }
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private int groupSize;

        public int GroupSize
        {
            get { return groupSize; }
            set
            {
                if (value != groupSize)
                {
                    groupSize = value;
                    OnPropertyChanged("GroupSize");
                }
            }
        }

        private int periodNum;

        public int PeriodNum
        {
            get { return periodNum; }
            set {
                if (value != periodNum)
                {
                    periodNum = value;
                    OnPropertyChanged("PeriodNum");
                }
            }
        }

        private bool projector;

        public bool Projector
        {
            get { return projector; }
            set {
                if (value != projector)
                {
                    projector = value;
                    OnPropertyChanged("Projector");
                }
            }
        }

        private bool board;

        public bool Board
        {
            get { return board; }
            set {
                if (value != board)
                {
                    board = value;
                    OnPropertyChanged("Board");
                }
            }
        }

        private bool smartBoard;

        public bool SmartBoard
        {
            get { return smartBoard; }
            set {
                if (value != smartBoard)
                {
                    smartBoard = value;
                    OnPropertyChanged("SmartBoard");
                }
            }
        }

        private string os;

        public string Os //moze biti w, l ili svejedno
        {
            get { return os; }
            set {
                if (value != os)
                {
                    os = value;
                    OnPropertyChanged("Os");
                }
            }
        }

        private Softvare softvare;

        public Softvare Softvare
        {
            get { return softvare; }
            set {
                if (value != softvare)
                {
                    softvare = value;
                    OnPropertyChanged("Softvare");
                }
            }
        }

        public Subject() { }





    }
}
