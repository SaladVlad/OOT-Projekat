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
using System.Windows.Shapes;

namespace Zadatak1
{
    /// <summary>
    /// Interaction logic for IzmeniLokaciju.xaml
    /// </summary>
    public partial class IzmeniLokaciju : Window
    {
        ObservableCollection<lokacija> lokacije = new ObservableCollection<lokacija>();
        public IzmeniLokaciju(ObservableCollection<lokacija> obscolection, List<lokacija> lista,TextBox dobijeno)
        {
            List<lokacija> l = new List<lokacija>();
            lokacije = obscolection;
            l = lista;


            InitializeComponent();
        }
    }
}
