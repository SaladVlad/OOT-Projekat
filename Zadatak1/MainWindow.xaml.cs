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
using System.Windows.Media;

namespace Zadatak1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point startPoint = new Point();
        public MainWindow()
        {
            this.DataContext = this;
            List<lokacija> l = new List<lokacija>();
            l.Add(new lokacija { Id = "1", Grad = "Novi Sad", Drzava = "Srbija", Logo = "/Logoi/novisad.png" } );
            l.Add(new lokacija { Id = "2", Grad = "Beograd", Drzava = "Srbija", Logo = "/Logoi/beograd.png" });
            l.Add(new lokacija { Id = "3", Grad = "Nis", Drzava = "Srbija", Logo = "/Logoi/nis.png" });
            l.Add(new lokacija { Id = "4", Grad = "Subotica", Drzava = "Srbija", Logo = "/Logoi/subotica.png" });
            l.Add(new lokacija { Id = "5", Grad = "Leskovac", Drzava = "Srbija", Logo = "/Logoi/leskovac.png" });

            lokacije = new ObservableCollection<lokacija>(l);
            InitializeComponent();
        }
        public ObservableCollection<lokacija> lokacije
        {
            get;
            set;
        }

        private void TextBlock_MouseMove(object sender, MouseEventArgs e)
        {
         
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
   
           
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);

        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
    

                // Find the data behind the ListViewItem
                lokacija student = (lokacija)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", student);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
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
                lokacija student = e.Data.GetData("myFormat") as lokacija;
            TextBlock nesto = new TextBlock();
            slika.Children.Add(nesto);
            nesto.Text = student.Grad;
            nesto.FontSize = 20;
            nesto.Background = Brushes.Transparent;

          
        
    //          lokacija nista = (lokacija)listapogleda.SelectedItems[0];
  //        int index =listapogleda.Items.IndexOf(nista);
//            ListViewItem item = listapogleda.Items[index] as ListViewItem;

         //   foreach (ListViewItem i in listapogleda.Items)
          //  {
                
            //    MessageBox.Show(i.ToString());
            //}
            //item.IsEnabled = false;
            //   (ListViewItem)listapogleda.Items.
            //nista.IsEnabled = false;
            //       lokacije.Remove(student);
            Canvas.SetLeft(nesto, dropPosition.X );
                Canvas.SetTop(nesto, dropPosition.Y);



            
        }
    }
}
