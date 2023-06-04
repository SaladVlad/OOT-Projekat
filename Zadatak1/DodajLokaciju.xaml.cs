using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Path = System.IO.Path;

namespace Zadatak1
{
    /// <summary>
    /// Interaction logic for DodajLokaciju.xaml
    /// </summary>
    public partial class DodajLokaciju : Window
    {
        ObservableCollection<lokacija> lokacije = new ObservableCollection<lokacija>();
        List<lokacija> l = new List<lokacija>();
        public DodajLokaciju(ObservableCollection<lokacija> obscolection , List<lokacija> lista)
        {
            
           
            lokacije = obscolection;
            l = lista;
          


            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tID.Text) && !string.IsNullOrWhiteSpace(tGrad.Text) && !string.IsNullOrWhiteSpace(tSediste.Text) && !string.IsNullOrWhiteSpace(tlogo.Text))
            {
                string id = tID.Text;
                string grad = tGrad.Text;

             
                bool isIdExists = lokacije.Any(item => item.Id == id);
                bool isGradExists = lokacije.Any(item => item.Grad == grad);

                if (!isIdExists && !isGradExists)
                {
                    lokacija objekat = new lokacija();
                    objekat.Id = id;
                    objekat.Grad = grad;
                    objekat.Drzava = tSediste.Text;
                    objekat.Logo = UbacivanjePutanje();
               
                    lokacije.Add(objekat);

                    string filePath = "Podaci.txt";

                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        foreach (lokacija item in lokacije)
                        {
                            writer.WriteLine(item.Id + ";" + item.Grad + ";" + item.Drzava + ";" + item.Logo);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Već postoji unos sa zadatim ID-em ili Gradom. Molimo unesite drugu vrednost.");
                    tID.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Niste uneli sva polja.");
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        { 
            this.Close();
        }
       
        
        int br2 = 0;
        private string UbacivanjePutanje()
        {
            string logolocation = "";
            logolocation += "pack://application:,,,/Logoi/";
           
            if (br2 == 1)
            {
                logolocation+= "novisad.png";
            }else if (br2 == 2)
            {
                logolocation += "beograd.png";
            }
            else if (br2 == 3)
            {
                logolocation += "nis.png";
            }
            else if (br2 == 4)
            {
                logolocation += "leskovac.png";
            }
            else if (br2 == 5)
            {
                logolocation += "subotica.png";
            }
            else if (br2 == 6)
            {
                logolocation += "sremskamitrovica.png";
            }
            else if (br2 == 7)
            {
                logolocation += "cacak.png";
            }
            else if (br2 == 8)
            {
                logolocation += "jagodina.png";
            }
            else if (br2 == 9)
            {
                logolocation += "pirot.png";
            }
            else if (br2 == 10)
            {
                logolocation += "novipazar.png";
            }
            else if (br2 == 11)
            {
                logolocation += "nista.png";
            }
            else
            {
                //string selectedItemText = string.Empty;

                //if (tlogo.SelectedItem is ComboBoxItem selectedComboBoxItem)
                //{
                //    selectedItemText = selectedComboBoxItem.Content.ToString();
                //}

                //logolocation += selectedItemText;
         
            }
         

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
                    case "Nista":
                        br2 = 11;
                        break;
                   
                       
                }
            }
          
        }

        //private void DodajLogo_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();

        //    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        //    openFileDialog.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";

     
        //    bool? dialogResult = openFileDialog.ShowDialog();
        //    if (dialogResult.HasValue && dialogResult.Value)
        //    {
        //        string selectedFileName = openFileDialog.FileName;
        //        string selectedFilePath = Path.GetFullPath(selectedFileName);

           
        //        string selectedFileNameOnly = Path.GetFileName(selectedFileName);


        //        string dirName = AppDomain.CurrentDomain.BaseDirectory; 
        //        FileInfo fileInfo = new FileInfo(dirName);
        //        DirectoryInfo parentDir1 = fileInfo.Directory.Parent;
        //        DirectoryInfo parentDir = parentDir1.Parent;
        //        string baseDirectory = parentDir.FullName;
       


        //        string destinationFolder = Path.Combine(baseDirectory, "Images");



        //        Directory.CreateDirectory(destinationFolder);

           
        //        string destinationFilePath = Path.Combine(destinationFolder, selectedFileNameOnly);
        //        try
        //        {
           
        //            File.Copy(selectedFilePath, destinationFilePath);
        //            ComboBoxItem newItem = new ComboBoxItem
        //            {
        //                Content = selectedFileNameOnly
        //            };
        //            tlogo.Items.Add(newItem);

         
        //            tlogo.SelectedItem = newItem;
             
               

        //        }
        //        catch (Exception ex)
        //        {
                

        //        }
        //    }
        //    br2 = 12;
        //}
    }
}
