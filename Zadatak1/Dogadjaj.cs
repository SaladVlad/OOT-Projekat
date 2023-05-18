using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Zadatak1
{
    public class Dogadjaj : INotifyPropertyChanged
    {

        

        private int id;

        private List<string> atributi;
        private string naziv, opis, datumOdrzavanja, imageSource;

        private Image slika;
        private double posX, posY;

        public Dogadjaj(int id, string naziv, string opis, string datumOdrzavanja, string imageSource)
        {
            this.Id = id;
            this.Naziv = naziv;
            this.Opis = opis;
            this.DatumOdrzavanja = datumOdrzavanja;
            this.imageSource = imageSource;

            atributi = new List<string>();

            atributi.Add(naziv);
            atributi.Add(opis);
            atributi.Add(datumOdrzavanja);

            this.Slika = new Image();
            if (imageSource == "")
                 Slika.Source = null;
            else Slika.Source = new BitmapImage(new Uri(imageSource,UriKind.Relative));


        }

        #region =====set-get=====
        public int Id {
            get { return this.id; }
            set {
                if (this.id != value) { this.id = value; this.NotifyPropertyChanged("Id"); }
            }
        }
        public string Naziv {
            get { return this.naziv; }
            set
            {
                if (this.naziv != value) { this.naziv = value; this.NotifyPropertyChanged("Naziv"); }
            }
        }
        public string Opis {
            get { return this.opis; }
            set
            {
                if (this.opis != value) { this.opis = value; this.NotifyPropertyChanged("Opis"); }
            }
        }
        public string DatumOdrzavanja {
            get { return this.datumOdrzavanja; }
            set
            {
                if (this.datumOdrzavanja != value) { this.datumOdrzavanja = value; this.NotifyPropertyChanged("DatumOdrzavanja"); }
            }
        }
        public string ImageSource
        {
            get { return this.imageSource; }
            set
            {
                if (this.imageSource != value) { this.imageSource = value; this.NotifyPropertyChanged("ImageSource"); }
            }
        }
        public Image Slika {
            get { return this.slika; }
            set
            {
                if (this.slika != value) { this.slika = value; this.NotifyPropertyChanged("Slika"); }
            }
        }
        public double PosX {
            get { return this.posX; }
            set
            {
                if (this.posX != value) { this.posX = value; this.NotifyPropertyChanged("PosX"); }
            }
        }
        public double PosY {
            get { return this.posY; }
            set
            {
                if (this.posY != value) { this.posY = value; this.NotifyPropertyChanged("PosY"); }
            }
        }
        public List<string> Atributi { get => atributi; set => atributi = value; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }

}
