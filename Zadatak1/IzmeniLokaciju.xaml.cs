﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Path = System.IO.Path;
namespace Zadatak1
{
    /// <summary>
    /// Interaction logic for IzmeniLokaciju.xaml
    /// </summary>
    public partial class IzmeniLokaciju : Window
    {
        ObservableCollection<lokacija> lokacije = new ObservableCollection<lokacija>();
        TextBlock tekstboks = new TextBlock();
        lokacija objekat = new lokacija();
        string idconst, gradconst;
        public IzmeniLokaciju(ObservableCollection<lokacija> obscolection, List<lokacija> lista, TextBlock dobijeno)
        {

            InitializeComponent();



            tekstboks = dobijeno;
            List<lokacija> l = new List<lokacija>();
            lokacije = obscolection;
            l = lista;


            foreach (lokacija k in lokacije)
            {
                if (k.Grad == tekstboks.Text)
                {
                    objekat = k;

                }
            }

            dID.Text = objekat.Id;
            idconst = objekat.Id;
            gradconst = objekat.Grad;
            tGrad.Text = objekat.Grad;
            tSediste.Text = objekat.Drzava;

            if (objekat.Logo == "pack://application:,,,Logoi/novisad.png")
            {
                tlogo.SelectedIndex = 0;
            }
            else if (objekat.Logo == "pack://application:,,,Logoi/beograd.png")
            {
                tlogo.SelectedIndex = 1;
            }
            else if (objekat.Logo == "pack://application:,,,Logoi/nis.png")
            {
                tlogo.SelectedIndex = 2;
            }
            else if (objekat.Logo == "pack://application:,,,Logoi/leskovac.png")
            {
                tlogo.SelectedIndex = 3;
            }
            else if (objekat.Logo == "pack://application:,,,Logoi/subotica.png")
            {
                tlogo.SelectedIndex = 4;
            }
            else if (objekat.Logo == "pack://application:,,,Logoi/sremskamitrovica.png")
            {
                tlogo.SelectedIndex = 5;
            }
            else if (objekat.Logo == "pack://application:,,,Logoi/cacak.png")
            {
                tlogo.SelectedIndex = 6;
            }
            else if (objekat.Logo == "pack://application:,,,Logoi/jagodina.png")
            {
                tlogo.SelectedIndex = 7;
            }
            else if (objekat.Logo == "pack://application:,,,Logoi/pirot.png")
            {
                tlogo.SelectedIndex = 8;
            }
            else if (objekat.Logo == "pack://application:,,,Logoi/novipazar.png")
            {
                tlogo.SelectedIndex = 9;
            }
         

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool isIdUnique = lokacije.All(item => item.Id != dID.Text);
            if (dID.Text == idconst)
            {
                isIdUnique = true;
            }
            bool isGradUnique = lokacije.All(item => item.Grad.Trim().ToLower() != tGrad.Text.Trim().ToLower());
            if (tGrad.Text == gradconst)
            {
                isGradUnique = true;
            }
            bool isFieldsNotEmpty = !string.IsNullOrWhiteSpace(dID.Text) && !string.IsNullOrWhiteSpace(tGrad.Text) && !string.IsNullOrWhiteSpace(tSediste.Text);
            bool isIdValidNumber = int.TryParse(dID.Text, out int parsedId);

            if (isIdUnique && isGradUnique && isFieldsNotEmpty && isIdValidNumber)
            {
                objekat.Id = dID.Text;
                objekat.Grad = tGrad.Text;
                objekat.Drzava = tSediste.Text;
                objekat.Logo = UbacivanjePutanje();
                tekstboks.Text = objekat.Grad;
                try
                {
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri(UbacivanjePutanje(), UriKind.RelativeOrAbsolute));
                    image.Width = 30;
                    image.Height = 30;
                    InlineUIContainer inlineContainer = new InlineUIContainer(image);
                    tekstboks.Inlines.Add(inlineContainer);
                }
                catch (Exception) { }
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
                MessageBox.Show("Uneli ste vec postojeci ID,grad ili niste uneli neko od polja");
            }

        }
        int br2 = -1;
        private string UbacivanjePutanje()
        {
            string logolocation = "pack://application:,,,";
            logolocation += "/Logoi/";
            if (br2 == 1)
            {
                logolocation += "novisad";
            }
            else if (br2 == 2)
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
            else
            {
                logolocation = "";
                logolocation = "C:\\Users\\Tomas\\Desktop\\OOT-PROJEKAT\\Zadatak1\\Logoi\\";
                string selectedItemText = string.Empty;

                if (tlogo.SelectedItem is ComboBoxItem selectedComboBoxItem)
                {
                    selectedItemText = selectedComboBoxItem.Content.ToString();
                }

                logolocation += selectedItemText;

                return logolocation;

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
                    case "Nista":
                        br2 = 11;
                        break;

                }
            }
        }
        private void DodajLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";

            bool? dialogResult = openFileDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                string selectedFileName = openFileDialog.FileName;
                string selectedFilePath = Path.GetFullPath(selectedFileName);

                string selectedFileNameOnly = Path.GetFileName(selectedFileName);

                string dirName = AppDomain.CurrentDomain.BaseDirectory;
                FileInfo fileInfo = new FileInfo(dirName);
                DirectoryInfo parentDir1 = fileInfo.Directory.Parent;
                DirectoryInfo parentDir = parentDir1.Parent;
                string baseDirectory = parentDir.FullName;

                string destinationFolder = Path.Combine(baseDirectory, "Logoi");

                Directory.CreateDirectory(destinationFolder);

                string destinationFilePath = Path.Combine(destinationFolder, selectedFileNameOnly);
                try
                {

                    File.Copy(selectedFilePath, destinationFilePath);
                    ComboBoxItem newItem = new ComboBoxItem
                    {
                        Content = selectedFileNameOnly
                    };
                    tlogo.Items.Add(newItem);


                    tlogo.SelectedItem = newItem;

                }
                catch (Exception )
                {

                }
            }
            br2 = 12;
        }
    }
}
