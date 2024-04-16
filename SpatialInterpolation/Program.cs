

    class Program
{
    static void Main(string[] args)
    {
        string fileKnownPoints = "data/known_points.csv";
        List<double> x_known = new List<double>();
        List<double> y_known = new List<double>();
        List<double> z_known = new List<double>();
        DateTime startTime = DateTime.Now;

        ReadPointsFromFile(fileKnownPoints, x_known, y_known, z_known);

        string fileUnknownPoints = "src/data/unknown_points.csv";
        List<double> x_unknown = new List<double>();
        List<double> y_unknown = new List<double>();

        ReadPointsFromFile(fileUnknownPoints, x_unknown, y_unknown);

        List<double> z_interpolated = SpatialInterpolation.InverseDistanceWeighting(x_known, y_known, z_known, x_unknown, y_unknown, 2.0);

        DateTime endTime = DateTime.Now;
        TimeSpan duration = endTime - startTime;

        Console.WriteLine("Tempo de execução: " + duration.TotalSeconds + " segundos");

        foreach (double val in z_interpolated)
        {
            Console.WriteLine(val);
        }
    }
    static void ReadPointsFromFile(string filePath, List<double> x_list, List<double> y_list,
        List<double> z_list = null)
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
                    x_list.Add(double.Parse(parts[0]));
                    y_list.Add(double.Parse(parts[1]));
                    if (z_list != null)
                    {
                        z_list.Add(double.Parse(parts[2]));
                    }
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Error reading the file: " + e.Message);
        }
    }

}

