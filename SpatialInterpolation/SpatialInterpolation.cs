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
                double distance = Math.Sqrt( ((k.X - u.X) * (k.X - u.X) ) + ((k.Y - u.Y) * (k.Y - u.Y)) );
                double weight = 1.0 / Pow(distance, power);
                sumWeights += weight;
                sumWeightedValues += weight * k.Z;
            }
            u.Z = sumWeightedValues / sumWeights;
            return u;
        };
    }
    
    
    
    private static double Pow(double baseNumber, double exponent)
    {
        double result = 1;

        for (int i = 0; i < Math.Abs(exponent); i++)
        {
            result *= baseNumber;
        }

        if (exponent < 0)
        {
            result = 1 / result;
        }

        return result;
    }
}