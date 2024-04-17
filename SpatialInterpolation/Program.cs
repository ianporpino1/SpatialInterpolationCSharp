

using System.Collections.Concurrent;

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

        List<Point> results = new List<Point>();

        DateTime startTime = DateTime.Now;

        int numThreads = Environment.ProcessorCount;
        List<Thread> threads = new List<Thread>(numThreads);

        int totalPoints = unknown_points.Count;
        int pointsPerThread = totalPoints / numThreads;
        int extraPoints = totalPoints % numThreads;

        int startIndex = 0;
        for (int i = 0; i < numThreads; i++)
        {
            int endIndex = startIndex + pointsPerThread;
            if (i < extraPoints)
            {
                endIndex++;
            }

            List<Point> subUnknown = unknown_points.GetRange(startIndex, endIndex - startIndex);
            

            ThreadStart threadStart = () =>
            {
                List<Point> z_interpolated = SpatialInterpolation.InverseDistanceWeighting(known_points, subUnknown, 2.0);
                lock (results)
                {
                    foreach (var value in z_interpolated)
                    {
                        results.Add(value);
                    }
                }
            };

            Thread thread = new Thread(threadStart);
            thread.Start();
            threads.Add(thread);

            startIndex = endIndex;
        }

        foreach (Thread thread in threads)
        {
            try
            {
                thread.Join();
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine(e);
            }
        }


        DateTime endTime = DateTime.Now;
        TimeSpan duration = endTime - startTime;

        Console.WriteLine("Tempo de execução: " + duration.TotalSeconds + " segundos"); //357seg ?? 9min

        foreach (Point val in results)
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
