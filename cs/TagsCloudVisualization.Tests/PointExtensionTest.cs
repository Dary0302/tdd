using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.Extension;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class PointExtensionTest
{
    [TestCase(0, 0, 0, 0, 0, 0, Description = "Addition of zero points")]
    [TestCase(1, 1, 1, 1, 2, 2, Description = "Addition of positive points")]
    [TestCase(-1, -1, -1, -1, -2, -2, Description = "Addition of negative points")]
    public void Add_WhenAddPoints_ReturnsSumOfPoints(
        int xPoint1,
        int yPoint1,
        int xPoint2,
        int yPoint2,
        int xResult,
        int yResult)
    {
        var point1 = new Point(xPoint1, yPoint1);
        var point2 = new Point(xPoint2, yPoint2);
        var result = point1.Add(point2);
        result.X.Should().Be(xResult);
        result.Y.Should().Be(yResult);
    }

    [TestCase(0, 0, 0, 0, 0, 0, Description = "Subtract of zero points")]
    [TestCase(1, 1, 1, 1, 0, 0, Description = "Subtract of positive points")]
    [TestCase(-1, -1, -1, -1, 0, 0, Description = "Subtract of negative points")]
    public void Subtract_WhenSubtractPoints_ReturnsDifferenceOfPoints(
        int xPoint1,
        int yPoint1,
        int xPoint2,
        int yPoint2,
        int xResult,
        int yResult)
    {
        var point1 = new Point(xPoint1, yPoint1);
        var point2 = new Point(xPoint2, yPoint2);
        var result = point1.Subtract(point2);
        result.X.Should().Be(xResult);
        result.Y.Should().Be(yResult);
    }

    [TestCase(0, 0, 10, 12, 5, 6, Description = "Rectangle with zero location")]
    [TestCase(5, 6, 10, 12, 10, 12, Description = "Rectangle with positive location")]
    [TestCase(-5, -6, 10, 12, 0, 0, Description = "Rectangle with negative location")]
    [TestCase(0, 0, 3, 2, 1, 1, Description = "Rectangle with odd width")]
    [TestCase(0, 0, 2, 3, 1, 1, Description = "Rectangle with odd height")]
    public void GetCenter_ReturnsCenterOfRectangle(int x, int y, int width, int height, int xCenter, int yCenter)
    {
        var rectangle = new Rectangle(x, y, width, height);
        var center = rectangle.GetCenter();
        center.X.Should().Be(xCenter);
        center.Y.Should().Be(yCenter);
    }

    [TestCase(5, 5, 10, 10, Description = "Rectangles intersect")]
    [TestCase(0, 0, 10, 10, Description = "Rectangles are equal")]
    [TestCase(0, 0, 5, 5, Description = "Rectangle is inside another rectangle")]
    public void IsIntersectOthersRectangles_WhenRectanglesIntersect(int x, int y, int width, int height)
    {
        var rectangle = new Rectangle(0, 0, 10, 10);
        var rectangles = new List<Rectangle> { new Rectangle(x, y, width, height) };
        rectangle.IsIntersectOthersRectangles(rectangles).Should().BeTrue();
    }

    [TestCase(0, 1, 1, 1, Description = "The rectangle touch at the top")]
    [TestCase(1, 0, 1, 1, Description = "The rectangle touch on the right side")]
    [TestCase(0, -1, 1, 1, Description = "The rectangle touch at the bottom")]
    [TestCase(-1, 0, 1, 1, Description = "The rectangle touch on the left side")]
    [TestCase(1, 1, 1, 1, Description = "The rectangle touch at the top right corner")]
    [TestCase(-1, 1, 1, 1, Description = "The rectangle touch at the top left corner")]
    [TestCase(1, -1, 1, 1, Description = "The rectangle touch at the bottom right corner")]
    [TestCase(-1, -1, 1, 1, Description = "The rectangle touch at the bottom left corner")]
    public void IsIntersectOthersRectangles_WhenRectanglesNotIntersect(int x, int y, int width, int height)
    {
        var rectangle = new Rectangle(0, 0, 1, 1);
        var rectangles = new List<Rectangle> { new Rectangle(x, y, width, height) };
        rectangle.IsIntersectOthersRectangles(rectangles).Should().BeFalse();
    }
}