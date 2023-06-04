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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Zadatak1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ContextMenu contextMenu = new ContextMenu();
        MenuItem menuItem1 = new MenuItem();
    

            MenuItem menuItem2 = new MenuItem();
     
        Point startPoint = new Point();
        List<lokacija> l = new List<lokacija>();

        public MainWindow()
        {
            this.DataContext = this;

            //l.Add(new lokacija { Id = "1", Grad = "Novi Sad", Drzava = "Srbija", Logo = "/Logoi/novisad.png" });
            //l.Add(new lokacija { Id = "2", Grad = "Beograd", Drzava = "Srbija", Logo = "/Logoi/beograd.png" });
            //l.Add(new lokacija { Id = "3", Grad = "Nis", Drzava = "Srbija", Logo = "/Logoi/nis.png" });
            //l.Add(new lokacija { Id = "4", Grad = "Subotica", Drzava = "Srbija", Logo = "/Logoi/subotica.png" });
            //l.Add(new lokacija { Id = "5", Grad = "Leskovac", Drzava = "Srbija", Logo = "/Logoi/leskovac.png" });
            string[] lines = File.ReadAllLines("Podaci.txt");

            foreach (string line in lines)
            {

                string[] values = line.Split(';');

                if (values.Length == 4)
                {
                    lokacija obj = new lokacija
                    {
                        Id = values[0],
                        Grad = values[1],
                        Drzava = values[2],
                        Logo = values[3]
                    };


                    l.Add(obj);
                }
            }

            lokacije = new ObservableCollection<lokacija>(l);
            menuItem1.Header = "Ukloni ikonicu sa mape";
            menuItem1.Click += MenuItem_Click;
            menuItem2.Header = "Ukloni lokaciju iz liste";
            menuItem2.Click += MenuItem_Click;

            contextMenu.Items.Add(menuItem1);
            contextMenu.Items.Add(menuItem2);

                InitializeComponent();
            
        }
        public ObservableCollection<lokacija> lokacije
        {
            get;
            set;
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);

        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Point mousePos = e.GetPosition(null);
                Vector diff = startPoint - mousePos;

                if (e.LeftButton == MouseButtonState.Pressed &&
                    (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    try
                    {
                        ListView listView = sender as ListView;
                        ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);



                        lokacija nista = (lokacija)listapogleda.SelectedItems[0];
                        int index = listapogleda.Items.IndexOf(nista);
                        ListViewItem itemToDisable = (ListViewItem)listapogleda.ItemContainerGenerator.ContainerFromIndex(index);

                        if (itemToDisable.IsEnabled == true)
                        {

                            lokacija podaci = (lokacija)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                            DataObject dragData = new DataObject("myFormat", podaci);
                            DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                        }
                    }
                    catch (Exception)
                    {
                    }

                }
            }
            catch (ArgumentNullException o)
            {

            }
        }
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void slika_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

      
        private void slika_Drop(object sender, DragEventArgs e)
        {

            
                Point dropPosition = e.GetPosition(slika);
                lokacija podaci = e.Data.GetData("myFormat") as lokacija;
            TextBlock nesto2 = new TextBlock();
            if (podaci != null)
            {

           
                //---------------------------------


                nesto2.Text = podaci.Grad;

                nesto2.HorizontalAlignment = HorizontalAlignment.Center;

                nesto2.PreviewMouseLeftButtonDown += Nesto2_PreviewMouseLeftButtonDown;
                nesto2.MouseLeftButtonDown += Nesto2_MouseLeftButtonDown;
                nesto2.MouseRightButtonDown += nesto2_MouseRightButtonDown;
                nesto2.MouseDown += Nesto2_MouseWheel;


                try
                {
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri(podaci.Logo, UriKind.RelativeOrAbsolute));
                    image.Width = 30;
                    image.Height = 30;
                    InlineUIContainer inlineContainer = new InlineUIContainer(image);
                    nesto2.Inlines.Add(inlineContainer);
                }
                catch (Exception) { }
                slika.Children.Add(nesto2);
              

                lokacija nista = (lokacija)listapogleda.SelectedItems[0];
                int index = listapogleda.Items.IndexOf(nista);
                ListViewItem itemToDisable = (ListViewItem)listapogleda.ItemContainerGenerator.ContainerFromIndex(index);
                itemToDisable.IsEnabled = false;
                itemToDisable.IsHitTestVisible = false;

            }
            else
            {
                nesto2 = nova;
                nesto2.PreviewMouseLeftButtonDown += Nesto2_PreviewMouseLeftButtonDown;
                nesto2.MouseRightButtonDown += nesto2_MouseRightButtonDown;
              

            }
            Canvas.SetLeft(nesto2, dropPosition.X -20);
            Canvas.SetTop(nesto2, dropPosition.Y - 30);


        }
        TextBlock nova;
    
        private void Nesto2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          
                nova = sender as TextBlock;
                DragDrop.DoDragDrop(sender as TextBlock, sender as TextBlock, DragDropEffects.Move);
 
        
        }
        private void Nesto2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
         
        }
        TextBlock desno;
        private void nesto2_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            desno = sender as TextBlock;
      
            TextBlock textBlock = sender as TextBlock;
            textBlock.ContextMenu = contextMenu;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (sender == menuItem1)
            {
                   foreach (lokacija nista in l)
                {
                    if (nista.Grad == desno.Text)
                    {
                        int index = listapogleda.Items.IndexOf(nista);
                        ListViewItem itemToDisable = (ListViewItem)listapogleda.ItemContainerGenerator.ContainerFromIndex(index);
                        itemToDisable.IsEnabled = true;
                        itemToDisable.IsHitTestVisible = true;
                        slika.Children.Remove(desno);
                    }
                }
                    slika.Children.Remove(desno);
                
            }
            if (sender == menuItem2)
            {
             

                List<lokacija> itemsToRemove = new List<lokacija>();

                lokacija opa = new lokacija();
                foreach(lokacija k in l)
                {
                    if(k.Grad == desno.Text)
                    {
                        opa = k;
                    }
                }
                lokacije.Remove(opa);
                slika.Children.Remove(desno);
                string filePath = "Podaci.txt";

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (lokacija item in lokacije)
                    {
                        writer.WriteLine(item.Id + ";" + item.Grad + ";" + item.Drzava  +";" + item.Logo);
                    }
                }

            }

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            DodajLokaciju dl = new DodajLokaciju(lokacije,l);
            dl.Show();
        }

            

        private void Nesto2_MouseWheel(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {

                TextBlock textBlock = sender as TextBlock;
                IzmeniLokaciju iz = new IzmeniLokaciju(lokacije, l, textBlock);
                iz.Show();
            }
        }
    }
}
