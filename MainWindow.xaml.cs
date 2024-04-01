using System;
using System.Windows;
using System.Windows.Controls;

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
            bird.LoadFlightData(filename);

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
            bird.SaveWay("output_path.txt");
        }
    }
}
