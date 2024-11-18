using System.Drawing;

namespace TagsCloudVisualization.Extension;

public static class RectangleExtension
{
    public static Point GetCenter(this Rectangle rectangle) =>
        new(rectangle.Location.X + rectangle.Width / 2, rectangle.Location.Y + rectangle.Height / 2);

    public static bool IntersectsWithAnyOf(this Rectangle rectangle, IEnumerable<Rectangle> rectangles) => 
        rectangles.Any(rectangle.IntersectsWith);
}