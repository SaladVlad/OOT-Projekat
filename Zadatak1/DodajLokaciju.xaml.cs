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
            if (tGrad.Text != "" && tSediste.Text != "" && tlogo.Text != "" && tID.Text != "")
            {
                lokacija objekat = new lokacija();
                lokacija temp = new lokacija();
                int br = 0;
                foreach (lokacija b in lokacije)
                {
                    if (b.Id == tID.Text)
                    {
                        br = 1;
                    }

                }



                if (br != 1)
                {
                    objekat.Id = tID.Text;
                    objekat.Grad = tGrad.Text;
                    objekat.Drzava = tSediste.Text;
                    objekat.Logo = UbacivanjePutanje();
                    MessageBox.Show(UbacivanjePutanje());
                    lokacije.Add(objekat);
                }
                else
                {
                    MessageBox.Show("Vec postoji poslje sa zadatim ID-em probajte neku drugu vrednost");
                    tID.Text = "";
                }
            }
            else
                MessageBox.Show("Niste uneli neko od polja");

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
        int br2 = 0;
        private string UbacivanjePutanje()
        {
            string logolocation = "";
            logolocation += "/Logoi/";
            if (br2 == 1)
            {
                logolocation+="novisad";
            }else if (br2 == 2)
            {
                logolocation += "beograd";
            }
            else if (br2 == 3)
            {
                logolocation += "nis";
            }
            else if (br2 == 4)
            {
                logolocation += "leskovac";
            }
            else if (br2 == 5)
            {
                logolocation += "subotica";
            }
            else if (br2 == 6)
            {
                logolocation += "sremskamitrovica";
            }
            else if (br2 == 7)
            {
                logolocation += "cacak";
            }
            else if (br2 == 8)
            {
                logolocation += "jagodina";
            }
            else if (br2 == 9)
            {
                logolocation += "pirot";
            }
            else if (br2 == 10)
            {
                logolocation += "novipazar";
            }
            logolocation += ".png";

            return logolocation;
        }
        private void tlogo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            if (selectedItem != null)
            {
                string selectedCity = selectedItem.Content.ToString();

                
                switch (selectedCity)
                {
                    case "Novi Sad":
                        br2 = 1;
                        break;
                    case "Beograd":
                        br2 = 2;
                        break;
                    case "Nis":
                        br2 = 3;
                        break;
                    case "Leskovac":
                        br2 = 4;
                        break;
                    case "Subotica":
                        br2 = 5;
                        break;
                    case "Sremska Mitrovica":
                        br2 = 6;
                        break;
                    case "Cacak":
                        br2 = 7;
                        break;
                    case "Jagodina":
                        br2 = 8;
                        break;
                    case "Pirot":
                        br2 = 9;
                        break;
                    case "Novi Pazar":
                        br2 = 10;
                        break;
                    case "nista":
                        br2 = 11;
                        break;
                       
                }
            }
          
        }

        
    }
}
