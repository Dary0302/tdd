using System.Drawing;

namespace TagsCloudVisualization.CloudLayouter;

public class CircularCloudLayouter(Point center) : ILayouter
{
    public readonly List<Rectangle> Rectangles = new();
    private readonly Spiral spiral = new(center, 2);

    public Rectangle PutNextRectangle(Size sizeRectangle)
    {
        if (sizeRectangle.Width <= 0 || sizeRectangle.Height <= 0)
            throw new ArgumentException("Width and height should be greater than zero.");

        Rectangle rectangle;

        while (true)
        {
            rectangle = new(spiral.GetNextPoint(), sizeRectangle);
            if (rectangle.IsNotIntersectOthersRectangles(Rectangles))
                break;
        }
        rectangle = MoveRectangleCloserCenter(rectangle);
        Rectangles.Add(rectangle);

        return rectangle;
    }

    private Rectangle MoveRectangleCloserCenter(Rectangle rectangle)
    {
        var newRectangle = MoveRectangleAxis(rectangle, rectangle.GetCenter().X, center.X,
            new(rectangle.GetCenter().X < center.X ? 1 : -1, 0));
        newRectangle = MoveRectangleAxis(newRectangle, newRectangle.GetCenter().Y, center.Y,
            new(0, rectangle.GetCenter().Y < center.Y ? 1 : -1));
        
        return newRectangle;
    }

    private Rectangle MoveRectangleAxis(
        Rectangle newRectangle,
        int currentPosition,
        int desiredPosition,
        Point stepPoint)
    {
        while (newRectangle.IsNotIntersectOthersRectangles(Rectangles) && desiredPosition != currentPosition)
        {
            currentPosition += currentPosition < desiredPosition ? 1 : -1;
            newRectangle.Location = newRectangle.Location.Add(stepPoint);
        }

        if (!newRectangle.IsNotIntersectOthersRectangles(Rectangles))
        {
            newRectangle.Location = newRectangle.Location.Subtract(stepPoint);
        }

        return newRectangle;
    }
}