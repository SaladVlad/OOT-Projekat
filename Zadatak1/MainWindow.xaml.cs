using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Zadatak1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Dogadjaj> TreeNodes { get; set; }
        public ObservableCollection<Dogadjaj> Postavljeni { get; set; }
        public List<TextBlock> listaPostavljenih { get; set; }

        Point startPoint = new Point();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            TreeNodes = ReadDogadjaji();
            Postavljeni = new ObservableCollection<Dogadjaj>();
            listaPostavljenih = new List<TextBlock>();

        }

        private ObservableCollection<Dogadjaj> ReadDogadjaji()
        {
            ObservableCollection<Dogadjaj> dogadjaji = new ObservableCollection<Dogadjaj>();
            try
            {
                using (StreamReader sr = new StreamReader("Files/Dogadjaji.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] s = line.Split(',');

                        Dogadjaj d = new Dogadjaj(Int32.Parse(s[0]), s[1], s[2], s[3], s[4]);
                        dogadjaji.Add(d);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Neuspesno ucitavanje iz fajla");
            }


            return dogadjaji;
        }

        private void btnSacuvajDogadjaje_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Files/Dogadjaji.txt"))
                {
                    foreach(Dogadjaj d in TreeNodes)
                    {
                        sw.WriteLine(d.ToString());
                    }
                }
            }
            catch
            {
                MessageBox.Show("Neuspesno cuvanje u fajl...");
            }
        }

        private void TreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void TreeView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged TreeView item
                TreeView treeView = sender as TreeView;
                TreeViewItem treeViewItem = FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);

                // Find the data behind the ListViewItem
                try
                {

                    Dogadjaj dogadjaj = (Dogadjaj)treeView.ItemContainerGenerator.
                    ItemFromContainer(treeViewItem);


                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("myFormat", dogadjaj);
                    DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move);


                }
                catch (ArgumentNullException)
                {
                }
                catch (InvalidCastException)
                {
                }


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

        private void Image_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Dogadjaj dogadjaj = e.Data.GetData("myFormat") as Dogadjaj;

                if (dogadjaj != null && !Postavljeni.Contains(dogadjaj))
                {

                    TextBlock newElement = new TextBlock();
                    newElement.Width = 40;
                    newElement.Height = 40;
                    newElement.Name = "e" + dogadjaj.Id.ToString();

                    ContextMenu contextMenu = new ContextMenu();

                    // Create menu items
                    MenuItem izmeni = new MenuItem();
                    izmeni.Header = "Izmeni";
                    izmeni.Click += Izmeni_Dogadjaj_Click;

                    MenuItem obrisi = new MenuItem();
                    obrisi.Header = "Obriši";
                    obrisi.Click += Obrisi_Dogadjaj_Click;

                    // Add menu items to the context menu
                    contextMenu.Items.Add(izmeni);
                    contextMenu.Items.Add(obrisi);

                    newElement.ContextMenu = contextMenu;
                    ContextMenuService.SetIsEnabled(newElement, true);

                    newElement.PreviewMouseLeftButtonDown += Element_PreviewMouseLeftButtonDown;

                    //const string baseDir = @"E:\Visual Studio Projects\Zadatak1\Zadatak1";
                    //string fileName = Path.Combine(baseDir,dogadjaj.ImageSource);

                    ImageBrush imageBrush = new ImageBrush();
                    Uri imageUri = new Uri(dogadjaj.ImageSource, UriKind.Relative);
                    imageBrush.ImageSource = new BitmapImage(imageUri);
                    newElement.Background = imageBrush;

                    Point dropPosition = e.GetPosition(sender as Image);
                    Canvas.SetLeft(newElement, dropPosition.X - 15);
                    Canvas.SetTop(newElement, dropPosition.Y - 15);



                    canvas.Children.Add(newElement);
                    Postavljeni.Add(dogadjaj);
                    listaPostavljenih.Add(newElement);

                }

            }
        }

        private void Element_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Izmeni_Dogadjaj_Click(sender,e);
            }
        }

        private void Izmeni_Dogadjaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dogadjaj d = trv.SelectedItem as Dogadjaj;
                Izmeni_Dogadjaj izmeni = new Izmeni_Dogadjaj(d);
                izmeni.Show();
            }
            catch
            {
                try
                {
                    
                    MenuItem menuItem = (MenuItem)sender;
                    ContextMenu contextMenu = (ContextMenu)menuItem.Parent;
                    TextBlock element = (TextBlock)contextMenu.PlacementTarget;

                    Dogadjaj d = TreeNodes[Int32.Parse(element.Name.Replace("e", "")) - 1];
                    Izmeni_Dogadjaj izmeni = new Izmeni_Dogadjaj(d);
                    izmeni.Show();
                }
                catch
                {

                }
            }
            
            
        }

        private void Obrisi_Dogadjaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dogadjaj d = trv.SelectedItem as Dogadjaj;
                TreeNodes.Remove(d);
                if (Postavljeni.Contains(d))
                {
                    foreach(TextBlock tb in listaPostavljenih)
                    {
                        if (tb.Name == "e" + d.Id.ToString())
                        {
                            Postavljeni.Remove(d);
                            listaPostavljenih.Remove(tb);

                            Canvas c = (Canvas)tb.Parent;
                            c.Children.Remove(tb);
                            
                            MessageBox.Show("Uspesno brisanje eventa!","Brisanje",MessageBoxButton.OK,MessageBoxImage.Information);
                            return;
                        }
                    }
                }
            }
            catch
            {
                try
                {

                    MenuItem menuItem = (MenuItem)sender;
                    ContextMenu contextMenu = (ContextMenu)menuItem.Parent;
                    TextBlock element = (TextBlock)contextMenu.PlacementTarget;

                    Dogadjaj d = TreeNodes[Int32.Parse(element.Name.Replace("e", "")) - 1];
                    TreeNodes.Remove(d);
                    if (Postavljeni.Contains(d))
                    {
                        foreach (TextBlock tb in listaPostavljenih)
                        {
                            if (tb.Name == "e" + d.Id.ToString())
                            {
                                Postavljeni.Remove(d);
                                listaPostavljenih.Remove(tb);

                                Canvas c = (Canvas)tb.Parent;
                                c.Children.Remove(tb);

                                MessageBox.Show("Uspesno brisanje eventa!", "Brisanje", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void Dodaj_Dogadjaj_Click(object sender, RoutedEventArgs e)
        {
            Dodaj_Dogadjaj dodaj = new Dodaj_Dogadjaj(TreeNodes);
            dodaj.Show();
        }

        
    }
}
