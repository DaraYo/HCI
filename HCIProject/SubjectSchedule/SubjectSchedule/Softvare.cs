using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectSchedule
{
    class Softvare
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

        private string os;

        public string Os //operativni sistem moze biti w, l ili cs (windows, linux or cross-platform)
        {
            get { return os; }
            set { os = value; }
        }

        private string producer;

        public string Producer
        {
            get { return producer; }
            set { producer = value; }
        }

        private string site;

        public string Site
        {
            get { return site; }
            set { site = value; }
        }

        private DateTime dateOfCreating;

        public DateTime DateOfCreating
        {
            get { return dateOfCreating; }
            set { dateOfCreating = value; }
        }

        private int price;

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Softvare() { }


        
    }
}
