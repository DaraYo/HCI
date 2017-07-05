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
            this.mainWindow = mw;
            String[] allSubs = File.ReadAllLines("../../config/library.txt");

            Subs = new ObservableCollection<Subject>();

            if (allSubs.Length != 0)
                foreach (var item in allSubs)
                {
                    string[] items = item.Split('/');
                    Subs.Add(new Subject { });
                }
        }

        /*public Subjests(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }*/
    }
}
