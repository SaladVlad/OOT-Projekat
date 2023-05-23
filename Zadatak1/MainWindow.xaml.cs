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

//FOR WRITING TO AN EXCEL FILE//
using Microsoft.Win32;
using OfficeOpenXml;
//----------------------------//
/*Disclaimer: used a NuGet package inside solution, install before using "Save to .xls" function.*/

namespace Zadatak1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region PR40/2021

        #region window and lists init
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

            ContextMenuService.SetIsEnabled(cmImage, true);

        }
        #endregion

        #region loading/saving from/to  file
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
                    foreach (Dogadjaj d in TreeNodes)
                    {
                        sw.WriteLine(d.ToString());
                    }
                }
                MessageBox.Show("Uspesno sacuvano u fajl!", "Cuvanje", MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
                Izmeni_Dogadjaj izmeni = new Izmeni_Dogadjaj(treeViewItem.DataContext as Dogadjaj, listaPostavljenih);
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
                if (treeViewItem != null)
                {
                    Dogadjaj dogadjaj = (Dogadjaj)treeView.ItemContainerGenerator.
                    ItemFromContainer(treeViewItem);

                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("myFormat", dogadjaj);
                    DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move);
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

            //setting up event for drag&drop and double click
            newElement.PreviewMouseLeftButtonDown += Element_PreviewMouseLeftButtonDown;

            //asigning a new ImageBrush object for TextBlock background
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
                Izmeni_Dogadjaj_Click(sender, e);
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
            if (isDragging && tb != null)
            {
                isDragging = false;
                tb.ReleaseMouseCapture();
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && tb != null)
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

        #region adding/removing events
        private void Izmeni_Dogadjaj_Click(object sender, RoutedEventArgs e)
        {

            if(sender is TextBlock)
            {
                TextBlock element = (TextBlock)sender;
                if (sender != null && element.Name.Contains("e"))
                {
                    foreach (Dogadjaj d in TreeNodes)
                    {
                        if (element.Name.Replace("e", "") == d.Id.ToString())
                        {
                            Izmeni_Dogadjaj izmeni = new Izmeni_Dogadjaj(d, listaPostavljenih);
                            izmeni.Show();
                        }
                    }
                }
            }
            else
            {
                MenuItem menuItem = sender as MenuItem;
                ContextMenu contextMenu = (ContextMenu)menuItem?.Parent;
                TextBlock element = (TextBlock)contextMenu?.PlacementTarget;

                if (element != null && element.Name.Contains("e"))
                {
                    foreach (Dogadjaj d in TreeNodes)
                    {
                        if (element.Name.Replace("e", "") == d.Id.ToString())
                        {
                            Izmeni_Dogadjaj izmeni = new Izmeni_Dogadjaj(d, listaPostavljenih);
                            izmeni.Show();
                        }
                    }
                }
                else
                {
                    Dogadjaj d = trv.SelectedItem as Dogadjaj;
                    if (d != null)
                    {
                        Izmeni_Dogadjaj izmeni = new Izmeni_Dogadjaj(d, listaPostavljenih);
                        izmeni.Show();
                    }
                }
            }
        }
        private void Ukloni_Dogadjaj_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;
            TextBlock element = (TextBlock)contextMenu.PlacementTarget;
            foreach (Dogadjaj d in TreeNodes)
            {
                if (element.Name.Replace("e", "") == d.Id.ToString()) RemoveFromCanvas(d);

            }
        }
        private void Obrisi_Dogadjaj_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            ContextMenu contextMenu = menuItem?.Parent as ContextMenu;
            Image image = contextMenu?.PlacementTarget as Image;

            if (image != null && image.Tag?.ToString() == "image")
            {
                TextBlock element = (TextBlock)contextMenu.PlacementTarget;

                Dogadjaj d = TreeNodes[Int32.Parse(element.Name.Replace("e", "")) - 1];
                TreeNodes.Remove(d);
                if (Postavljeni.Contains(d)) RemoveFromCanvas(d);

            }
            else
            {
                Dogadjaj d = trv.SelectedItem as Dogadjaj;
                TreeNodes.Remove(d);
                if (Postavljeni.Contains(d)) RemoveFromCanvas(d);
            }
        }
        private void RemoveFromCanvas(Dogadjaj d)
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

        Point p;
        private int before;
        private void Dodaj_Dogadjaj_Click(object sender, RoutedEventArgs e)
        {
            Dodaj_Dogadjaj dodaj = new Dodaj_Dogadjaj(TreeNodes);

            MenuItem menuItem = sender as MenuItem;
            ContextMenu contextMenu = menuItem?.Parent as ContextMenu;
            Image image = contextMenu?.PlacementTarget as Image;

            if (image != null && image.Tag?.ToString() == "image")
            {
                p = Mouse.GetPosition(canvas);
                dodaj.Closed += Dodaj_Closed;
            }
            before = TreeNodes.Count;
            dodaj.Show();

        }
        private void Dodaj_Closed(object sender, EventArgs e)
        {
            if (before < TreeNodes.Count)
            {

                TextBlock newElement = Generate_TextBlock(TreeNodes.Last());

                Canvas.SetTop(newElement, p.Y);
                Canvas.SetLeft(newElement, p.X);

                canvas.Children.Add(newElement);
                Postavljeni.Add(TreeNodes.Last());
                listaPostavljenih.Add(newElement);
            }
        }

        #endregion

        private void Save_XLS_Click(object sender, RoutedEventArgs e)
        {

            string filePath;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File (*.xls)|*.xls";
            saveFileDialog.DefaultExt = ".xls";

            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Lista");

                    int columnIndex = 1;
                    foreach (DataGridColumn column in dtg.Columns)
                    {
                        worksheet.Cells[1, columnIndex].Value = column.Header;
                        columnIndex++;
                    }

                    int rowIndex = 2;
                    foreach (var item in dtg.Items)
                    {
                        columnIndex = 1;
                        foreach (DataGridColumn column in dtg.Columns)
                        {
                            var cellValue = (column.GetCellContent(item) as TextBlock).Text;
                            worksheet.Cells[rowIndex, columnIndex].Value = cellValue;
                            columnIndex++;
                        }
                        rowIndex++;
                    }
                    package.SaveAs(new FileInfo(filePath));
                }
            }
        }

        #endregion
    }
}
