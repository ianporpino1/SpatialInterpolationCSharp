

using System.Collections.Concurrent;

class Program
{
    static void Main(string[] args)
    {

        string fileKnownPoints = "data/known_points.csv";
        List<double> x_known = new List<double>();
        List<double> y_known = new List<double>();
        List<double> z_known = new List<double>();

        DateTime startReadingFile = DateTime.Now;

        ReadPointsFromFile(fileKnownPoints, x_known, y_known, z_known);

        DateTime endReadingFile = DateTime.Now;
        TimeSpan durationReading = endReadingFile - startReadingFile;

        Console.WriteLine("Tempo de execução lendo arquivo: " + durationReading.TotalSeconds + " segundos");
        Console.WriteLine(x_known.Count);


        string fileUnknownPoints = "data/unknown_points.csv";
        List<double> x_unknown = new List<double>();
        List<double> y_unknown = new List<double>();

        ReadPointsFromFile(fileUnknownPoints, x_unknown, y_unknown);


        List<double> results = new List<double>();

        DateTime startTime = DateTime.Now;

        int numThreads = Environment.ProcessorCount;
        List<Thread> threads = new List<Thread>(numThreads);

        int totalPoints = x_unknown.Count;
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

            List<double> subXUnknown = x_unknown.GetRange(startIndex, endIndex - startIndex);
            List<double> subYUnknown = y_unknown.GetRange(startIndex, endIndex - startIndex);

            ThreadStart threadStart = () =>
            {
                List<double> z_interpolated =
                    SpatialInterpolation.InverseDistanceWeighting(x_known, y_known, z_known, subXUnknown, subYUnknown,
                        2.0);

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

        Console.WriteLine("Tempo de execução: " + duration.TotalSeconds + " segundos"); //547seg ?? 9min

        //foreach (double val in results)
        //{
        //Console.WriteLine(val);
        //}
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