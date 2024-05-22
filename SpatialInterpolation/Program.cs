

using System.Collections.Concurrent;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();

        string fileKnownPoints = "data/known_points.csv";
        List<Point> known_points = new List<Point>();
        ReadPointsFromFile(fileKnownPoints, known_points, true);

        string fileUnknownPoints = "data/unknown_points.csv";
        List<Point> unknown_points = new List<Point>();

        ReadPointsFromFile(fileUnknownPoints, unknown_points, false);

        int numberOfTasks = Environment.ProcessorCount;
        var tasks = new List<Task<List<Point>>>();
        
        int totalPoints = unknown_points.Count;
        int pointsPerTask = totalPoints / numberOfTasks;
        int extraPoints = totalPoints % numberOfTasks;

        int startIndex = 0;
        for (int i = 0; i < numberOfTasks; i++)
        {
            int endIndex = startIndex + pointsPerTask;
            if (i < extraPoints)
            {
                endIndex++;
            }

            List<Point> subUnknown = unknown_points.GetRange(startIndex, endIndex - startIndex);
            

            tasks.Add(Task.Run(() => SpatialInterpolation.InverseDistanceWeighting(known_points, subUnknown, 2.0)));

            startIndex = endIndex;
        }
        
        Task.WhenAll(tasks).Wait();
        
        var results = tasks.SelectMany(t => t.Result).ToList();
        
        stopwatch.Stop();

        Console.WriteLine("Tempo de execução: " + stopwatch.Elapsed + " segundos"); 

        int i1=0;
        foreach (Point val in results)
        {
        	Console.WriteLine(i1 + ":" + val.ToString());
            i1++;
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
