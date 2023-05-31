using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Zadatak1
{
    public class Dogadjaj : INotifyPropertyChanged
    {
        private int id;

        private ObservableCollection<string> atributi;
        private string naziv, opis, datumOdrzavanja, imageSource;

        private Image slika;

        private string lokacija;

        public Dogadjaj(int id, string naziv, string opis, string datumOdrzavanja, string imageSource, string lokacija)
        {
            this.Id = id;
            this.Naziv = naziv;
            this.Opis = opis;
            this.DatumOdrzavanja = datumOdrzavanja;
            this.imageSource = imageSource;
            this.lokacija = lokacija;

            atributi = new ObservableCollection<string>
            {
                opis,
                datumOdrzavanja,
                lokacija
            };

            this.Slika = new Image();
            if (imageSource == "" && (!imageSource.Contains(".png") || !imageSource.Contains(".png")))
                Slika.Source = new BitmapImage(new Uri("placeholder.png", UriKind.Relative));
            else Slika.Source = new BitmapImage(new Uri(imageSource, UriKind.Relative));


        }

        public override string ToString()
        {
            return id + "," + naziv + "," + opis + "," + datumOdrzavanja + "," + imageSource + "," + lokacija;
        }

        public void Update_List()
        {
            atributi.Clear();
            atributi.Add(Opis);
            atributi.Add(DatumOdrzavanja);
            atributi.Add(Lokacija);
            OnPropertyChanged("Atributi");
        }

        #region =====set-get=====
        public int Id
        {
            get { return this.id; }
            set
            {
                if (this.id != value) { this.id = value; this.OnPropertyChanged("Id"); }
            }
        }
        public string Naziv
        {
            get { return this.naziv; }
            set
            {
                if (this.naziv != value) { this.naziv = value; this.OnPropertyChanged("Naziv"); }
            }
        }
        public string Opis
        {
            get { return this.opis; }
            set
            {
                if (this.opis != value) { this.opis = value; this.OnPropertyChanged("Opis"); }
            }
        }
        public string DatumOdrzavanja
        {
            get { return this.datumOdrzavanja; }
            set
            {
                if (this.datumOdrzavanja != value) { this.datumOdrzavanja = value; this.OnPropertyChanged("DatumOdrzavanja"); }
            }
        }
        public ObservableCollection<string> Atributi
        {
            get { return this.atributi; }
            set
            {
                if(this.atributi != value) { this.atributi = value; this.OnPropertyChanged("Atributi");
                    this.OnPropertyChanged("Naziv");
                    this.OnPropertyChanged("Opis");
                    this.OnPropertyChanged("DatumOdrzavanja");
                }
            }
        }
        public string ImageSource
        {
            get { return this.imageSource; }
            set
            {
                if (this.imageSource != value) { this.imageSource = value; this.OnPropertyChanged("ImageSource"); }
            }
        }
        public Image Slika
        {
            get { return this.slika; }
            set
            {
                if (this.slika != value) { this.slika = value; this.OnPropertyChanged("Slika"); }
            }
        }

        public string Lokacija {
            get { return this.lokacija; }
            set
            {
                if (this.lokacija != value) { this.lokacija = value; this.OnPropertyChanged("Lokacija"); }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
