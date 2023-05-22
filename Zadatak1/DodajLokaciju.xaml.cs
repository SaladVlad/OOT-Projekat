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
    /// Interaction logic for DodajLokaciju.xaml
    /// </summary>
    public partial class DodajLokaciju : Window
    {
        ObservableCollection<lokacija> lokacije = new ObservableCollection<lokacija>();
        public DodajLokaciju(ObservableCollection<lokacija> obscolection , List<lokacija> lista)
        {
            
            List<lokacija> l = new List<lokacija>();
            lokacije = obscolection;
            l = lista;
          


            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lokacija objekat = new lokacija();
            int br=0;
            foreach (lokacija b in lokacije) {
                if (b.Id == tID.Text)
                    MessageBox.Show("Vec postoji grad sa tim id-em pokusajte ponovo");
                br = 1;
                tID.Text = "";
                }

            if (br != 1)
            {
                objekat.Id = tID.Text;
            }
            
            objekat.Grad = tGrad.Text;
            objekat.Drzava = tSediste.Text;
            objekat.Logo = tlogo.Text;
            lokacije.Add(objekat);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
