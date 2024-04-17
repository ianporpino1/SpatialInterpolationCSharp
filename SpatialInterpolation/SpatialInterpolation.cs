using System;
using System.Collections.Generic;

public class SpatialInterpolation
{
    public static List<Point> InverseDistanceWeighting(List<Point> known, List<Point> unknown, double power)
    {
        List<Point> interpolatedPoints = new List<Point>();
        foreach (Point u in unknown)
        {
            double sumWeights = 0.0;
            double sumWeightedValues = 0.0;
            foreach (Point k in known)
            {
                double distance = Math.Sqrt( ((k.X - u.X) * (k.X - u.X) ) + ((k.Y - u.Y) * (k.Y - u.Y)) );
                double weight = 1.0 / Math.Pow(distance, power);
                sumWeights += weight;
                sumWeightedValues += weight * k.Z;
            }
            Point p = new Point(u.X, u.Y, sumWeightedValues / sumWeights);
            interpolatedPoints.Add(p);
        }
        return interpolatedPoints;
    }
}