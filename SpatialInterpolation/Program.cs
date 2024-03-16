

    class Program
{
    static void Main(string[] args)
    {
        string fileKnownPoints = "data/known_points.csv";
        List<double> x_known = new List<double>();
        List<double> y_known = new List<double>();
        List<double> z_known = new List<double>();
        DateTime startTime = DateTime.Now;

        try
        {
            using (StreamReader sr = new StreamReader(fileKnownPoints))
            {
                sr.ReadLine();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    x_known.Add(double.Parse(parts[0]));
                    y_known.Add(double.Parse(parts[1]));
                    z_known.Add(double.Parse(parts[2]));
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Error reading the file: " + e.Message);
        }

        string fileUnknownPoints = "src/data/unknown_points.csv";
        List<double> x_unknown = new List<double>();
        List<double> y_unknown = new List<double>();

        try
        {
            using (StreamReader sr = new StreamReader(fileUnknownPoints))
            {
                sr.ReadLine();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    x_unknown.Add(double.Parse(parts[0]));
                    y_unknown.Add(double.Parse(parts[1]));
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Error reading the file: " + e.Message);
        }

      

        List<double> z_interpolated = SpatialInterpolation.InverseDistanceWeighting(x_known, y_known, z_known, x_unknown, y_unknown, 2.0);

        DateTime endTime = DateTime.Now;
        TimeSpan duration = endTime - startTime;

        Console.WriteLine("Tempo de execução: " + duration.TotalSeconds + " segundos");

        foreach (double val in z_interpolated)
        {
            Console.WriteLine(val);
        }
    }
}

