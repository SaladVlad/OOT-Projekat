using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
namespace Zadatak1
{
    public class lokacija : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        private string id;
        private string grad;
        private string drzava;
        private string logo;


        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Grad
        {
            get
            {
                return grad;
            }
            set
            {
                if (value != grad)
                {
                    grad = value;
                    OnPropertyChanged("Grad");
                }
            }
        }

        public string Drzava
        {
            get
            {
                return drzava;
            }
            set
            {
                if (value != drzava)
                {
                    drzava = value;
                    OnPropertyChanged("Drzava");
                }
            }
        }
        public string Logo
        {
            get
            {
                return logo;
            }
            set
            {
                if (value != logo)
                {
                    logo = value;
                    OnPropertyChanged("Logo");
                }
            }
        }




    }
}
