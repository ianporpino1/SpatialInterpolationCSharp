public class Point
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; set;}

    public Point(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public Point(double x, double y)
    {
        X = x;
        Y = y;
        
    }
}