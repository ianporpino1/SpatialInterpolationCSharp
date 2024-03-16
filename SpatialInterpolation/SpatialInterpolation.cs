
using System.Collections;

public class SpatialInterpolation
{
    public static List<double> InverseDistanceWeighting(List<double> x_known, List<double> y_known, List<double> z_known, List<double> x_unknown, List<double> y_unknown, double power)
    {
        List<double> z_unknown = new List<double>();
        for (int i = 0; i < x_unknown.Count; i++)
        {
            double sumWeights = 0.0;
            double sumWeightedValues = 0.0;
            for (int j = 0; j < x_known.Count; j++)
            {
                double distance = Math.Sqrt(Math.Pow(x_known[j] - x_unknown[i], 2) + Math.Pow(y_known[j] - y_unknown[i], 2));
                double weight = 1.0 / Math.Pow(distance, power);
                sumWeights += weight;
                sumWeightedValues += weight * z_known[j];
            }
            z_unknown.Add(sumWeightedValues / sumWeights);
        }
        return z_unknown;
    }
}

