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
          
            l.Add(new lokacija { Id = "1", Grad = "Novi Sad", Drzava = "Srbija", Logo = "/Logoi/novisad.png" } );
            l.Add(new lokacija { Id = "2", Grad = "Beograd", Drzava = "Srbija", Logo = "/Logoi/beograd.png" });
            l.Add(new lokacija { Id = "3", Grad = "Nis", Drzava = "Srbija", Logo = "/Logoi/nis.png" });
            l.Add(new lokacija { Id = "4", Grad = "Subotica", Drzava = "Srbija", Logo = "/Logoi/subotica.png" });
            l.Add(new lokacija { Id = "5", Grad = "Leskovac", Drzava = "Srbija", Logo = "/Logoi/leskovac.png" });

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
        public class AutoResizedCanvas : Canvas
        {
            protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)
            {
                base.MeasureOverride(constraint);
                double width = base
                    .InternalChildren
                    .OfType<UIElement>()
                    .Where(i => i.GetValue(Canvas.LeftProperty) != null)
                    .Max(i => i.DesiredSize.Width + (double)i.GetValue(Canvas.LeftProperty));

                if (Double.IsNaN(width))
                {
                    width = 0;
                }

                double height = base
                    .InternalChildren
                    .OfType<UIElement>()
                    .Where(i => i.GetValue(Canvas.TopProperty) != null)
                    .Max(i => i.DesiredSize.Height + (double)i.GetValue(Canvas.TopProperty));

                if (Double.IsNaN(height))
                {
                    height = 0;
                }

                return new Size(width, height);
            }
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
                    // Get the dragged ListViewItem
                    ListView listView = sender as ListView;
                    ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);


                    // Find the data behind the ListViewItem
                    


                    lokacija nista = (lokacija)listapogleda.SelectedItems[0];
                    int index = listapogleda.Items.IndexOf(nista);
                    ListViewItem itemToDisable = (ListViewItem)listapogleda.ItemContainerGenerator.ContainerFromIndex(index);
                    if (itemToDisable.IsEnabled == true)
                    {
                        // Initialize the drag & drop operation
                        lokacija student = (lokacija)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                        DataObject dragData = new DataObject("myFormat", student);
                        DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                    }
                }
            }
            catch(ArgumentNullException o)
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

        TextBlock nesto2;
        private void slika_Drop(object sender, DragEventArgs e)
        {

            
                Point dropPosition = e.GetPosition(slika);
                lokacija student = e.Data.GetData("myFormat") as lokacija;
          nesto2 = new TextBlock();

            if (student != null)
            {

           
                //---------------------------------


                nesto2.Text = student.Grad;

                nesto2.HorizontalAlignment = HorizontalAlignment.Center;

                nesto2.PreviewMouseLeftButtonDown += Nesto2_PreviewMouseLeftButtonDown;
                nesto2.MouseLeftButtonDown += Nesto2_MouseLeftButtonDown;
                nesto2.MouseRightButtonDown += nesto2_MouseRightButtonDown;


                Image image = new Image();
                image.Source = new BitmapImage(new Uri(student.Logo, UriKind.RelativeOrAbsolute));
                image.Width = 30;
                image.Height = 30;
                InlineUIContainer inlineContainer = new InlineUIContainer(image);
                nesto2.Inlines.Add(inlineContainer);

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
            Canvas.SetLeft(nesto2, dropPosition.X);
            Canvas.SetTop(nesto2, dropPosition.Y);


        }
        TextBlock nova;
 
        private void Nesto2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            nova = sender as TextBlock;
            DragDrop.DoDragDrop(sender as TextBlock, sender as TextBlock, DragDropEffects.Move);
            brojac++;
       
          
           
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

            }

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            DodajLokaciju dl = new DodajLokaciju(lokacije,l);
            dl.Show();
        }

               int brojac=0;
        private void Nesto2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
                IzmeniLokaciju iz = new IzmeniLokaciju(lokacije,l,sender as TextBox);
                iz.Show();
         
        }
    }
}
