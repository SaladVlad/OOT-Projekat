using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        public int brojDogadjaja = 0;
        public List<TextBlock> listaPostavljenih { get; set; }

        Point startPoint = new Point();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            TreeNodes = ReadDogadjaji();
            Postavljeni = new ObservableCollection<Dogadjaj>();
            listaPostavljenih = new List<TextBlock>();

            ContextMenuService.SetIsEnabled(cmImage, true);

        }

        #region loading/saving from file
        public ObservableCollection<Dogadjaj> ReadDogadjaji()
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
                        brojDogadjaja++;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Neuspesno ucitavanje iz fajla");
            }

            return dogadjaji;
        }

        
        public void BtnSacuvajDogadjaje_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Uspesno sacuvano u fajl!","Cuvanje",MessageBoxButton.OK,MessageBoxImage.Asterisk);
            }
            catch
            {
                MessageBox.Show("Neuspesno cuvanje u fajl...");
            }
        }

        #endregion

        #region TreeView dragNdrop
        private void TreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ClickCount == 2)
            {
                TreeViewItem treeViewItem = FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
                Izmeni_Dogadjaj izmeni = new Izmeni_Dogadjaj(treeViewItem.DataContext as Dogadjaj);
                izmeni.Show();
            }
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

                // Find the data behind the TreeViewItem
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

                    TextBlock newElement = Generate_TextBlock(dogadjaj);

                    Point dropPosition = e.GetPosition(sender as Image);
                    Canvas.SetLeft(newElement, dropPosition.X - 15);
                    Canvas.SetTop(newElement, dropPosition.Y - 15);

                    canvas.Children.Add(newElement);
                    Postavljeni.Add(dogadjaj);
                    listaPostavljenih.Add(newElement);

                }
                else if (Postavljeni.Contains(dogadjaj))
                {
                    MessageBox.Show("Ovaj dogadjaj je vec dodat!");
                }

            }
        }

        #endregion
        private TextBlock Generate_TextBlock(Dogadjaj d)
        {
            TextBlock newElement = new TextBlock
            {
                Width = 40,
                Height = 40,
                Name = "e" + d.Id.ToString()
            };

            //Create context menu for textblock

            ContextMenu contextMenu = new ContextMenu();

            MenuItem izmeni = new MenuItem();
            izmeni.Header = "Izmeni";
            izmeni.Click += Izmeni_Dogadjaj_Click;

            MenuItem ukloni = new MenuItem();
            ukloni.Header = "Ukloni";
            ukloni.Click += Ukloni_Dogadjaj_Click;

            MenuItem obrisi = new MenuItem();
            obrisi.Header = "Obriši";
            obrisi.Click += Obrisi_Dogadjaj_Click;

        
            contextMenu.Items.Add(izmeni);
            contextMenu.Items.Add(ukloni);
            contextMenu.Items.Add(obrisi);

            newElement.ContextMenu = contextMenu;
            ContextMenuService.SetIsEnabled(newElement, true);

            newElement.PreviewMouseLeftButtonDown += Element_PreviewMouseLeftButtonDown;



            //const string baseDir = @"E:\Visual Studio Projects\Zadatak1\Zadatak1";
            //string fileName = Path.Combine(baseDir,dogadjaj.ImageSource);

            ImageBrush imageBrush = new ImageBrush();
            Uri imageUri = new Uri(d.ImageSource, UriKind.Relative);
            imageBrush.ImageSource = new BitmapImage(imageUri);
            newElement.Background = imageBrush;

            return newElement;
        }

        #region ---TextBox events---

        bool isDragging = false;
        TextBlock tb = null;
        private void Element_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Izmeni_Dogadjaj_Click(sender,e);
            }
            else
            {
                tb = (TextBlock)sender;
                isDragging = true;
                startPoint = e.GetPosition(canvas);
                tb.CaptureMouse();
            }
        }

        #region drag/drop textblock
        private void Canvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(isDragging && tb != null)
            {
                isDragging = false;
                tb.ReleaseMouseCapture();
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(isDragging && tb != null)
            {
                Point current = e.GetPosition(canvas);

                double diffX = current.X - startPoint.X;
                double diffY = current.Y - startPoint.Y;

                Canvas.SetLeft(tb, Canvas.GetLeft(tb) + diffX);
                Canvas.SetTop(tb, Canvas.GetTop(tb) + diffY);

                startPoint = current;
            }
        }

        #endregion

        #endregion

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
        private void Ukloni_Dogadjaj_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;
            TextBlock element = (TextBlock)contextMenu.PlacementTarget;
            foreach(Dogadjaj d in TreeNodes)
            {
                if(element.Name.Replace("e","") == d.Id.ToString())
                {
                    foreach (TextBlock tb in listaPostavljenih)
                    {
                        if (tb.Name == "e" + d.Id.ToString())
                        {
                            Postavljeni.Remove(d);
                            listaPostavljenih.Remove(tb);

                            Canvas c = (Canvas)tb.Parent;
                            c.Children.Remove(tb);
                            return;
                        }
                    }
                }
                
            }

            

        }
        private void Obrisi_Dogadjaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dogadjaj d = trv.SelectedItem as Dogadjaj;
                TreeNodes.Remove(d);
                brojDogadjaja--;
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

        Point p;
        private void Dodaj_Dogadjaj_Click(object sender, RoutedEventArgs e)
        {
            Dodaj_Dogadjaj dodaj = new Dodaj_Dogadjaj(TreeNodes,ref brojDogadjaja);
            try
            {
                Canvas c = ((sender as MenuItem).Parent as ContextMenu).PlacementTarget as Canvas;
                p = Mouse.GetPosition(canvas);
                dodaj.Closed += Dodaj_Closed;
            }
            catch { }
            finally
            {
                dodaj.Show();
            }
           
        }
        private void Dodaj_Closed(object sender, EventArgs e)
        {
            if (TreeNodes.Count != brojDogadjaja)
            {
                TextBlock newElement = Generate_TextBlock(TreeNodes.Last());

                Canvas.SetTop(newElement, p.Y);
                Canvas.SetLeft(newElement, p.X);

                canvas.Children.Add(newElement);
                Postavljeni.Add(TreeNodes.Last());
                listaPostavljenih.Add(newElement);
            }
        }
    }
}
