using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class Subjests : Window, INotifyPropertyChanged
    {
        private MainWindow mainWindow;

        List<string> subjects = new List<string>();
        int i = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private ICollectionView _View;
        public ICollectionView View
        {
            get
            {
                return _View;
            }
            set
            {
                _View = value;
                OnPropertyChanged("View");
            }
        }

        private bool _GroupView;
        public bool GroupView
        {
            get
            {
                return _GroupView;
            }
            set
            {
                if (value != _GroupView && View != null)
                {
                    View.GroupDescriptions.Clear();
                    if (value)
                    {
                        View.GroupDescriptions.Add(new PropertyGroupDescription("Upisan"));
                    }
                    _GroupView = value;
                    OnPropertyChanged("GroupView");

                    OnPropertyChanged("View");
                }
            }
        }
        public ObservableCollection<Subject> Subs
        {
            get;
            set;
        }

        public Subjests(MainWindow mw)
        {
            InitializeComponent();
            //Subs = new ObservableCollection<Subject>();
            test();
        }
        public void test()
        {
            var board= false;
            var projector = false;
            var smartBoard = false;
            String[] subjects = File.ReadAllLines("../../Subjects.txt");
            this.DataContext = this;
            List<Subject> l = new List<Subject>();
            if (subjects.Length != 0)
                foreach (var su in subjects)
                {
                    String[] parts = su.Split(';');
                    //Subject s = new Subject();
                    if (parts[6].Equals("true"))
                        board = true;
                    if (parts[7].Equals("true"))
                        projector = true;
                    if (parts[8].Equals("true"))
                        smartBoard = true;
                    l.Add(new Subject
                    {
                        Label = parts[0],
                        Name = parts[1],
                        Description = parts[3],
                        GroupSize = int.Parse(parts[4]),
                        PeriodNum = int.Parse(parts[5]),
                        Board = board,
                        Projector = projector,
                        SmartBoard = smartBoard,
                        Os = parts[9]
                    });
                    /*s.Label = parts[0];
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
                    s.Os = parts[9];*/
                    //Subs.Add(s); //new Subject { Name = "Test" }
                    Subs = new ObservableCollection<Subject>(l);
                }
            //dgSubs.Items.Refresh();
            View = CollectionViewSource.GetDefaultView(Subs);
            GroupView = false;
            Console.WriteLine(Subs[0]);
        }
    }
}
