using System.Drawing;

namespace TagsCloudVisualization.Extension;

public static class PointExtension
{
    public static Point Add(this Point selfPoint, Point otherPoint) =>
        new(selfPoint.X + otherPoint.X, selfPoint.Y + otherPoint.Y);

    public static Point Subtract(this Point selfPoint, Point otherPoint) =>
        new(selfPoint.X - otherPoint.X, selfPoint.Y - otherPoint.Y);

    public static Point GetCenter(this Rectangle rectangle) =>
        new(rectangle.Location.X + rectangle.Width / 2, rectangle.Location.Y + rectangle.Height / 2);

    public static bool IsIntersectOthersRectangles(this Rectangle rectangle, IEnumerable<Rectangle> rectangles) => 
        rectangles.Any(rectangle.IntersectsWith);
}