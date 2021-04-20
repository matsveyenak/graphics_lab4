using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace lab_4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int pixelScale = 1;
        private static int middleX = 0, middleY = 0;
        private static int w, h, offset = 30;
        private static HashSet<(int, int)> dots = new HashSet<(int, int)>();
        private int[] initParameters(int mode)
        {
            int x1, x2, y1, y2, r;
            if (mode == 0)
            {
                if (!int.TryParse(text_x1.Text, out x1))
                    MessageBox.Show("Please enter integer values", "Warning");
                if (!int.TryParse(text_x2.Text, out x2))
                    MessageBox.Show("Please enter integer values", "Warning");
                if (!int.TryParse(text_y1.Text, out y1))
                    MessageBox.Show("Please enter integer values", "Warning");
                if (!int.TryParse(text_y2.Text, out y2))
                    MessageBox.Show("Please enter integer values", "Warning");

                return new int[] { x1, x2, y1, y2 };
            }
            else;
            {
                if (!int.TryParse(text_x1.Text, out x1))
                    MessageBox.Show("Please enter integer values", "Warning");
                if (!int.TryParse(text_y1.Text, out y1))
                    MessageBox.Show("Please enter integer values", "Warning");
                if (!int.TryParse(text_r.Text, out r))
                    MessageBox.Show("Please enter integer values", "Warning");

                return new int[] { x1, y1, r };
            }
        }
        private void stepButton_Click(object sender, RoutedEventArgs e)
        {
            int[] param = initParameters(0);
            int x1 = param[0], x2 = param[1], y1 = param[2], y2 = param[3];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            dots.Clear();

            var steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (steep)
            {
                swap(ref x1, ref y1);
                swap(ref x2, ref y2);
            }

            if (x1 > x2)
            {
                swap(ref x1, ref x2);
                swap(ref y1, ref y2);
            }

            if (x1 == x2)
            {
                for (int i = y1; i <= y2; i++)
                    dots.Add((x1, -i));
            }
            else
            {
                double k = (1.0 * y2 - y1) / (x2 - x1);
                double b = y1 - k * x1;

                for (int i = x1; i <= x2; i++)
                {
                    int Y = (int)Math.Round(k * i + b);
                    dots.Add((steep ? Y : i, steep ? -i : -Y));
                }
            }
            stopwatch.Stop();

            long ticks = stopwatch.ElapsedTicks;
            double ns = 1000000000.0 * ticks / Stopwatch.Frequency;
            timer.Text = ns + " ns";
            canvasPaint();
        }

        private void ddaButton_Click(object sender, RoutedEventArgs e)
        {
            int[] param = initParameters(0);
            int x1 = param[0], x2 = param[1], y1 = param[2], y2 = param[3];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            dots.Clear();

            double dx = x2 - x1;
            double dy = y2 - y1;
            double L = Math.Max(Math.Abs(dx), Math.Abs(dy));

            dots.Add((x1, -y1));

            double X = x1;
            double Y = y1;

            for (int i = 1; i < L; i++)
            {
                X += dx / L;
                Y += dy / L;
                dots.Add(((int)Math.Round(X), -(int)Math.Round(Y)));
            }

            dots.Add((x2, -y2));

            stopwatch.Stop();

            long ticks = stopwatch.ElapsedTicks;
            double ns = 1000000000.0 * ticks / Stopwatch.Frequency;
            timer.Text = ns + " ns";

            canvasPaint();
        }

        private static void swap(ref int x1, ref int x2)
        {
            int t = x2;
            x2 = x1;
            x1 = t;
        }

        private void lineButton_Click(object sender, RoutedEventArgs e)
        {
            int[] param = initParameters(0);
            int x1 = param[0], x2 = param[1], y1 = param[2], y2 = param[3];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            dots.Clear();

            var steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (steep)
            {
                swap(ref x1, ref y1);
                swap(ref x2, ref y2);
            }

            if (x1 > x2)
            {
                swap(ref x1, ref x2);
                swap(ref y1, ref y2);
            }

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int err = dx / 2;
            int yStep = (y1 < y2) ? 1 : -1;
            int y = y1;

            for (int x = x1; x <= x2; x++)
            {
                dots.Add((steep ? y : x, steep ? -x : -y));
                err -= dy;

                if (err < 0)
                {
                    y += yStep;
                    err += dx;
                }
            }

            stopwatch.Stop();

            long ticks = stopwatch.ElapsedTicks;
            double ns = 1000000000.0 * ticks / Stopwatch.Frequency;
            timer.Text = ns + " ns";

            canvasPaint();
        }

        private void circleButton_Click(object sender, RoutedEventArgs ee)
        {
            int[] param = initParameters(1);
            int x1 = param[0], y1 = -param[1], r = param[2];
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            dots.Clear();

            int x = 0;
            int y = r;
            int e = 3 - 2 * r;

            dots.Add((x + x1, y + y1));
            dots.Add((x + x1, -y + y1));
            dots.Add((-x + x1, y + y1));
            dots.Add((-x + x1, -y + y1));

            dots.Add((y + x1, x + y1));
            dots.Add((-y + x1, x + y1));
            dots.Add((y + x1, -x + y1));
            dots.Add((-y + x1, -x + y1));

            while (x < y)
            {
                if (e >= 0)
                {
                    e += 4 * (x - y) + 10;
                    x += 1;
                    y -= 1;
                }
                else
                {
                    e += 4 * x + 6;
                    x += 1;
                }

                dots.Add((x + x1, y + y1));
                dots.Add((x + x1, -y + y1));
                dots.Add((-x + x1, y + y1));
                dots.Add((-x + x1, -y + y1));

                dots.Add((y + x1, x + y1));
                dots.Add((y + x1, -x + y1));
                dots.Add((-y + x1, x + y1));
                dots.Add((-y + x1, -x + y1));
            }

            stopwatch.Stop();

            long ticks = stopwatch.ElapsedTicks;
            double ns = 1000000000.0 * ticks / Stopwatch.Frequency;
            timer.Text = ns + " ns";

            canvasPaint();
        }
        public MainWindow()
        {
            InitializeComponent();
            canvasPaint();
        }
        private void canvasPaint()
        {
            canvas.Children.Clear();
            w = (int)canvas.Width;  
            h = (int)canvas.Height;

            int initX = middleX  - w / pixelScale / 2;
            int endX = initX + w / pixelScale;

            int initY = middleY  - h / pixelScale / 2;
            int endY = initY + h / pixelScale;

            int c = 0;

            for (int i = initX; i <= endX; i += (endX - initX) / 40)
            {
                int X = (i - initX) * pixelScale + offset;

                if (c % 2 == 0)
                {
                    TextBlock textBlock = new TextBlock()
                    {
                        Text = i.ToString(),
                        FontSize = 10,
                        Foreground = new SolidColorBrush(Colors.Red)
                    };
                    Canvas.SetLeft(textBlock, X - 5);
                    Canvas.SetTop(textBlock, 0);
                    canvas.Children.Add(textBlock);
                }

                Line line = new Line()
                {
                    Stroke = new SolidColorBrush(Colors.DarkGray),
                    StrokeThickness = 1,
                    X1 = X,
                    Y1 = offset,
                    X2 = X,
                    Y2 = h + offset
                };

                canvas.Children.Add(line);

                c++;
            }

            c = 0;
            for (int i = initY; i <= endY; i += (endY - initY) / 20)
            {
                int Y = (i - initY) * pixelScale + offset;

                if (c % 2 == 0)
                {
                    TextBlock textBlock = new TextBlock()
                    {
                        Text = (-i).ToString(),
                        FontSize = 10,
                        Foreground = new SolidColorBrush(Colors.BlueViolet)
                    };

                    Canvas.SetLeft(textBlock, 0);
                    Canvas.SetTop(textBlock, Y - 5);
                    canvas.Children.Add(textBlock);
                }

                Line line = new Line()
                {
                    Stroke = new SolidColorBrush(Colors.DarkGray),
                    StrokeThickness = 1,
                    X1 = offset,
                    Y1 = Y,
                    X2 = w + offset,
                    Y2 = Y
                };

                canvas.Children.Add(line);

                c++;
            }

            if (initX <= 0 && endX >= 0)
            {
                int X = offset  - initX * pixelScale ;

                Line line = new Line()
                {
                    Stroke = new SolidColorBrush(Colors.DarkViolet),
                    StrokeThickness = 2,
                    X1 = X,
                    Y1 = offset,
                    X2 = X,
                    Y2 = h + offset
                };

                canvas.Children.Add(line);
            }

            if (initY <= 0 && endY >= 0)
            {
                int Y = offset  - initY * pixelScale;

                Line line = new Line()
                {
                    Stroke = new SolidColorBrush(Colors.DarkViolet),
                    StrokeThickness = 2,
                    X1 = offset,
                    Y1 = Y,
                    X2 = w + offset,
                    Y2 = Y
                };
                canvas.Children.Add(line);
            }

            drawAllPoints();
        }
        private void drawAllPoints()
        {
            (int, int) leftTuple = (middleX - w / 2 / pixelScale, middleY - h / 2 / pixelScale);
            (int, int) rightTuple = (middleX + w / 2 / pixelScale, middleY + h / 2 / pixelScale);

            foreach ((int, int) finalTuple in dots)
            {
                if (finalTuple.Item1 >= leftTuple.Item1 && finalTuple.Item1 < rightTuple.Item1 &&
                    finalTuple.Item2 >= leftTuple.Item2 && finalTuple.Item2 < rightTuple.Item2)
                {
                    int X = (finalTuple.Item1 - leftTuple.Item1) * pixelScale + offset;
                    int Y = (finalTuple.Item2 - leftTuple.Item2) * pixelScale + offset;
                    Rectangle rec = new Rectangle()
                    {
                        Width = pixelScale,
                        Height = pixelScale,
                        Fill = Brushes.Black
                    };
                    canvas.Children.Add(rec);
                    Canvas.SetLeft(rec, X);
                    Canvas.SetTop(rec, Y);
                }
            }
        }
        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int X = ((int)e.GetPosition(this).X - 30) / pixelScale - w / 2 / pixelScale;
            int Y = ((int)e.GetPosition(this).Y - 30) / pixelScale - h / 2 / pixelScale;

            if (e.Delta < 0)
            {
                if (pixelScale != 1)
                {
                    middleX += X;
                    middleY += Y;
                    pixelScale /= 5;
                }
            }
            else
            {
                if (pixelScale < 25)
                {
                    middleX += X;
                    middleY += Y;
                    pixelScale *= 5;
                }
            }

            if (pixelScale == 1)
            {
                middleX = 0;
                middleY = 0;
            }

            canvasPaint();
        }
    }
}