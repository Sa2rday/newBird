using System;
using System.Collections.Generic;
using System.IO;

struct Coord
{
    public double x;
    public double y;

    public Coord(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
}

struct Obstacle
{
    public Coord position;
    public double width;
    public double height;

    public Obstacle(double x, double y, double width, double height)
    {
        this.position = new Coord(x, y);
        this.width = width;
        this.height = height;
    }

    public bool IsColliding(Coord birdPosition)
    {
        return birdPosition.x >= position.x && birdPosition.x <= position.x + width &&
               birdPosition.y >= position.y && birdPosition.y <= position.y + height;
    }
}

class Bird
{
    public double X { get; set; }
    public double Y { get; set; }
    private double angle;
    private double speed;
    private double resistance;
    private double startX;
    private double startY;
    private List<Coord> trajectory = new List<Coord>();
    private const double g = 9.8; // Ускорение свободного падения
    public Obstacle? obstacle { get; private set; }

    public Bird(double x, double y)
    {
        X = x;
        Y = y;
    }

    public void SetupFlightData(string angle, string speed, string resistance,
                                string obsX, string obsY, string obsWidth, string obsHeight,
                                string strtX, string strtY )
    {
        this.angle = Convert.ToDouble(angle) * Math.PI / 180;
        this.speed = Convert.ToDouble(speed);
        this.resistance = Convert.ToDouble(resistance);
        this.startX = Convert.ToDouble(strtX);
        this.startY = Convert.ToDouble(strtY);

        if (!string.IsNullOrWhiteSpace(obsX) && !string.IsNullOrWhiteSpace(obsY) &&
            !string.IsNullOrWhiteSpace(obsWidth) && !string.IsNullOrWhiteSpace(obsHeight))
        {
            double x = Convert.ToDouble(obsX);
            double y = Convert.ToDouble(obsY);
            double width = Convert.ToDouble(obsWidth);
            double height = Convert.ToDouble(obsHeight);
            this.obstacle = new Obstacle(x, y, width, height);
        }
        else
        {
            this.obstacle = null;
        }
    }

    public void LoadFlightData(string filename)
    {
        try
        {
            string[] data = File.ReadAllText(filename).Split(';');
            SetupFlightData(data[0], data[1], data.Length > 2 ? data[2] : "0",
                            data.Length > 3 ? data[3] : "", data.Length > 4 ? data[4] : "",
                            data.Length > 5 ? data[5] : "", data.Length > 6 ? data[6] : "",
                            data.Length > 7 ? data[7] : "", data.Length > 8 ? data[8] : "");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
        }
    }

    public void Fly()
    {
        double time = 0.0;
        double deltaTime = 0.1;
        double cosa = Math.Cos(angle);
        double sina = Math.Sin(angle);

        while (Y >= 0)
        {
            X += startX + speed * cosa * deltaTime / (1 + resistance * time);
            Y += startY + speed * sina * deltaTime - 0.5 * g * Math.Pow(time, 2) / (1 + resistance * time);
            time += deltaTime;

            trajectory.Add(new Coord(X, Y));

            if (obstacle.HasValue && obstacle.Value.IsColliding(new Coord(X, Y)))
            {
                Console.WriteLine("Столкновение с препятствием.");
                break;
            }

            if (Y < 0)
            {
                Console.WriteLine("Птица достигла земли.");
                break;
            }
        }
    }

    public void PrintWay()
    {
        foreach (var point in trajectory)
        {
            Console.WriteLine($"X: {point.x}, Y: {point.y}");
        }
    }

    // Дополнительный метод для сохранения траектории в файл, если необходимо
    public void SaveWay(string path)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            foreach (var point in trajectory)
            {
                writer.WriteLine($"{point.x}, {point.y}");
            }
        }
    }

    public List<Coord> GetTrajectory()
    {
        return trajectory;
    }
}
