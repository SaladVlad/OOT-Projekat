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
    /// Interaction logic for Dodaj_Dogadjaj.xaml
    /// </summary>
    public partial class Dodaj_Dogadjaj : Window
    {   
        public ObservableCollection<Dogadjaj> Dogadjaji { get; set; }
        public Dodaj_Dogadjaj(ObservableCollection<Dogadjaj> dogadjaji)
        {
            InitializeComponent();
            Dogadjaji = dogadjaji;

        }

        private void btnNapravi_Click(object sender, RoutedEventArgs e)
        {
            
                if (tbNaziv.Text == "" || tbOpis.Text == "" || tbDatum.Text == "" || tbID.Text == "")
                {
                    MessageBox.Show("Ne smeju da postoje prazna polja!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                else
                {
                    string[] ss = tbDatum.Text.Split('.');
                    int check;
                    try
                    {
                        if (Int32.TryParse(ss[0], out check) && Int32.TryParse(ss[1], out check) && Int32.TryParse(ss[2], out check) && ss.Length == 4)
                        {
                        int id = int.Parse(tbID.Text);
                            string datumOdrzavanja = tbDatum.Text;
                            string naziv = tbNaziv.Text;
                            string opis = tbOpis.Text;
                            string imageSource;
                        if (tbSource.Text == "(default)") imageSource = "Images/placeholder.png"; 
                        else imageSource = "Images/" + tbSource.Text;

                        Dogadjaj d = new Dogadjaj(id, naziv, opis, datumOdrzavanja, imageSource);
                        Dogadjaji.Add(d);
                    }   
                    }
                    catch
                    {
                        MessageBox.Show("Format datuma nije ispravan!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    


                }
                MessageBox.Show("Uspesno dodat dogadjaj!", "Dodavanje", MessageBoxButton.OK, MessageBoxImage.Information);
           
        }

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
