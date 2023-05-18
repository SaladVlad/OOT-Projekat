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
using Zadatak1;

namespace Zadatak1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Dogadjaj> TreeNodes { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            TreeNodes = new ObservableCollection<Dogadjaj>
            {
                new Dogadjaj(1, "Naziv1", "Opis1", "Datum1", "Images/serbia-map.jpg"),
                new Dogadjaj(2, "Naziv2", "Opis2", "Datum2", "ImageSource2")
            };

        }

    }
}
