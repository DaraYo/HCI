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

        public ObservableCollection<String> crInfo
        {
            get;
            set;
        }
        List<Subject> ls = new List<Subject>();
        Point startPoint = new Point();
        public MainWindow()
        {
            InitializeComponent();
            CommandBinding helpBinding = new CommandBinding(ApplicationCommands.Help);
            helpBinding.CanExecute += CanHelpExecute;
            helpBinding.Executed += HelpExecuted;
            CommandBindings.Add(helpBinding);
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
            crInfo = new ObservableCollection<string>();
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
                cr.Softvare = info[6];
                cr.Os = info[7];
    
                this.lc.Add(cr);
            }

            List<Button> lb = new List<Button>();
            for(int i = 0 ; i < lc.Count ; i++ )
            {
                Button b = new Button();
                b.Width = ClassroomsList.Width-10;
                b.Height = 45;
                b.Content = lc[i].Label;
                
                b.Click += delegate
                 {
                     testFunction((String)b.Content);
                 };
                lb.Add(b);
                Canvas.SetLeft(lb[i], 5);
                Canvas.SetTop(lb[i], i*50+5);
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
            crInfo.Clear();
            crInfo.Add("Label: " + c.Label);
            crInfo.Add("Name: " + c.Name);
            crInfo.Add("Spots: " + c.NumbOfSpots);
            if (c.Projector == true)
                crInfo.Add("Projector: Yes");
            else
                crInfo.Add("Projector: No");

            if (c.Board == true)
                crInfo.Add("Board: Yes");
            else
                crInfo.Add("Board: No");

            if (c.SmartBoard == true)
                crInfo.Add("Projector: Yes");
            else
                crInfo.Add("Projector: No");

            crInfo.Add("Software: " + c.Softvare);

            crInfo.Add("OS: " + c.Os);


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

        //#############################################################################################
        private void Go_to_subjects(object sender, RoutedEventArgs e)
        {
            var allSubjects = new Subjests(this);
            allSubjects.Show();
        }
        private void TextBlock_MouseEnter11(object sender, MouseEventArgs e)
        {
            TB11.Background = Brushes.DarkSeaGreen;
            TB10.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB11.FontSize = 25;
            TB10.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave11(object sender, MouseEventArgs e)
        {
            TB11.Background = Brushes.Transparent;
            TB10.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB11.FontSize = 12;
            TB10.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter12(object sender, MouseEventArgs e)
        {
            TB12.Background = Brushes.DarkSeaGreen;
            TB10.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB12.FontSize = 25;
            TB10.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave12(object sender, MouseEventArgs e)
        {
            TB12.Background = Brushes.Transparent;
            TB10.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB12.FontSize = 12;
            TB10.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter13(object sender, MouseEventArgs e)
        {
            TB13.Background = Brushes.DarkSeaGreen;
            TB10.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB13.FontSize = 25;
            TB10.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave13(object sender, MouseEventArgs e)
        {
            TB13.Background = Brushes.Transparent;
            TB10.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB13.FontSize = 12;
            TB10.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter14(object sender, MouseEventArgs e)
        {
            TB14.Background = Brushes.DarkSeaGreen;
            TB10.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB14.FontSize = 25;
            TB10.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave14(object sender, MouseEventArgs e)
        {
            TB14.Background = Brushes.Transparent;
            TB10.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB14.FontSize = 12;
            TB10.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter15(object sender, MouseEventArgs e)
        {
            TB15.Background = Brushes.DarkSeaGreen;
            TB10.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB15.FontSize = 25;
            TB10.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave15(object sender, MouseEventArgs e)
        {
            TB15.Background = Brushes.Transparent;
            TB10.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB15.FontSize = 12;
            TB10.FontSize = 12;
            FridayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter16(object sender, MouseEventArgs e)
        {
            TB16.Background = Brushes.DarkSeaGreen;
            TB10.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB16.FontSize = 25;
            TB10.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave16(object sender, MouseEventArgs e)
        {
            TB16.Background = Brushes.Transparent;
            TB10.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB16.FontSize = 12;
            TB10.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter21(object sender, MouseEventArgs e)
        {
            TB21.Background = Brushes.DarkSeaGreen;
            TB20.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB21.FontSize = 25;
            TB20.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave21(object sender, MouseEventArgs e)
        {
            TB21.Background = Brushes.Transparent;
            TB20.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB21.FontSize = 12;
            TB20.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter22(object sender, MouseEventArgs e)
        {
            TB22.Background = Brushes.DarkSeaGreen;
            TB20.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB22.FontSize = 25;
            TB20.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave22(object sender, MouseEventArgs e)
        {
            TB22.Background = Brushes.Transparent;
            TB20.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB22.FontSize = 12;
            TB20.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }
        private void TextBlock_MouseEnter23(object sender, MouseEventArgs e)
        {
            TB23.Background = Brushes.DarkSeaGreen;
            TB20.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB23.FontSize = 25;
            TB20.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave23(object sender, MouseEventArgs e)
        {
            TB23.Background = Brushes.Transparent;
            TB20.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB23.FontSize = 12;
            TB20.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter24(object sender, MouseEventArgs e)
        {
            TB24.Background = Brushes.DarkSeaGreen;
            TB20.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB24.FontSize = 25;
            TB20.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave24(object sender, MouseEventArgs e)
        {
            TB24.Background = Brushes.Transparent;
            TB20.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB24.FontSize = 12;
            TB20.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter25(object sender, MouseEventArgs e)
        {
            TB25.Background = Brushes.DarkSeaGreen;
            TB20.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB25.FontSize = 25;
            TB20.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave25(object sender, MouseEventArgs e)
        {
            TB25.Background = Brushes.Transparent;
            TB20.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB25.FontSize = 12;
            TB20.FontSize = 12;
            FridayTB.FontSize = 12;
        }


        private void TextBlock_MouseEnter26(object sender, MouseEventArgs e)
        {
            TB26.Background = Brushes.DarkSeaGreen;
            TB20.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB26.FontSize = 25;
            TB20.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave26(object sender, MouseEventArgs e)
        {
            TB26.Background = Brushes.Transparent;
            TB20.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB26.FontSize = 12;
            TB20.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }
        private void TextBlock_MouseEnter31(object sender, MouseEventArgs e)
        {
            TB31.Background = Brushes.DarkSeaGreen;
            TB30.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB31.FontSize = 25;
            TB30.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave31(object sender, MouseEventArgs e)
        {
            TB31.Background = Brushes.Transparent;
            TB30.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB31.FontSize = 12;
            TB30.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter32(object sender, MouseEventArgs e)
        {
            TB32.Background = Brushes.DarkSeaGreen;
            TB30.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB32.FontSize = 25;
            TB30.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave32(object sender, MouseEventArgs e)
        {
            TB32.Background = Brushes.Transparent;
            TB30.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB32.FontSize = 12;
            TB30.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }
        private void TextBlock_MouseEnter33(object sender, MouseEventArgs e)
        {
            TB33.Background = Brushes.DarkSeaGreen;
            TB30.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB33.FontSize = 25;
            TB30.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave33(object sender, MouseEventArgs e)
        {
            TB33.Background = Brushes.Transparent;
            TB30.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB33.FontSize = 12;
            TB30.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter34(object sender, MouseEventArgs e)
        {
            TB34.Background = Brushes.DarkSeaGreen;
            TB30.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB34.FontSize = 25;
            TB30.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave34(object sender, MouseEventArgs e)
        {
            TB34.Background = Brushes.Transparent;
            TB30.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB34.FontSize = 12;
            TB30.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter35(object sender, MouseEventArgs e)
        {
            TB35.Background = Brushes.DarkSeaGreen;
            TB30.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB35.FontSize = 25;
            TB30.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave35(object sender, MouseEventArgs e)
        {
            TB35.Background = Brushes.Transparent;
            TB30.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB35.FontSize = 12;
            TB30.FontSize = 12;
            FridayTB.FontSize = 12;
        }


        private void TextBlock_MouseEnter36(object sender, MouseEventArgs e)
        {
            TB36.Background = Brushes.DarkSeaGreen;
            TB30.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB36.FontSize = 25;
            TB30.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave36(object sender, MouseEventArgs e)
        {
            TB36.Background = Brushes.Transparent;
            TB30.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB36.FontSize = 12;
            TB30.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter41(object sender, MouseEventArgs e)
        {
            TB41.Background = Brushes.DarkSeaGreen;
            TB40.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB41.FontSize = 25;
            TB40.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave41(object sender, MouseEventArgs e)
        {
            TB41.Background = Brushes.Transparent;
            TB40.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB41.FontSize = 12;
            TB40.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter42(object sender, MouseEventArgs e)
        {
            TB42.Background = Brushes.DarkSeaGreen;
            TB40.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB42.FontSize = 25;
            TB40.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave42(object sender, MouseEventArgs e)
        {
            TB42.Background = Brushes.Transparent;
            TB40.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB42.FontSize = 12;
            TB40.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }
        private void TextBlock_MouseEnter43(object sender, MouseEventArgs e)
        {
            TB43.Background = Brushes.DarkSeaGreen;
            TB40.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB43.FontSize = 25;
            TB40.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave43(object sender, MouseEventArgs e)
        {
            TB43.Background = Brushes.Transparent;
            TB40.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB43.FontSize = 12;
            TB40.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter44(object sender, MouseEventArgs e)
        {
            TB44.Background = Brushes.DarkSeaGreen;
            TB40.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB44.FontSize = 25;
            TB40.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave44(object sender, MouseEventArgs e)
        {
            TB44.Background = Brushes.Transparent;
            TB40.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB44.FontSize = 12;
            TB40.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter45(object sender, MouseEventArgs e)
        {
            TB45.Background = Brushes.DarkSeaGreen;
            TB40.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB45.FontSize = 25;
            TB40.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave45(object sender, MouseEventArgs e)
        {
            TB45.Background = Brushes.Transparent;
            TB40.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB45.FontSize = 12;
            TB40.FontSize = 12;
            FridayTB.FontSize = 12;
        }


        private void TextBlock_MouseEnter46(object sender, MouseEventArgs e)
        {
            TB46.Background = Brushes.DarkSeaGreen;
            TB40.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB46.FontSize = 25;
            TB40.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave46(object sender, MouseEventArgs e)
        {
            TB46.Background = Brushes.Transparent;
            TB40.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB46.FontSize = 12;
            TB40.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }
        private void TextBlock_MouseEnter51(object sender, MouseEventArgs e)
        {
            TB51.Background = Brushes.DarkSeaGreen;
            TB50.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB51.FontSize = 25;
            TB50.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave51(object sender, MouseEventArgs e)
        {
            TB51.Background = Brushes.Transparent;
            TB50.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB51.FontSize = 12;
            TB50.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter52(object sender, MouseEventArgs e)
        {
            TB52.Background = Brushes.DarkSeaGreen;
            TB50.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB52.FontSize = 25;
            TB50.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave52(object sender, MouseEventArgs e)
        {
            TB52.Background = Brushes.Transparent;
            TB50.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB52.FontSize = 12;
            TB50.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }
        private void TextBlock_MouseEnter53(object sender, MouseEventArgs e)
        {
            TB53.Background = Brushes.DarkSeaGreen;
            TB50.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB53.FontSize = 25;
            TB50.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave53(object sender, MouseEventArgs e)
        {
            TB53.Background = Brushes.Transparent;
            TB50.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB53.FontSize = 12;
            TB50.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter54(object sender, MouseEventArgs e)
        {
            TB54.Background = Brushes.DarkSeaGreen;
            TB50.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB54.FontSize = 25;
            TB50.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave54(object sender, MouseEventArgs e)
        {
            TB54.Background = Brushes.Transparent;
            TB50.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB54.FontSize = 12;
            TB50.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter55(object sender, MouseEventArgs e)
        {
            TB55.Background = Brushes.DarkSeaGreen;
            TB50.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB55.FontSize = 25;
            TB50.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave55(object sender, MouseEventArgs e)
        {
            TB55.Background = Brushes.Transparent;
            TB50.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB55.FontSize = 12;
            TB50.FontSize = 12;
            FridayTB.FontSize = 12;
        }


        private void TextBlock_MouseEnter56(object sender, MouseEventArgs e)
        {
            TB56.Background = Brushes.DarkSeaGreen;
            TB50.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB56.FontSize = 25;
            TB50.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave56(object sender, MouseEventArgs e)
        {
            TB56.Background = Brushes.Transparent;
            TB50.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB56.FontSize = 12;
            TB50.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter61(object sender, MouseEventArgs e)
        {
            TB61.Background = Brushes.DarkSeaGreen;
            TB60.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB61.FontSize = 25;
            TB60.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave61(object sender, MouseEventArgs e)
        {
            TB61.Background = Brushes.Transparent;
            TB60.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB61.FontSize = 12;
            TB60.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter62(object sender, MouseEventArgs e)
        {
            TB62.Background = Brushes.DarkSeaGreen;
            TB60.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB62.FontSize = 25;
            TB60.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave62(object sender, MouseEventArgs e)
        {
            TB62.Background = Brushes.Transparent;
            TB60.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB62.FontSize = 12;
            TB60.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }
        private void TextBlock_MouseEnter63(object sender, MouseEventArgs e)
        {
            TB63.Background = Brushes.DarkSeaGreen;
            TB60.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB63.FontSize = 25;
            TB60.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave63(object sender, MouseEventArgs e)
        {
            TB63.Background = Brushes.Transparent;
            TB60.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB63.FontSize = 12;
            TB60.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter64(object sender, MouseEventArgs e)
        {
            TB64.Background = Brushes.DarkSeaGreen;
            TB60.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB64.FontSize = 25;
            TB60.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave64(object sender, MouseEventArgs e)
        {
            TB64.Background = Brushes.Transparent;
            TB60.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB64.FontSize = 12;
            TB60.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter65(object sender, MouseEventArgs e)
        {
            TB65.Background = Brushes.DarkSeaGreen;
            TB60.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB65.FontSize = 25;
            TB60.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave65(object sender, MouseEventArgs e)
        {
            TB65.Background = Brushes.Transparent;
            TB60.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB65.FontSize = 12;
            TB60.FontSize = 12;
            FridayTB.FontSize = 12;
        }


        private void TextBlock_MouseEnter66(object sender, MouseEventArgs e)
        {
            TB66.Background = Brushes.DarkSeaGreen;
            TB60.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB66.FontSize = 25;
            TB60.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave66(object sender, MouseEventArgs e)
        {
            TB66.Background = Brushes.Transparent;
            TB60.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB66.FontSize = 12;
            TB60.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }
        private void TextBlock_MouseEnter71(object sender, MouseEventArgs e)
        {
            TB71.Background = Brushes.DarkSeaGreen;
            TB70.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB71.FontSize = 25;
            TB70.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave71(object sender, MouseEventArgs e)
        {
            TB71.Background = Brushes.Transparent;
            TB70.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB71.FontSize = 12;
            TB70.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter72(object sender, MouseEventArgs e)
        {
            TB72.Background = Brushes.DarkSeaGreen;
            TB70.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB72.FontSize = 25;
            TB70.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave72(object sender, MouseEventArgs e)
        {
            TB72.Background = Brushes.Transparent;
            TB70.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB72.FontSize = 12;
            TB70.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }
        private void TextBlock_MouseEnter73(object sender, MouseEventArgs e)
        {
            TB73.Background = Brushes.DarkSeaGreen;
            TB70.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB73.FontSize = 25;
            TB70.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave73(object sender, MouseEventArgs e)
        {
            TB73.Background = Brushes.Transparent;
            TB70.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB73.FontSize = 12;
            TB70.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter74(object sender, MouseEventArgs e)
        {
            TB74.Background = Brushes.DarkSeaGreen;
            TB70.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB74.FontSize = 25;
            TB70.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave74(object sender, MouseEventArgs e)
        {
            TB74.Background = Brushes.Transparent;
            TB70.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB74.FontSize = 12;
            TB70.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter75(object sender, MouseEventArgs e)
        {
            TB75.Background = Brushes.DarkSeaGreen;
            TB70.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB75.FontSize = 25;
            TB70.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave75(object sender, MouseEventArgs e)
        {
            TB75.Background = Brushes.Transparent;
            TB70.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB75.FontSize = 12;
            TB70.FontSize = 12;
            FridayTB.FontSize = 12;
        }


        private void TextBlock_MouseEnter76(object sender, MouseEventArgs e)
        {
            TB76.Background = Brushes.DarkSeaGreen;
            TB70.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB76.FontSize = 25;
            TB70.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave76(object sender, MouseEventArgs e)
        {
            TB76.Background = Brushes.Transparent;
            TB70.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB76.FontSize = 12;
            TB70.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }
        //


        private void TextBlock_MouseEnter81(object sender, MouseEventArgs e)
        {
            TB81.Background = Brushes.DarkSeaGreen;
            TB80.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB81.FontSize = 25;
            TB80.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave81(object sender, MouseEventArgs e)
        {
            TB81.Background = Brushes.Transparent;
            TB80.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB81.FontSize = 12;
            TB80.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter82(object sender, MouseEventArgs e)
        {
            TB82.Background = Brushes.DarkSeaGreen;
            TB80.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB82.FontSize = 25;
            TB80.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave82(object sender, MouseEventArgs e)
        {
            TB82.Background = Brushes.Transparent;
            TB80.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB82.FontSize = 12;
            TB80.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }
        private void TextBlock_MouseEnter83(object sender, MouseEventArgs e)
        {
            TB83.Background = Brushes.DarkSeaGreen;
            TB80.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB83.FontSize = 25;
            TB80.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave83(object sender, MouseEventArgs e)
        {
            TB83.Background = Brushes.Transparent;
            TB80.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB83.FontSize = 12;
            TB80.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter84(object sender, MouseEventArgs e)
        {
            TB84.Background = Brushes.DarkSeaGreen;
            TB80.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB84.FontSize = 25;
            TB80.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave84(object sender, MouseEventArgs e)
        {
            TB84.Background = Brushes.Transparent;
            TB80.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB84.FontSize = 12;
            TB80.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter85(object sender, MouseEventArgs e)
        {
            TB85.Background = Brushes.DarkSeaGreen;
            TB80.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB85.FontSize = 25;
            TB80.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave85(object sender, MouseEventArgs e)
        {
            TB85.Background = Brushes.Transparent;
            TB80.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB85.FontSize = 12;
            TB80.FontSize = 12;
            FridayTB.FontSize = 12;
        }


        private void TextBlock_MouseEnter86(object sender, MouseEventArgs e)
        {
            TB86.Background = Brushes.DarkSeaGreen;
            TB80.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB86.FontSize = 25;
            TB80.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave86(object sender, MouseEventArgs e)
        {
            TB86.Background = Brushes.Transparent;
            TB80.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB86.FontSize = 12;
            TB80.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }
        private void TextBlock_MouseEnter91(object sender, MouseEventArgs e)
        {
            TB91.Background = Brushes.DarkSeaGreen;
            TB90.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB91.FontSize = 25;
            TB90.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave91(object sender, MouseEventArgs e)
        {
            TB91.Background = Brushes.Transparent;
            TB90.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB91.FontSize = 12;
            TB90.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter92(object sender, MouseEventArgs e)
        {
            TB92.Background = Brushes.DarkSeaGreen;
            TB90.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB92.FontSize = 25;
            TB90.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave92(object sender, MouseEventArgs e)
        {
            TB92.Background = Brushes.Transparent;
            TB90.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB92.FontSize = 12;
            TB90.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }
        private void TextBlock_MouseEnter93(object sender, MouseEventArgs e)
        {
            TB93.Background = Brushes.DarkSeaGreen;
            TB90.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB93.FontSize = 25;
            TB90.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave93(object sender, MouseEventArgs e)
        {
            TB93.Background = Brushes.Transparent;
            TB90.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB93.FontSize = 12;
            TB90.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter94(object sender, MouseEventArgs e)
        {
            TB94.Background = Brushes.DarkSeaGreen;
            TB90.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB94.FontSize = 25;
            TB90.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave94(object sender, MouseEventArgs e)
        {
            TB94.Background = Brushes.Transparent;
            TB90.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB94.FontSize = 12;
            TB90.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter95(object sender, MouseEventArgs e)
        {
            TB95.Background = Brushes.DarkSeaGreen;
            TB90.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB95.FontSize = 25;
            TB90.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave95(object sender, MouseEventArgs e)
        {
            TB95.Background = Brushes.Transparent;
            TB90.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB95.FontSize = 12;
            TB90.FontSize = 12;
            FridayTB.FontSize = 12;
        }


        private void TextBlock_MouseEnter96(object sender, MouseEventArgs e)
        {
            TB96.Background = Brushes.DarkSeaGreen;
            TB90.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB96.FontSize = 25;
            TB90.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave96(object sender, MouseEventArgs e)
        {
            TB96.Background = Brushes.Transparent;
            TB90.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB96.FontSize = 12;
            TB90.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter101(object sender, MouseEventArgs e)
        {
            TB101.Background = Brushes.DarkSeaGreen;
            TB100.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB101.FontSize = 25;
            TB100.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave101(object sender, MouseEventArgs e)
        {
            TB101.Background = Brushes.Transparent;
            TB100.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB101.FontSize = 12;
            TB100.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter102(object sender, MouseEventArgs e)
        {
            TB102.Background = Brushes.DarkSeaGreen;
            TB100.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB102.FontSize = 25;
            TB100.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave102(object sender, MouseEventArgs e)
        {
            TB102.Background = Brushes.Transparent;
            TB100.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB102.FontSize = 12;
            TB100.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter103(object sender, MouseEventArgs e)
        {
            TB103.Background = Brushes.DarkSeaGreen;
            TB100.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB103.FontSize = 25;
            TB100.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave103(object sender, MouseEventArgs e)
        {
            TB103.Background = Brushes.Transparent;
            TB100.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB103.FontSize = 12;
            TB100.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter104(object sender, MouseEventArgs e)
        {
            TB104.Background = Brushes.DarkSeaGreen;
            TB100.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB104.FontSize = 25;
            TB100.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave104(object sender, MouseEventArgs e)
        {
            TB104.Background = Brushes.Transparent;
            TB100.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB104.FontSize = 12;
            TB100.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter105(object sender, MouseEventArgs e)
        {
            TB105.Background = Brushes.DarkSeaGreen;
            TB100.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB105.FontSize = 25;
            TB100.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave105(object sender, MouseEventArgs e)
        {
            TB105.Background = Brushes.Transparent;
            TB100.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB105.FontSize = 12;
            TB100.FontSize = 12;
            FridayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter106(object sender, MouseEventArgs e)
        {
            TB106.Background = Brushes.DarkSeaGreen;
            TB100.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB106.FontSize = 25;
            TB100.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave106(object sender, MouseEventArgs e)
        {
            TB106.Background = Brushes.Transparent;
            TB100.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB106.FontSize = 12;
            TB100.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter111(object sender, MouseEventArgs e)
        {
            TB111.Background = Brushes.DarkSeaGreen;
            TB110.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB111.FontSize = 25;
            TB110.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave111(object sender, MouseEventArgs e)
        {
            TB111.Background = Brushes.Transparent;
            TB110.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB111.FontSize = 12;
            TB110.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter112(object sender, MouseEventArgs e)
        {
            TB112.Background = Brushes.DarkSeaGreen;
            TB110.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB112.FontSize = 25;
            TB110.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave112(object sender, MouseEventArgs e)
        {
            TB112.Background = Brushes.Transparent;
            TB110.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB112.FontSize = 12;
            TB110.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter113(object sender, MouseEventArgs e)
        {
            TB113.Background = Brushes.DarkSeaGreen;
            TB110.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB113.FontSize = 25;
            TB110.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave113(object sender, MouseEventArgs e)
        {
            TB113.Background = Brushes.Transparent;
            TB110.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB113.FontSize = 12;
            TB110.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter114(object sender, MouseEventArgs e)
        {
            TB114.Background = Brushes.DarkSeaGreen;
            TB110.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB114.FontSize = 25;
            TB110.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave114(object sender, MouseEventArgs e)
        {
            TB114.Background = Brushes.Transparent;
            TB110.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB114.FontSize = 12;
            TB110.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter115(object sender, MouseEventArgs e)
        {
            TB115.Background = Brushes.DarkSeaGreen;
            TB110.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB115.FontSize = 25;
            TB110.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave115(object sender, MouseEventArgs e)
        {
            TB115.Background = Brushes.Transparent;
            TB110.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB115.FontSize = 12;
            TB110.FontSize = 12;
            FridayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter116(object sender, MouseEventArgs e)
        {
            TB116.Background = Brushes.DarkSeaGreen;
            TB110.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB116.FontSize = 25;
            TB110.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave116(object sender, MouseEventArgs e)
        {
            TB116.Background = Brushes.Transparent;
            TB110.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB116.FontSize = 12;
            TB110.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter121(object sender, MouseEventArgs e)
        {
            TB121.Background = Brushes.DarkSeaGreen;
            TB120.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB121.FontSize = 25;
            TB120.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave121(object sender, MouseEventArgs e)
        {
            TB121.Background = Brushes.Transparent;
            TB120.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB120.FontSize = 12;
            TB121.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter122(object sender, MouseEventArgs e)
        {
            TB122.Background = Brushes.DarkSeaGreen;
            TB120.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB122.FontSize = 25;
            TB120.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave122(object sender, MouseEventArgs e)
        {
            TB122.Background = Brushes.Transparent;
            TB120.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB122.FontSize = 12;
            TB120.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter123(object sender, MouseEventArgs e)
        {
            TB123.Background = Brushes.DarkSeaGreen;
            TB120.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB123.FontSize = 25;
            TB120.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave123(object sender, MouseEventArgs e)
        {
            TB123.Background = Brushes.Transparent;
            TB120.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB123.FontSize = 12;
            TB120.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter124(object sender, MouseEventArgs e)
        {
            TB124.Background = Brushes.DarkSeaGreen;
            TB120.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB124.FontSize = 25;
            TB120.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave124(object sender, MouseEventArgs e)
        {
            TB124.Background = Brushes.Transparent;
            TB120.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB124.FontSize = 12;
            TB120.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter125(object sender, MouseEventArgs e)
        {
            TB125.Background = Brushes.DarkSeaGreen;
            TB120.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB125.FontSize = 25;
            TB120.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave125(object sender, MouseEventArgs e)
        {
            TB125.Background = Brushes.Transparent;
            TB120.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB125.FontSize = 12;
            TB120.FontSize = 12;
            FridayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter126(object sender, MouseEventArgs e)
        {
            TB126.Background = Brushes.DarkSeaGreen;
            TB120.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB126.FontSize = 25;
            TB120.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave126(object sender, MouseEventArgs e)
        {
            TB126.Background = Brushes.Transparent;
            TB120.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB126.FontSize = 12;
            TB120.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }


        private void TextBlock_MouseEnter141(object sender, MouseEventArgs e)
        {
            TB141.Background = Brushes.DarkSeaGreen;
            TB140.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB141.FontSize = 25;
            TB140.FontSize = 25;
            MondayTB.FontSize = 25;
        }
        private void TextBlock_MouseEnter131(object sender, MouseEventArgs e)
        {
            TB131.Background = Brushes.DarkSeaGreen;
            TB130.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB131.FontSize = 25;
            TB130.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave131(object sender, MouseEventArgs e)
        {
            TB131.Background = Brushes.Transparent;
            TB130.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB131.FontSize = 12;
            TB130.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter132(object sender, MouseEventArgs e)
        {
            TB132.Background = Brushes.DarkSeaGreen;
            TB130.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB132.FontSize = 25;
            TB130.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave132(object sender, MouseEventArgs e)
        {
            TB132.Background = Brushes.Transparent;
            TB130.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB132.FontSize = 12;
            TB130.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter133(object sender, MouseEventArgs e)
        {
            TB133.Background = Brushes.DarkSeaGreen;
            TB130.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB133.FontSize = 25;
            TB130.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave133(object sender, MouseEventArgs e)
        {
            TB133.Background = Brushes.Transparent;
            TB130.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB133.FontSize = 12;
            TB130.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter134(object sender, MouseEventArgs e)
        {
            TB134.Background = Brushes.DarkSeaGreen;
            TB130.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB134.FontSize = 25;
            TB130.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave134(object sender, MouseEventArgs e)
        {
            TB134.Background = Brushes.Transparent;
            TB130.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB134.FontSize = 12;
            TB130.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter135(object sender, MouseEventArgs e)
        {
            TB135.Background = Brushes.DarkSeaGreen;
            TB130.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB135.FontSize = 25;
            TB130.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave135(object sender, MouseEventArgs e)
        {
            TB135.Background = Brushes.Transparent;
            TB130.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB135.FontSize = 12;
            TB130.FontSize = 12;
            FridayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter136(object sender, MouseEventArgs e)
        {
            TB136.Background = Brushes.DarkSeaGreen;
            TB130.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB136.FontSize = 25;
            TB130.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave136(object sender, MouseEventArgs e)
        {
            TB136.Background = Brushes.Transparent;
            TB130.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB136.FontSize = 12;
            TB130.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }
        private void TextBlock_MouseLeave141(object sender, MouseEventArgs e)
        {
            TB141.Background = Brushes.Transparent;
            TB140.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB141.FontSize = 12;
            TB140.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter142(object sender, MouseEventArgs e)
        {
            TB142.Background = Brushes.DarkSeaGreen;
            TB140.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB142.FontSize = 25;
            TB140.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave142(object sender, MouseEventArgs e)
        {
            TB142.Background = Brushes.Transparent;
            TB140.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB142.FontSize = 12;
            TB140.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter143(object sender, MouseEventArgs e)
        {
            TB143.Background = Brushes.DarkSeaGreen;
            TB140.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB143.FontSize = 25;
            TB140.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave143(object sender, MouseEventArgs e)
        {
            TB143.Background = Brushes.Transparent;
            TB140.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB143.FontSize = 12;
            TB140.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter144(object sender, MouseEventArgs e)
        {
            TB144.Background = Brushes.DarkSeaGreen;
            TB140.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB144.FontSize = 25;
            TB140.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave144(object sender, MouseEventArgs e)
        {
            TB144.Background = Brushes.Transparent;
            TB140.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB144.FontSize = 12;
            TB140.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter145(object sender, MouseEventArgs e)
        {
            TB145.Background = Brushes.DarkSeaGreen;
            TB140.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB145.FontSize = 25;
            TB140.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave145(object sender, MouseEventArgs e)
        {
            TB145.Background = Brushes.Transparent;
            TB140.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB145.FontSize = 12;
            TB140.FontSize = 12;
            FridayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter146(object sender, MouseEventArgs e)
        {
            TB146.Background = Brushes.DarkSeaGreen;
            TB140.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB146.FontSize = 25;
            TB140.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave146(object sender, MouseEventArgs e)
        {
            TB146.Background = Brushes.Transparent;
            TB140.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB146.FontSize = 12;
            TB140.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter151(object sender, MouseEventArgs e)
        {
            TB151.Background = Brushes.DarkSeaGreen;
            TB150.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB151.FontSize = 25;
            TB150.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave151(object sender, MouseEventArgs e)
        {
            TB151.Background = Brushes.Transparent;
            TB150.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB151.FontSize = 12;
            TB150.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter152(object sender, MouseEventArgs e)
        {
            TB152.Background = Brushes.DarkSeaGreen;
            TB150.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB152.FontSize = 25;
            TB150.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave152(object sender, MouseEventArgs e)
        {
            TB152.Background = Brushes.Transparent;
            TB150.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB152.FontSize = 12;
            TB150.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter153(object sender, MouseEventArgs e)
        {
            TB153.Background = Brushes.DarkSeaGreen;
            TB150.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB153.FontSize = 25;
            TB150.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave153(object sender, MouseEventArgs e)
        {
            TB153.Background = Brushes.Transparent;
            TB150.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB153.FontSize = 12;
            TB150.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter154(object sender, MouseEventArgs e)
        {
            TB154.Background = Brushes.DarkSeaGreen;
            TB150.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB154.FontSize = 25;
            TB150.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave154(object sender, MouseEventArgs e)
        {
            TB154.Background = Brushes.Transparent;
            TB150.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB154.FontSize = 12;
            TB150.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter155(object sender, MouseEventArgs e)
        {
            TB155.Background = Brushes.DarkSeaGreen;
            TB150.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB155.FontSize = 25;
            TB150.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave155(object sender, MouseEventArgs e)
        {
            TB155.Background = Brushes.Transparent;
            TB150.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB155.FontSize = 12;
            TB150.FontSize = 12;
            FridayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter156(object sender, MouseEventArgs e)
        {
            TB156.Background = Brushes.DarkSeaGreen;
            TB150.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB156.FontSize = 25;
            TB150.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave156(object sender, MouseEventArgs e)
        {
            TB156.Background = Brushes.Transparent;
            TB150.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB156.FontSize = 12;
            TB150.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter161(object sender, MouseEventArgs e)
        {
            TB161.Background = Brushes.DarkSeaGreen;
            TB160.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB161.FontSize = 25;
            TB160.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave161(object sender, MouseEventArgs e)
        {
            TB161.Background = Brushes.Transparent;
            TB160.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB161.FontSize = 12;
            TB160.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter162(object sender, MouseEventArgs e)
        {
            TB162.Background = Brushes.DarkSeaGreen;
            TB160.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB162.FontSize = 25;
            TB160.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave162(object sender, MouseEventArgs e)
        {
            TB162.Background = Brushes.Transparent;
            TB160.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB162.FontSize = 12;
            TB160.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter163(object sender, MouseEventArgs e)
        {
            TB163.Background = Brushes.DarkSeaGreen;
            TB160.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB163.FontSize = 25;
            TB160.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave163(object sender, MouseEventArgs e)
        {
            TB163.Background = Brushes.Transparent;
            TB160.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB163.FontSize = 12;
            TB160.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter164(object sender, MouseEventArgs e)
        {
            TB164.Background = Brushes.DarkSeaGreen;
            TB160.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB164.FontSize = 25;
            TB160.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave164(object sender, MouseEventArgs e)
        {
            TB164.Background = Brushes.Transparent;
            TB160.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB164.FontSize = 12;
            TB160.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter165(object sender, MouseEventArgs e)
        {
            TB165.Background = Brushes.DarkSeaGreen;
            TB160.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB165.FontSize = 25;
            TB160.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave165(object sender, MouseEventArgs e)
        {
            TB165.Background = Brushes.Transparent;
            TB160.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB165.FontSize = 12;
            TB160.FontSize = 12;
            FridayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter166(object sender, MouseEventArgs e)
        {
            TB166.Background = Brushes.DarkSeaGreen;
            TB160.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB166.FontSize = 25;
            TB160.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave166(object sender, MouseEventArgs e)
        {
            TB166.Background = Brushes.Transparent;
            TB160.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB166.FontSize = 12;
            TB160.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter171(object sender, MouseEventArgs e)
        {
            TB171.Background = Brushes.DarkSeaGreen;
            TB170.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB171.FontSize = 25;
            TB170.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave171(object sender, MouseEventArgs e)
        {
            TB171.Background = Brushes.Transparent;
            TB170.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB171.FontSize = 12;
            TB170.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter172(object sender, MouseEventArgs e)
        {
            TB172.Background = Brushes.DarkSeaGreen;
            TB170.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB172.FontSize = 25;
            TB170.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave172(object sender, MouseEventArgs e)
        {
            TB172.Background = Brushes.Transparent;
            TB170.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB172.FontSize = 12;
            TB170.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter173(object sender, MouseEventArgs e)
        {
            TB173.Background = Brushes.DarkSeaGreen;
            TB170.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB173.FontSize = 25;
            TB170.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave173(object sender, MouseEventArgs e)
        {
            TB173.Background = Brushes.Transparent;
            TB170.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB173.FontSize = 12;
            TB170.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter174(object sender, MouseEventArgs e)
        {
            TB174.Background = Brushes.DarkSeaGreen;
            TB170.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB174.FontSize = 25;
            TB170.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave174(object sender, MouseEventArgs e)
        {
            TB174.Background = Brushes.Transparent;
            TB170.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB174.FontSize = 12;
            TB170.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter175(object sender, MouseEventArgs e)
        {
            TB175.Background = Brushes.DarkSeaGreen;
            TB170.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB175.FontSize = 25;
            TB170.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave175(object sender, MouseEventArgs e)
        {
            TB175.Background = Brushes.Transparent;
            TB170.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB175.FontSize = 12;
            TB170.FontSize = 12;
            FridayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter176(object sender, MouseEventArgs e)
        {
            TB176.Background = Brushes.DarkSeaGreen;
            TB170.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB176.FontSize = 25;
            TB170.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave176(object sender, MouseEventArgs e)
        {
            TB176.Background = Brushes.Transparent;
            TB170.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB176.FontSize = 12;
            TB170.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter181(object sender, MouseEventArgs e)
        {
            TB181.Background = Brushes.DarkSeaGreen;
            TB180.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB181.FontSize = 25;
            TB180.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave181(object sender, MouseEventArgs e)
        {
            TB181.Background = Brushes.Transparent;
            TB180.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB181.FontSize = 12;
            TB180.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter182(object sender, MouseEventArgs e)
        {
            TB182.Background = Brushes.DarkSeaGreen;
            TB180.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB182.FontSize = 25;
            TB180.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave182(object sender, MouseEventArgs e)
        {
            TB182.Background = Brushes.Transparent;
            TB180.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB182.FontSize = 12;
            TB180.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter183(object sender, MouseEventArgs e)
        {
            TB183.Background = Brushes.DarkSeaGreen;
            TB180.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB183.FontSize = 25;
            TB180.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave183(object sender, MouseEventArgs e)
        {
            TB183.Background = Brushes.Transparent;
            TB180.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB183.FontSize = 12;
            TB180.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter184(object sender, MouseEventArgs e)
        {
            TB184.Background = Brushes.DarkSeaGreen;
            TB180.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB184.FontSize = 25;
            TB180.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave184(object sender, MouseEventArgs e)
        {
            TB184.Background = Brushes.Transparent;
            TB180.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB184.FontSize = 12;
            TB180.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter185(object sender, MouseEventArgs e)
        {
            TB185.Background = Brushes.DarkSeaGreen;
            TB180.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB185.FontSize = 25;
            TB180.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave185(object sender, MouseEventArgs e)
        {
            TB185.Background = Brushes.Transparent;
            TB180.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB185.FontSize = 12;
            TB180.FontSize = 12;
            FridayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter186(object sender, MouseEventArgs e)
        {
            TB186.Background = Brushes.DarkSeaGreen;
            TB180.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB186.FontSize = 25;
            TB180.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave186(object sender, MouseEventArgs e)
        {
            TB186.Background = Brushes.Transparent;
            TB180.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB186.FontSize = 12;
            TB180.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter191(object sender, MouseEventArgs e)
        {
            TB191.Background = Brushes.DarkSeaGreen;
            TB190.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB191.FontSize = 25;
            TB190.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave191(object sender, MouseEventArgs e)
        {
            TB191.Background = Brushes.Transparent;
            TB190.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB191.FontSize = 12;
            TB190.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter192(object sender, MouseEventArgs e)
        {
            TB192.Background = Brushes.DarkSeaGreen;
            TB190.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB192.FontSize = 25;
            TB190.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave192(object sender, MouseEventArgs e)
        {
            TB192.Background = Brushes.Transparent;
            TB190.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB192.FontSize = 12;
            TB190.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter193(object sender, MouseEventArgs e)
        {
            TB193.Background = Brushes.DarkSeaGreen;
            TB190.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB193.FontSize = 25;
            TB190.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave193(object sender, MouseEventArgs e)
        {
            TB193.Background = Brushes.Transparent;
            TB190.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB193.FontSize = 12;
            TB190.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter194(object sender, MouseEventArgs e)
        {
            TB194.Background = Brushes.DarkSeaGreen;
            TB190.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB194.FontSize = 25;
            TB190.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave194(object sender, MouseEventArgs e)
        {
            TB194.Background = Brushes.Transparent;
            TB190.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB194.FontSize = 12;
            TB190.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter195(object sender, MouseEventArgs e)
        {
            TB195.Background = Brushes.DarkSeaGreen;
            TB190.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB195.FontSize = 25;
            TB190.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave195(object sender, MouseEventArgs e)
        {
            TB195.Background = Brushes.Transparent;
            TB190.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB195.FontSize = 12;
            TB190.FontSize = 12;
            FridayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter196(object sender, MouseEventArgs e)
        {
            TB196.Background = Brushes.DarkSeaGreen;
            TB190.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB196.FontSize = 25;
            TB190.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave196(object sender, MouseEventArgs e)
        {
            TB196.Background = Brushes.Transparent;
            TB190.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB196.FontSize = 12;
            TB190.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter201(object sender, MouseEventArgs e)
        {
            TB201.Background = Brushes.DarkSeaGreen;
            TB200.Background = Brushes.DarkSeaGreen;
            MondayTB.Background = Brushes.DarkSeaGreen;
            TB201.FontSize = 25;
            TB200.FontSize = 25;
            MondayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave201(object sender, MouseEventArgs e)
        {
            TB201.Background = Brushes.Transparent;
            TB200.Background = Brushes.Transparent;
            MondayTB.Background = Brushes.Transparent;
            TB201.FontSize = 12;
            TB200.FontSize = 12;
            MondayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter202(object sender, MouseEventArgs e)
        {
            TB202.Background = Brushes.DarkSeaGreen;
            TB200.Background = Brushes.DarkSeaGreen;
            TuesdayTB.Background = Brushes.DarkSeaGreen;
            TB202.FontSize = 25;
            TB200.FontSize = 25;
            TuesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave202(object sender, MouseEventArgs e)
        {
            TB202.Background = Brushes.Transparent;
            TB200.Background = Brushes.Transparent;
            TuesdayTB.Background = Brushes.Transparent;
            TB202.FontSize = 12;
            TB200.FontSize = 12;
            TuesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter203(object sender, MouseEventArgs e)
        {
            TB203.Background = Brushes.DarkSeaGreen;
            TB200.Background = Brushes.DarkSeaGreen;
            WednesdayTB.Background = Brushes.DarkSeaGreen;
            TB203.FontSize = 25;
            TB200.FontSize = 25;
            WednesdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave203(object sender, MouseEventArgs e)
        {
            TB203.Background = Brushes.Transparent;
            TB200.Background = Brushes.Transparent;
            WednesdayTB.Background = Brushes.Transparent;
            TB203.FontSize = 12;
            TB200.FontSize = 12;
            WednesdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter204(object sender, MouseEventArgs e)
        {
            TB204.Background = Brushes.DarkSeaGreen;
            TB200.Background = Brushes.DarkSeaGreen;
            ThursdayTB.Background = Brushes.DarkSeaGreen;
            TB204.FontSize = 25;
            TB200.FontSize = 25;
            ThursdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave204(object sender, MouseEventArgs e)
        {
            TB204.Background = Brushes.Transparent;
            TB200.Background = Brushes.Transparent;
            ThursdayTB.Background = Brushes.Transparent;
            TB204.FontSize = 12;
            TB200.FontSize = 12;
            ThursdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter205(object sender, MouseEventArgs e)
        {
            TB205.Background = Brushes.DarkSeaGreen;
            TB200.Background = Brushes.DarkSeaGreen;
            FridayTB.Background = Brushes.DarkSeaGreen;
            TB205.FontSize = 25;
            TB200.FontSize = 25;
            FridayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave205(object sender, MouseEventArgs e)
        {
            TB205.Background = Brushes.Transparent;
            TB200.Background = Brushes.Transparent;
            FridayTB.Background = Brushes.Transparent;
            TB205.FontSize = 12;
            TB200.FontSize = 12;
            FridayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter206(object sender, MouseEventArgs e)
        {
            TB206.Background = Brushes.DarkSeaGreen;
            TB200.Background = Brushes.DarkSeaGreen;
            SaturdayTB.Background = Brushes.DarkSeaGreen;
            TB206.FontSize = 25;
            TB200.FontSize = 25;
            SaturdayTB.FontSize = 25;
        }

        private void TextBlock_MouseLeave206(object sender, MouseEventArgs e)
        {
            TB206.Background = Brushes.Transparent;
            TB200.Background = Brushes.Transparent;
            SaturdayTB.Background = Brushes.Transparent;
            TB206.FontSize = 12;
            TB200.FontSize = 12;
            SaturdayTB.FontSize = 12;
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock obj = sender as TextBlock;
            obj.FontSize = 25;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock obj = sender as TextBlock;
            obj.FontSize = 12;
        }

        private void CanHelpExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Ova aplikacija namenjena je  vođenje evidencije o rasporedu predmeta u računarskom centru. Nastava iz odredjenih predmeta izvodi se u ucionicama koje imaju svoj naziv.Svaki predmet izvodi se u odredjenom terminu i u odredjenom kabinetu. Ukoliko zelite da neki predmet postavite u raspored potrebno je  najpre da izabere ucionicu u kojoj se taj predmet izvod a zatim da ga prevucete iz liste predmeta i postavite na zeljeni dan i termin. Brisanje predmeta iz kalendara mozete uraditi primenom precice ***** ", "Help!");


        }
    }
}
