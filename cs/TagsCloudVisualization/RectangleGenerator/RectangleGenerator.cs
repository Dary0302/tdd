using System.Drawing;

namespace TagsCloudVisualization.RectangleGenerator;

public static class RectangleGenerator
{
    public static IEnumerable<Rectangle> GenerateRandomRectangles(int countRectangles)
    {
        var rectangles = new List<Rectangle>(countRectangles);
        var random = new Random(1);
        
        for (var i = 0; i < countRectangles; i++)
        {
            var size = new Size(random.Next(20,100), random.Next(10, 40));
            rectangles.Add(new Rectangle(new Point(), size));
        }

        return rectangles;
    }
}