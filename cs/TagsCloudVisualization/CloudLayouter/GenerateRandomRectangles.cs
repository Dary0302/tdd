using System.Drawing;

namespace TagsCloudVisualization.CloudLayouter;

public static class RectangleGenerator
{
    public static IEnumerable<Rectangle> GenerateRandomRectangles(CircularCloudLayouter layouter, int count)
    {
        var rectangles = new List<Rectangle>(count);
        var random = new Random(1);
        
        for (var i = 0; i < count; i++)
        {
            var size = new Size(random.Next(20,100), random.Next(20, 80));
            rectangles.Add(layouter.PutNextRectangle(size));
        }

        return rectangles;
    }
}