using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.Generic;

namespace BirdFlightSimulator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void StartSimulation(object sender, RoutedEventArgs e)
        {
            // Создаем экземпляр класса Bird
            Bird bird = new Bird(0, 0);

            // Путь к файлу с данными по умолчанию
            string filename = "default_flight_data.txt";
            try
            {
                bird.LoadFlightData(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при загрузке данных полета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверяем, заполнены ли поля ввода
            if (string.IsNullOrWhiteSpace(((TextBox)FindName("Angle")).Text) ||
                string.IsNullOrWhiteSpace(((TextBox)FindName("Speed")).Text) ||
                string.IsNullOrWhiteSpace(((TextBox)FindName("Resistance")).Text) ||
                string.IsNullOrWhiteSpace(((TextBox)FindName("ObstacleX")).Text) ||
                string.IsNullOrWhiteSpace(((TextBox)FindName("ObstacleY")).Text) ||
                string.IsNullOrWhiteSpace(((TextBox)FindName("ObstacleWidth")).Text) ||
                string.IsNullOrWhiteSpace(((TextBox)FindName("ObstacleHeight")).Text) ||
                string.IsNullOrWhiteSpace(((TextBox)FindName("StartX")).Text) ||
                string.IsNullOrWhiteSpace(((TextBox)FindName("StartY")).Text))
            {
                // Если пользователь не ввел данные, читаем их из файла

            }
            else
            {
                // Если пользователь ввел данные, используем их
                bird.SetupFlightData(((TextBox)FindName("Angle")).Text,
                                     ((TextBox)FindName("Speed")).Text,
                                     ((TextBox)FindName("Resistance")).Text,
                                     ((TextBox)FindName("ObstacleX")).Text,
                                     ((TextBox)FindName("ObstacleY")).Text,
                                     ((TextBox)FindName("ObstacleWidth")).Text,
                                     ((TextBox)FindName("ObstacleHeight")).Text,
                                     ((TextBox)FindName("StartX")).Text,
                                     ((TextBox)FindName("StartY")).Text);
            }

            // Запускаем моделирование полета
            bird.Fly();

            // Выводим результат в консоль/лог/интерфейс и т.д. В зависимости от требований.
            bird.PrintWay();

            // Опционально, сохраняем результат в файл
            string outputPath = "output_path.txt";
            bird.SaveWay(outputPath);

            try
            {
                string resultText = File.ReadAllText(outputPath);
                MessageBox.Show($"Симуляция успешно завершена!\nРезультаты:\n{resultText}", "Симуляция завершена", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при чтении результата симуляции: {ex.Message}", "Ошибка чтения файла", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                DrawFlightPath(bird.GetTrajectory());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DrawFlightPath(List<Coord> trajectory)
        {
            double x1, y1, x2, y2;
            flightCanvas.Children.Clear(); // Очищаем канвас перед рисованием
            for (int i = 1; i < trajectory.Count; i++) // Увеличиваем на 20, чтобы было видно в Canvas
            {
                x1 = Math.Round(trajectory[i - 1].x * 20);
                if (double.IsNaN(x1) || double.IsInfinity(x1))
                {
                    x1 = 0;
                }
                y1 = Math.Round(flightCanvas.ActualHeight - trajectory[i - 1].y * 20); // Инвертировать Y
                if (double.IsNaN(y1) || double.IsInfinity(y1))
                {
                    y1 = 0;
                }
                x2 = Math.Round(trajectory[i].x * 20);
                if (double.IsNaN(x2) || double.IsInfinity(x2))
                {
                    x2 = 0;
                }
                y2 = Math.Round(flightCanvas.ActualHeight - trajectory[i].y * 20);
                if (double.IsNaN(y2) || double.IsInfinity(y2))
                {
                    y2 = 0;
                }

                Line line = new Line
                {
                    Stroke = Brushes.Black,
                    X1 = x1,
                    Y1 = y1, 
                    X2 = x2,
                    Y2 = y2,
                    StrokeThickness = 2
                };

                flightCanvas.Children.Add(line);
            }
        }
    }
    
}
