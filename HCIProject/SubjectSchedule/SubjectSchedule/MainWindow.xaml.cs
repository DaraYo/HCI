using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SubjectSchedule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Classroom> lc = new List<Classroom>();
        public ObservableCollection<Subject> ocs
        {
            get;
            set;
        }
        public ObservableCollection<Subject> termSubjs1
        {
            get;
            set;
        }
        public ObservableCollection<Subject> termSubjs2
        {
            get;
            set;
        }
        public ObservableCollection<Subject> termSubjs3
        {
            get;
            set;
        }
        public ObservableCollection<Subject> termSubjs4
        {
            get;
            set;
        }
        public ObservableCollection<Subject> termSubjs5
        {
            get;
            set;
        }
        public ObservableCollection<Subject> termSubjs6
        {
            get;
            set;
        }
        public ObservableCollection<String> termini
        {
            get;
            set;
        }
        List<Subject> ls = new List<Subject>();
        Point startPoint = new Point();
        public MainWindow()
        {
            InitializeComponent();
            listClassrooms();
            //loadTerms();
            ls.Add(new Subject { Label = "Testiranje-.-" });
            ocs = new ObservableCollection<Subject>();
            termSubjs1 = new ObservableCollection<Subject>();
            termSubjs2 = new ObservableCollection<Subject>();
            termSubjs3 = new ObservableCollection<Subject>();
            termSubjs4 = new ObservableCollection<Subject>();
            termSubjs5 = new ObservableCollection<Subject>();
            termSubjs6 = new ObservableCollection<Subject>();
            String[] terms = File.ReadAllLines("../../termini.txt");
            termini = new ObservableCollection<String>(terms);
        }
        public void loadTerms()
        {
            String[] terms = File.ReadAllLines("../../termini.txt");
            foreach(String t in terms)
                termini.Add(t);
        }
        public void listClassrooms()
        {
            String[] classrooms = File.ReadAllLines("../../classrooms.txt");
            
            foreach(var r in classrooms)
            {
                String[] info = r.Split(';');
                Classroom cr = new Classroom();
                cr.Label = info[0];
                cr.Name = info[1];
                cr.NumbOfSpots = int.Parse(info[2]);
                if (info[3].Equals("true"))
                {
                    cr.Projector = true;
                }
                else
                    cr.Projector = false;
                if(info[4].Equals("true"))
                {
                    cr.Board = true;
                }
                else
                {
                    cr.Board = false;
                }
                if (info[5].Equals("true"))
                {
                    cr.SmartBoard = true;
                }
                else
                    cr.SmartBoard = false;
                //cr.Softvare = info[6];
                cr.Os = info[7];
    
                this.lc.Add(cr);
            }

            List<Button> lb = new List<Button>();
            for(int i = 0 ; i < lc.Count ; i++ )
            {
                Button b = new Button();
                b.Width = ClassroomsList.Width;
                b.Height = 50;
                b.Content = lc[i].Label;
                b.Click += delegate
                 {
                     testFunction((String)b.Content);
                 };
                lb.Add(b);
                Canvas.SetLeft(lb[i], 0);
                Canvas.SetTop(lb[i], i*50);
                ClassroomCanvas.Children.Add(lb[i]);
                
            }
        }
        public void testFunction(String classRoom)
        {
            Classroom c = new Classroom();
            foreach(var cr in lc)
            {
                if (cr.Label.Equals(classRoom))
                    c = cr;
            }
            ClassroomInfo.Children.Clear();
            ClassRoomInfo(c);
            AppendSubjects(c);
        }
        public void ClassRoomInfo(Classroom c)
        {
            TextBox tb = new TextBox();
            tb.Text = c.Label + " " + c.Name;
            tb.Background = Brushes.Transparent;
            ClassroomInfo.Children.Add(tb);
        }
        public void AppendSubjects(Classroom c)
        {
            ocs.Clear();
            this.DataContext = this;
            ls.Clear();
            String[] subjects = File.ReadAllLines("../../Subjects.txt");
            foreach(var su in subjects)
            {
                String[] parts = su.Split(';');
                Subject s = new Subject();
                s.Label = parts[0];
                s.Name = parts[1];
                //s.Course = parts[2];
                s.Description = parts[3];
                s.GroupSize = int.Parse(parts[4]);
                s.PeriodNum = int.Parse(parts[5]);
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
                //s.Softvare = parts[10];

                //veliki uslov -.-
                int g = 0;
                if (s.Os != c.Os)
                    g = 1;
                if (s.Board == true && c.Board == false)
                    g = 1;
                if (s.SmartBoard == true && c.SmartBoard == false)
                    g = 1;
                if (s.Projector == true && c.Projector == false)
                    g = 1;
                if (s.GroupSize > c.NumbOfSpots)
                    g = 1;
                if(g!=1)
                    ocs.Add(s);
            }
            /*
            for (int i = 0; i < ls.Count; i++)
            {
                Button b = new Button();
                b.Width = ClassroomsList.Width;
                b.Height = 50;
                b.Content = ls[i].Label;
                Canvas.SetLeft(b, 0);
                Canvas.SetTop(b, i * 50);
                SubjectsCanvas.Children.Add(b);

            }*/

        }

        //NEKI KOD KOJI SAM KOPIRAO ZA DRAG AND DROP -.-
        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                // Find the data behind the ListViewItem
                Subject student = (Subject)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", student);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
        }
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void ListView_Drop1(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1.Add(student);
            }
        }
        private void ListView_Drop2(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2.Add(student);
            }
        }
        private void ListView_Drop3(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3.Add(student);
            }
        }
        private void ListView_Drop4(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4.Add(student);
            }
        }
        private void ListView_Drop5(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5.Add(student);
            }
        }

        private void ListView_Drop6(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6.Add(student);
            }
        }
    }
}
