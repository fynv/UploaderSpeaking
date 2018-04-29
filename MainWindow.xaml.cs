using System;
using System.Collections.Generic;
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
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Threading;

namespace UploaderSpeaking
{
    class Marker
    {
        public System.Drawing.Color color;
        public List<System.Windows.Point> Points;
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        System.Drawing.Color text_color = System.Drawing.Color.Yellow;
        byte text_alpha = 255;
        System.Drawing.Color drawing_color = System.Drawing.Color.Red;
        bool drawing = false;

        public MainWindow()
        {
            InitializeComponent();

            #region fullscreen
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            this.Topmost = true;
            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            #endregion  

            timer.Tick += new EventHandler(timer_Tick);

            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(text_color.A, text_color.R, text_color.G, text_color.B));
            btnTextColor.Foreground = brush;
            Speech.Foreground = brush;

            SolidColorBrush brush2 = new SolidColorBrush(System.Windows.Media.Color.FromArgb(drawing_color.A, drawing_color.R, drawing_color.G, drawing_color.B));
            btnDrawColor.Foreground = brush2;
            
        }

        private void Focus_Click(object sender, RoutedEventArgs e)
        {
            Speech.Text = "";
            Speech.Focus();
        }

        private void BtnTextColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                text_color = colorDialog.Color;
                SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(text_color.A, text_color.R, text_color.G, text_color.B));
                btnTextColor.Foreground =brush;
                Speech.Foreground = brush;
            }
        }
        private void Text_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Speech.IsReadOnly = true;
                int len = Speech.Text.Length;
                text_alpha = 255;
                timer.Interval = TimeSpan.FromSeconds(1.0f);
                timer.Start();
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (text_alpha > 15)
            {
                timer.Interval = TimeSpan.FromSeconds(0.03f);
                text_alpha -= 15;
                SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(text_alpha, text_color.R, text_color.G, text_color.B));
                Speech.Foreground = brush;

            }
            else
            {
                SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(text_color.A, text_color.R, text_color.G, text_color.B));
                Speech.Foreground = brush;
                Speech.Text = "";
                Speech.IsReadOnly = false;
                timer.Stop();
            }
        }

        private void btnDrawColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                drawing_color = colorDialog.Color;
                SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(drawing_color.A, drawing_color.R, drawing_color.G, drawing_color.B));
                btnDrawColor.Foreground = brush;
            }
        }

        private void btnDraw_Click(object sender, RoutedEventArgs e)
        {
            canvas.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(1, 0, 0, 0));
            drawing = true;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
        }

        private void OnCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (drawing)
            {
                System.Windows.Point pos = e.GetPosition(canvas);

                Polyline polyLine = new Polyline();
                polyLine.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(drawing_color.A, drawing_color.R, drawing_color.G, drawing_color.B));
                polyLine.StrokeThickness = 5;
                polyLine.Points.Add(pos);
                canvas.Children.Add(polyLine);

            }

        }

        private void OnCanvasMosueMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (drawing && e.LeftButton == MouseButtonState.Pressed)
            {
                System.Windows.Point pos = e.GetPosition(canvas);
                Polyline polyLine = (Polyline)canvas.Children[canvas.Children.Count - 1];
                polyLine.Points.Add(pos);
            }
        }

        private void OnCanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (drawing)
            {               
                drawing = false;
                canvas.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
            }

        }

    }
}
