using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SubjectSchedule
{
    /// <summary>
    /// Interaction logic for Subjests.xaml
    /// </summary>
    public partial class Subjests : Window
    {
        private MainWindow mainWindow;

        List<string> subjects = new List<string>();
        int i = 0;
        public ObservableCollection<Subject> Subs
        {
            get;
            set;
        }

        public Subjests(MainWindow mw)
        {
            InitializeComponent();
            Subs = new ObservableCollection<Subject>();
            test();
        }
        public void test()
        {
            String[] subjects = File.ReadAllLines("../../Subjects.txt");
            this.DataContext = this;
            if (subjects.Length != 0)
                foreach (var su in subjects)
                {
                    String[] parts = su.Split(';');
                    Subject s = new Subject();
                    s.Label = parts[0];
                    s.Name = parts[1];
                    //s.Course = parts[2];
                    s.Description = parts[3];
                    s.GroupSize = int.Parse(parts[4]);
                    s.PeriodNum = int.Parse(parts[5]);
                    Console.WriteLine(parts[5]);
                    Console.WriteLine(parts[1]);
                    Console.WriteLine("Something else");
                    if (parts[6].Equals("true"))
                        s.Board = true;
                    else
                        s.Board = false;
                    if (parts[7].Equals("true"))
                        s.Projector = true;
                    else
                        s.Projector = false;
                    if (parts[8].Equals("true"))
                        s.SmartBoard = true;
                    else
                        s.SmartBoard = false;
                    s.Os = parts[9];
                    Subs.Add(new Subject { Name = "Test" });
                }
        }
    }
}
