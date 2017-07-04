using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectSchedule
{
    public class Classroom
    {
        public Classroom() { }

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

        private int numbOfSpots;

        public int NumbOfSpots
        {
            get { return numbOfSpots; }
            set { numbOfSpots = value; }
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

        private Softvare softvare;

        public Softvare Softvare
        {
            get { return softvare; }
            set { softvare = value; }
        }

        private string os;

        public string Os
        {
            get { return os; }
            set { os = value; }
        }



    }
}
