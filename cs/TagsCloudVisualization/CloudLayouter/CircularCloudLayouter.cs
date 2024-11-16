using System.Drawing;
using TagsCloudVisualization.Extension;

namespace TagsCloudVisualization.CloudLayouter;

public class CircularCloudLayouter(Point center) : ILayouter
{
    public readonly List<Rectangle> Rectangles = new();
    private readonly Spiral spiral = new(center, 2);

    public Rectangle PutNextRectangle(Size sizeRectangle)
    {
        var rectangle = FindNextValidRectanglePosition(sizeRectangle);
        rectangle = MoveRectangleCloserCenter(rectangle);
        Rectangles.Add(rectangle);

        return rectangle;
    }

    private Rectangle FindNextValidRectanglePosition(Size sizeRectangle)
    {
        if (sizeRectangle.Width <= 0 || sizeRectangle.Height <= 0)
            throw new ArgumentException("Width and height should be greater than zero.");

        Rectangle rectangle;

        while (true)
        {
            rectangle = new(spiral.GetNextPoint(), sizeRectangle);
            if (!rectangle.IsIntersectOthersRectangles(Rectangles))
                break;
        }
        
        return rectangle;
    }

    private Rectangle MoveRectangleCloserCenter(Rectangle rectangle)
    {
        var newRectangle = MoveRectangleAxis(rectangle,
            Math.Abs(rectangle.GetCenter().X - center.X),
            new(rectangle.GetCenter().X < center.X ? 1 : -1, 0));
        newRectangle = MoveRectangleAxis(newRectangle,
            Math.Abs(rectangle.GetCenter().Y - center.Y),
            new(0, rectangle.GetCenter().Y < center.Y ? 1 : -1));

        return newRectangle;
    }

    public void PutRectangles(IEnumerable<Rectangle> rectangles)
    {
        foreach (var rectangle in rectangles)
        {
            PutNextRectangle(rectangle.Size);
        }
    }

    private Rectangle MoveRectangleAxis(
        Rectangle newRectangle,
        int stepsToCenter,
        Point stepPoint)
    {
        var stepsTaken = 0;
        while (!newRectangle.IsIntersectOthersRectangles(Rectangles) && stepsTaken != stepsToCenter)
        {
            newRectangle.Location = newRectangle.Location.Add(stepPoint);
            stepsTaken++;
        }

        if (newRectangle.IsIntersectOthersRectangles(Rectangles))
        {
            newRectangle.Location = newRectangle.Location.Subtract(stepPoint);
        }

        return newRectangle;
    }
}