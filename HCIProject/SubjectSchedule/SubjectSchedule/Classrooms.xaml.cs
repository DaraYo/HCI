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
    /// Interaction logic for Classrooms.xaml
    /// </summary>
    public partial class Classrooms : Window, INotifyPropertyChanged
    {
        private MainWindow mainWindow;

        List<string> subjects = new List<string>();
        int i = 0;

        public void test()
        {
            var board = false;
            var projector = false;
            var smartBoard = false;
            String[] classrooms = File.ReadAllLines("../../classrooms.txt");
            this.DataContext = this;
            List<Classroom> l = new List<Classroom>();
            if (classrooms.Length != 0)
                foreach (var cr in classrooms)
                {
                    String[] parts = cr.Split(';');
                    Console.WriteLine(parts[0]);
                    Console.WriteLine(parts[1]);
                    Console.WriteLine(parts[2]);
                    Console.WriteLine(parts[3]);
                    Console.WriteLine(parts[4]);
                    Console.WriteLine(parts[5]);
                    //Subject s = new Subject();
                    if (parts[3].Equals("True"))
                        projector = true;
                    if (parts[4].Equals("True"))
                        board = true;
                    if (parts[5].Equals("True"))
                        smartBoard = true;
                    Console.WriteLine(projector);
                    Console.WriteLine(board);
                    Console.WriteLine(smartBoard);
                    l.Add(new Classroom
                    {
                        Label = parts[0],
                        Name = parts[1],
                        NumbOfSpots = int.Parse(parts[2]),
                        Projector = projector,
                        Board = board,
                        SmartBoard = smartBoard,
                        Softvare = parts[6],
                        Os = parts[7]

                    });
                    
                }
            Classr = new ObservableCollection<Classroom>(l);
            View = CollectionViewSource.GetDefaultView(Classr);
            GroupView = false;
        }

        public Classrooms(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            changeGrid.Visibility = Visibility.Hidden;
            test();
        }

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
        public ObservableCollection<Classroom> Classr
        {
            get;
            set;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            changeGrid.Visibility = Visibility.Visible;

        }

        //potvrda izmena
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String[] s;
            Classroom cr = (Classroom)dgSubs.SelectedItem;
            cr.Projector = (bool)projector.IsChecked;
            cr.Board = (bool)board.IsChecked;
            cr.SmartBoard = (bool)sBoard.IsChecked;
            cr.Os = osis.Text;
            cr.Softvare = soft.Text;
            List<string> lista = new List<string>();

            foreach (var su in Classr)
            {
                if (su.Label == cr.Label)
                {
                    su.Projector = cr.Projector;
                    su.Board = cr.Board;
                    su.SmartBoard = cr.SmartBoard;
                    su.Os = cr.Os;
                    su.Softvare = cr.Softvare;
                }
                lista.Add(su.Label + ";" + su.Name + ";" + su.NumbOfSpots + ";" + su.Projector + ";" + su.Board + ";" +
                    su.SmartBoard + ";" + su.Softvare+ ";" + su.Os);
            }
            File.WriteAllLines("../../classrooms.txt", lista);
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
            Classroom subj = (Classroom)dgSubs.SelectedItem;
            Classr.Remove(subj);
            List<string> lista = new List<string>();
            foreach (var su in Classr)
            {
                lista.Add(su.Label + ";" + su.Name + ";" + su.NumbOfSpots + ";" + su.Projector + ";" + su.Board + ";" +
                    su.SmartBoard + ";" + su.Softvare + ";" + su.Os);
            }
            File.WriteAllLines("../../classrooms.txt", lista);
        }

        //dodavanje predmeta u listu
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Classroom s = new Classroom();
            s.Label = a1.Text;
            s.Name = a2.Text;
            try
            {
                s.NumbOfSpots = int.Parse(a3.Text);
            }
            catch
            {
                s.NumbOfSpots = 0;
            }
            s.Projector = (bool)a4.IsChecked;
            s.Board = (bool)a5.IsChecked;
            s.SmartBoard = (bool)a6.IsChecked;
            s.Os = a8.Text;
            s.Softvare = a7.Text;
            if (s.Label != null && s.NumbOfSpots != 0)
                Classr.Add(s);
            List<string> lista = new List<string>();
            foreach (var su in Classr)
            {
                lista.Add(su.Label + ";" + su.Name + ";" + su.NumbOfSpots + ";" + su.Projector + ";" + su.Board + ";" +
                    su.SmartBoard + ";" + su.Softvare + ";" + su.Os);
            }
            File.WriteAllLines("../../classrooms.txt", lista);
        }
    }
}
