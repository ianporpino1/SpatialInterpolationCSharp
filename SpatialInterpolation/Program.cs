

    class Program
{
    static void Main(string[] args)
    {
        string fileKnownPoints = "data/known_points.csv";
        List<Point> known_points = new List<Point>();
        ReadPointsFromFile(fileKnownPoints, known_points, true);

        string fileUnknownPoints = "data/unknown_points.csv";
        List<Point> unknown_points = new List<Point>();

        ReadPointsFromFile(fileUnknownPoints, unknown_points, false);
        
        DateTime startTime = DateTime.Now;

        List<Point> z_interpolated = SpatialInterpolation.InverseDistanceWeighting(known_points, unknown_points, 2.0);

        DateTime endTime = DateTime.Now;
        TimeSpan duration = endTime - startTime;

        Console.WriteLine("Tempo de execução: " + duration.TotalSeconds + " segundos");

        foreach (Point val in z_interpolated)
        {
            Console.WriteLine(val);
        }
    }
    static void ReadPointsFromFile(string filePath, List<Point> points, Boolean flag)
    {
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                sr.ReadLine();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    Point point;
                    if(flag){
                        point = new Point(double.Parse(parts[0]), double.Parse(parts[1]),double.Parse(parts[2]));
                    }
                    else {
                        point = new Point(double.Parse(parts[0]), double.Parse(parts[1]));
                    }
                    points.Add(point);
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Error reading the file: " + e.Message);
        }
    }

}

