using System;
using System.Collections.Generic;

public class SpatialInterpolation
{
    public static Func<Point, Point> InverseDistanceWeightingFunction(List<Point> knownPoints, double power)
    {
        return u =>
        {
            double sumWeights = 0.0;
            double sumWeightedValues = 0.0;

            foreach (var k in knownPoints)
            {
                double distance = Math.Sqrt(Math.Pow(k.X - u.X, 2) + Math.Pow(k.Y - u.Y, 2));
                double weight = 1 / Math.Pow(distance, power);
                sumWeights += weight;
                sumWeightedValues += weight * k.Z;
            }
            u.Z = sumWeightedValues / sumWeights;
            return u;
        };
    }
}