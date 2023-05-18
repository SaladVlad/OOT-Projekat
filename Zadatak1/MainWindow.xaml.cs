using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Zadatak1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = this;
            List<lokacija> l = new List<lokacija>();
            l.Add(new lokacija { Id = "1", Grad = "Novi Sad", Drzava = "Srbija", Logo = "/Logoi/novisad.png" } );
            l.Add(new lokacija { Id = "2", Grad = "Beograd", Drzava = "Srbija", Logo = "/Logoi/beograd.png" });
            l.Add(new lokacija { Id = "3", Grad = "Nis", Drzava = "Srbija", Logo = "/Logoi/nis.png" });
            l.Add(new lokacija { Id = "4", Grad = "Subotica", Drzava = "Srbija", Logo = "/Logoi/subotica.png" });
            l.Add(new lokacija { Id = "5", Grad = "Leskovac", Drzava = "Srbija", Logo = "/Logoi/leskovac.png" });

            lokacije = new ObservableCollection<lokacija>(l);
            InitializeComponent();
        }
        public ObservableCollection<lokacija> lokacije
        {
            get;
            set;
        }

    }
}
