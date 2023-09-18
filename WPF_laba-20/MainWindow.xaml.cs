using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_laba_20
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Stroke> undidStrokes = new List<Stroke>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mkNew(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы хотите сохранить изменения?", "CapyPaint Lite", 
                MessageBoxButton.YesNoCancel);

            switch (result)
            {   
                case MessageBoxResult.Yes:
                    SaveFile(sender, e);
                    MainCanvas.Strokes.Clear();
                    MainCanvas.Background = null;
                    break;
                case MessageBoxResult.No:
                    MainCanvas.Strokes.Clear();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void OpenFile(object sender, RoutedEventArgs e)
         {
             OpenFileDialog openFD = new OpenFileDialog
             {
                 Filter = "PNG files (*.png)|*.png|JPG files (*.jpg)|*.jpg|JPEG files (*.jpeg)|*.jpeg"
             };

            if (openFD.ShowDialog() == true)
            {
                ImageBrush img = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(openFD.FileName, UriKind.Absolute))
                };
                MainCanvas.Background = img;
            }
         }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFD = new SaveFileDialog
            {
                Filter = "PNG files (*.png)|*.png|JPG files (*.jpg)|*.jpg|JPEG files (*.jpeg)|*.jpeg"
            };

            if (saveFD.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(saveFD.FileName, FileMode.Create))
                {
                    string ext = System.IO.Path.GetExtension(saveFD.FileName).ToUpper();

                    switch (ext)
                    {
                        case ".PNG":
                            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                            RenderTargetBitmap pngTarget = new RenderTargetBitmap((int)MainCanvas.ActualWidth, (int)MainCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                            pngTarget.Render(MainCanvas);
                            pngEncoder.Frames.Add(BitmapFrame.Create(pngTarget));
                            pngEncoder.Save(fs);
                            break;
                        case ".JPG":
                        case ".JPEG":
                            JpegBitmapEncoder jpegEncoder = new JpegBitmapEncoder();
                            RenderTargetBitmap jpegTarget = new RenderTargetBitmap((int)MainCanvas.ActualWidth, (int)MainCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                            jpegTarget.Render(MainCanvas);
                            jpegEncoder.Frames.Add(BitmapFrame.Create(jpegTarget));
                            jpegEncoder.Save(fs);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private void Exit(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Вы хотите сохранить изменения?", "CapyPaint Lite",
               MessageBoxButton.YesNoCancel);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    SaveFile(sender, e);
                    MainCanvas.Strokes.Clear();
                    MainCanvas.Background = null;
                    Close();
                    break;
                case MessageBoxResult.No:
                    MainCanvas.Strokes.Clear();
                    Close();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void Undo(object sender, RoutedEventArgs e)
        {
            if (MainCanvas.Strokes.Count > 0)
            {
                undidStrokes.Add(MainCanvas.Strokes[MainCanvas.Strokes.Count - 1]);
                MainCanvas.Strokes.RemoveAt(MainCanvas.Strokes.Count - 1);

            }
            else
            {
                MessageBox.Show("Ты тупой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Redo(object sender, RoutedEventArgs e)
        {
            if (undidStrokes.Count != 0)
            {
                Stroke stroke = undidStrokes[undidStrokes.Count-1];
                MainCanvas.Strokes.Add(stroke);
                undidStrokes.Remove(stroke);
            }
            else
            {
                MessageBox.Show("Дима урод", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Приложение CapiPaint Lite(полной версии не будет никогда)\n\nВерсия: Настолько сырая насколько можно\n\nДата создания: 18.09.2023\n\nРазработчики:\n-Капипар Кабибаров\n-Кабибар Капипаров\n-Ефремов Вадим\n\nСодействие:\n-Дмитрий Истомин\n-Билл Гейтс\n-Владислав Смирнов", "О CapyPaint Lite", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
