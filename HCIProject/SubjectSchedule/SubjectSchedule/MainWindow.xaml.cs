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
        public String label;
        public ObservableCollection<Subject> ocs
        {
            get;
            set;
        }
        public ObservableCollection<String> termSubjs1
        {
            get;
            set;
        }
        public ObservableCollection<String> termSubjs2
        {
            get;
            set;
        }
        public ObservableCollection<String> termSubjs3
        {
            get;
            set;
        }
        public ObservableCollection<String> termSubjs4
        {
            get;
            set;
        }
        public ObservableCollection<String> termSubjs5
        {
            get;
            set;
        }
        public ObservableCollection<String> termSubjs6
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
            termSubjs1 = new ObservableCollection<String>();
            termSubjs2 = new ObservableCollection<String>();
            termSubjs3 = new ObservableCollection<String>();
            termSubjs4 = new ObservableCollection<String>();
            termSubjs5 = new ObservableCollection<String>();
            termSubjs6 = new ObservableCollection<String>();

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
            this.label = c.Label;
            loadClassroomSchedule();
            ClassroomInfo.Children.Clear();
            ClassRoomInfo(c);
            AppendSubjects(c);
        }
        public void loadClassroomSchedule()
        {
            termSubjs1.Clear();
            termSubjs2.Clear();
            termSubjs3.Clear();
            termSubjs4.Clear();
            termSubjs5.Clear();
            termSubjs6.Clear();
            termSubjs1.Add("Monday");
            termSubjs2.Add("Tuesday");
            termSubjs3.Add("Wednesday");
            termSubjs4.Add("Thursday");
            termSubjs5.Add("Friday");
            termSubjs6.Add("Saturday");
            String[] schedule = File.ReadAllLines("../../ClassroomSchedule/"+this.label+".txt");
            for (int i = 1; i<21;i++)
            {
                String[] parts = schedule[i-1].Split(';');
                var tb1 = (TextBlock)this.FindName("TB" + i + "1");
                var tb2 = (TextBlock)this.FindName("TB" + i + "2");
                var tb3 = (TextBlock)this.FindName("TB" + i + "3");
                var tb4 = (TextBlock)this.FindName("TB" + i + "4");
                var tb5 = (TextBlock)this.FindName("TB" + i + "5");
                var tb6 = (TextBlock)this.FindName("TB" + i + "6");

                termSubjs1.Add(parts[0]);
                termSubjs2.Add(parts[1]);
                termSubjs3.Add(parts[2]);
                termSubjs4.Add(parts[3]);
                termSubjs5.Add(parts[4]);
                termSubjs6.Add(parts[5]);
            }
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
                //if(g!=1)
                    ocs.Add(s);
            }
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

        private void ListView_Drop01(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[1] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop02(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[1] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop03(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[1] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop04(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[1] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop05(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[1] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop06(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[1] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop11(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[1] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop12(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[1] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop13(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[1] = student.Label;
                saveSchedule();
            }

        }
        private void ListView_Drop14(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[1] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop15(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[1] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop16(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[1] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop21(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[2] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop22(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[2] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop23(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[2] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop24(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[2] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop25(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[2] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop26(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[2] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop31(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[3] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop32(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[3] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop33(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[3] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop34(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[3] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop35(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[3] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop36(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[3] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop41(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[4] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop42(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[4] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop43(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[4] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop44(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[4] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop45(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[4] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop46(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[4] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop51(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[5] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop52(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[5] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop53(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[5] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop54(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[5] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop55(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[5] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop56(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[5] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop61(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[6] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop62(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[6] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop63(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[6] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop64(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[6] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop65(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[6] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop66(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[6] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop71(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[7] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop72(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[7] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop73(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[7] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop74(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[7] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop75(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[7] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop76(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[7] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop81(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[8] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop82(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[8] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop83(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[8] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop84(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[8] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop85(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[8] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop86(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[8] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop91(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[9] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop92(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[9] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop93(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[9] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop94(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[9] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop95(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[9] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop96(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[9] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop101(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[10] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop102(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[10] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop103(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[10] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop104(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[10] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop105(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[10] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop106(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[10] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop111(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[11] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop112(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[11] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop113(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[11] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop114(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[11] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop115(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[11] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop116(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[11] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop121(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[12] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop122(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[12] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop123(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[12] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop124(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[12] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop125(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[12] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop126(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[12] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop131(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[13] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop132(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[13] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop133(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[13] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop134(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[13] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop135(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[13] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop136(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[13] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop141(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[14] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop142(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[14] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop143(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[14] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop144(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[14] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop145(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[14] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop146(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[14] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop151(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[15] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop152(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[15] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop153(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[15] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop154(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[15] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop155(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[15] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop156(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[15] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop161(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[16] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop162(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[16] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop163(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[16] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop164(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[16] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop165(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[16] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop166(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[16] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop171(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[17] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop172(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[17] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop173(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[17] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop174(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[17] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop175(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[17] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop176(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[17] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop181(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[18] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop182(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[18] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop183(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[18] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop184(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[18] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop185(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[18] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop186(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[18] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop191(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[19] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop192(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[19] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop193(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[19] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop194(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[19] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop195(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[19] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop196(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[19] = student.Label;
                saveSchedule();
            }
        }

        private void ListView_Drop201(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs1[20] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop202(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs2[20] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop203(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs3[20] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop204(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs4[20] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop205(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs5[20] = student.Label;
                saveSchedule();
            }
        }
        private void ListView_Drop206(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Subject student = e.Data.GetData("myFormat") as Subject;
                ocs.Remove(student);
                termSubjs6[20] = student.Label;
                saveSchedule();
            }
        }
        private void saveSchedule()
        {
            List<string> lista = new List<string>();
            for(int i = 1; i<21;i++)
            {
                String tmp = termSubjs1[i] + ";" + termSubjs2[i] + ";" + termSubjs3[i] + ";" + termSubjs4[i] + ";" + termSubjs5[i] + ";" + termSubjs6[i];
                lista.Add(tmp);
            }
            File.WriteAllLines("../../ClassroomSchedule/"+this.label+".txt", lista);
        }
    }
}
