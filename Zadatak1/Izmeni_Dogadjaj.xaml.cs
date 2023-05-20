using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Zadatak1
{
    /// <summary>
    /// Interaction logic for Izmeni_Dogadjaj.xaml
    /// </summary>
    public partial class Izmeni_Dogadjaj : Window
    {
        public Dogadjaj D { get; set; }
        public Izmeni_Dogadjaj(Dogadjaj d)
        {
            InitializeComponent();
            D = d;
            tbID.Text = D.Id.ToString();
            tbID.IsEnabled = false;
            tbNaziv.Text = D.Naziv;
            tbOpis.Text = D.Opis;
            tbDatum.Text = D.DatumOdrzavanja;
            try
            {
                cmbSource.SelectedIndex = int.Parse(D.ImageSource.Replace("Images/s", "").Remove(1));
            }
            catch
            {
                cmbSource.SelectedIndex = 0;
            }
            cmbSource_SelectionChanged(null,null);
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (tbNaziv.Text == "" || tbOpis.Text == "" || tbDatum.Text == "")
            {
                MessageBox.Show("Ne smeju da postoje prazna polja!","Upozorenje",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                string[] ss = tbDatum.Text.Split('.');
                int check;
                try
                {
                    if (Int32.TryParse(ss[0], out check) && Int32.TryParse(ss[1], out check) && Int32.TryParse(ss[2], out check))
                    {
                        D.DatumOdrzavanja = tbDatum.Text;
                    }
                }
                catch {
                    MessageBox.Show("Format datuma nije ispravan!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                D.Naziv = tbNaziv.Text;
                D.Opis = tbOpis.Text;
                D.Update_List();


            }
            MessageBox.Show("Uspesna izmena!", "Izmena", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmbSource_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            string selected = (cmbSource.SelectedItem as ComboBoxItem).Content.ToString();
            BitmapImage bmi;
            if (selected == "(default)")
            {
                selected = "placeholder";
            }
            bmi = new BitmapImage(new Uri("Images/" + selected + ".png", UriKind.RelativeOrAbsolute));
            
            
            if(img.Source != bmi)
            {
                img.Source = bmi;
                D.Slika.Source = bmi;
                D.ImageSource = "Images/" + selected + ".png";
            }
        }
    }
}
