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
            this.mainWindow = mw;
            InitializeComponent();
            changeGrid.Visibility = Visibility.Hidden;
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
                        Course = parts[2],
                        Description = parts[3],
                        GroupSize = int.Parse(parts[4]),
                        PeriodNum = int.Parse(parts[5]),
                        Board = board,
                        Projector = projector,
                        SmartBoard = smartBoard,
                        Os = parts[9],
                        Softvare= parts[10]
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
                    
                }
            Subs = new ObservableCollection<Subject>(l);
            //dgSubs.Items.Refresh();
            View = CollectionViewSource.GetDefaultView(Subs);
            GroupView = false;
            Console.WriteLine(Subs[0]);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            changeGrid.Visibility = Visibility.Visible;

        }

        //potvrda izmena
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String[] s;
            Subject subj = (Subject)dgSubs.SelectedItem;
            subj.PeriodNum = int.Parse(periodNum.Text);
            subj.Projector = (bool)projector.IsChecked;
            subj.Board = (bool)board.IsChecked;
            subj.SmartBoard = (bool)sBoard.IsChecked;
            subj.Os = osis.Text;
            subj.Softvare = soft.Text;
            List<string> lista = new List<string>();

            foreach (var su in Subs)
            {
                if (su.Label == subj.Label)
                {
                    su.PeriodNum = subj.PeriodNum;
                    su.Projector = subj.Projector;
                    su.Board = subj.Board;
                    su.SmartBoard = subj.SmartBoard;
                    su.Os = subj.Os;
                    su.Softvare = subj.Softvare;
                }
                lista.Add(su.Label + ";" + su.Name + ";" + su.Course + ";" + su.Description + ";" + su.GroupSize + ";" +
                    su.PeriodNum + ";" + su.Projector + ";" + su.Board + ";" + su.SmartBoard + ";" + su.Os + ";" + su.Softvare);
            }
            File.WriteAllLines("../../Subjects.txt", lista);
            dgSubs.Items.Refresh();
            changeGrid.Visibility = Visibility.Hidden;

        }

        //otkazivanje izmena
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            changeGrid.Visibility = Visibility.Hidden;
        }

        //brisanje predmeta iz liste
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Subject subj = (Subject)dgSubs.SelectedItem;
            Subs.Remove(subj);
            List<string> lista = new List<string>();
            foreach (var su in Subs)
            {
                lista.Add(su.Label + ";" + su.Name + ";" + su.Course + ";" + su.Description + ";" + su.GroupSize + ";" +
                    su.PeriodNum + ";" + su.Projector + ";" + su.Board + ";" + su.SmartBoard + ";" + su.Os + ";" + su.Softvare);
            }
            File.WriteAllLines("../../Subjects.txt", lista);
        }

        //dodavanje predmeta u listu
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Subject s = new Subject();
            s.Label = a1.Text;
            s.Name = a2.Text;
            s.Course = a3.Text;
            s.Description = a4.Text;
            try
            {
                s.GroupSize = int.Parse(a5.Text);
                s.PeriodNum = int.Parse(a6.Text);
            }
            catch
            {
                s.GroupSize = 0;
                s.PeriodNum = 0;
            }
            s.Projector = (bool)a7.IsChecked;
            s.Board = (bool)a8.IsChecked;
            s.SmartBoard = (bool)a9.IsChecked;
            s.Os = a10.Text;
            s.Softvare = a11.Text;
            if (s.Label != null && s.GroupSize != 0 && s.PeriodNum != 0)
                Subs.Add(s);
            List<string> lista = new List<string>();
            foreach (var su in Subs)
            {
                lista.Add(su.Label + ";" + su.Name + ";" + su.Course + ";" + su.Description + ";" + su.GroupSize + ";" +
                    su.PeriodNum + ";" + su.Projector + ";" + su.Board + ";" + su.SmartBoard + ";" + su.Os + ";" + su.Softvare);
            }
            File.WriteAllLines("../../Subjects.txt", lista);
        }
    }
}
