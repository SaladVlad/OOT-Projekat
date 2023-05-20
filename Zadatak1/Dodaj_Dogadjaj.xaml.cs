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
        int brDogadjaja;
        public Dodaj_Dogadjaj(ObservableCollection<Dogadjaj> dogadjaji, ref int brDogadjaja)
        {
            InitializeComponent();
            Dogadjaji = dogadjaji;
            this.brDogadjaja = brDogadjaja;

        }

        private void BtnNapravi_Click(object sender, RoutedEventArgs e)
        {
            if (tbNaziv.Text == "" || tbOpis.Text == "" || tbDatum.Text == "" || tbID.Text == "")
            {
                MessageBox.Show("Ne smeju da postoje prazna polja!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            else if (!int.TryParse(tbID.Text, out _))
            {
                MessageBox.Show("Format ID-a nije ispravan! ID može da sadrži samo brojeve.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                string[] ss = tbDatum.Text.Split('.');

                if (int.TryParse(ss[0], out _) && int.TryParse(ss[1], out _) && int.TryParse(ss[2], out _) && (ss.Length == 4 || ss.Length == 3))
                {

                    int dan, mesec, godina;
                    dan = int.Parse(ss[0]);
                    mesec = int.Parse(ss[1]);
                    godina = int.Parse(ss[2]);

                    if ((godina > 2022 && godina < 2100) && (mesec >= 1 && mesec <= 12) && (dan >= 1 && dan <= 31)) {
                        
                        int id = int.Parse(tbID.Text);
                        string datumOdrzavanja = tbDatum.Text;
                        string naziv = tbNaziv.Text;
                        string opis = tbOpis.Text;
                        ComboBoxItem cbi = cmbSource.SelectedItem as ComboBoxItem;
                        string imageSource = cbi.Content.ToString();
                        if (imageSource == "(default)") imageSource = "Images/placeholder.png";
                        else imageSource = "Images/" + imageSource + ".png";

                        Dogadjaj d = new Dogadjaj(id, naziv, opis, datumOdrzavanja, imageSource);
                        foreach(Dogadjaj dog in Dogadjaji)
                        {
                            if (dog.Id == id)
                            {
                                MessageBox.Show("Vec postoji dogadjaj sa ovim ID-em!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Hand);
                                return;
                            }
                        }
                        Dogadjaji.Add(d);
                        brDogadjaja++;
                    }
                    else
                    {
                        MessageBox.Show("Ovaj datum nije validan!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
 
                }
                else
                {
                    MessageBox.Show("Format datuma nije ispravan!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }


            }
            MessageBox.Show("Uspesno dodat dogadjaj!", "Dodavanje", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();

        }

        private void BtnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
