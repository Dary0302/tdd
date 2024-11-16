using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.RectangleGenerator;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class SpiralTest
{
    [TestCase(0, 0, 1)]
    [TestCase(2, 10, 2)]
    public void Constructor_WithCorrectParameters_NotThrow(int x, int y, double step)
    {
        var action = () => new Spiral(new(x, y), step);
        action.Should().NotThrow<ArgumentException>();
    }

    [Test]
    public void GetNextPoint_WithCorrectParameters_ReturnsCorrectPoint()
    {
        var spiral = new Spiral(new(0, 0), 1);
        spiral.GetNextPoint().Should().Be(new Point(0, 0));
        spiral.GetNextPoint().Should().Be(new Point((int)(Math.PI / 40 * Math.Cos(Math.PI / 40)),
            (int)(Math.PI / 40 * Math.Sin(Math.PI / 40))));
    }

    [Test]
    public void GetNextPoint_WithCorrectParameters_ReturnsCentralPoint()
    {
        var spiral = new Spiral(new(1, 2), 1);
        spiral.GetNextPoint().Should().Be(new Point(1, 2));
    }

    [TestCase(0, 0, 10, Description = "Center at the center of the coordinate axis")]
    [TestCase(0, 0, 100, Description = "Center at the center of the coordinate axis")]
    [TestCase(0, 0, 1000, Description = "Center at the center of the coordinate axis")]
    [TestCase(100, 100, 10, Description = "Center in the first quadrant of the coordinate axis")]
    [TestCase(100, 100, 100, Description = "Center in the first quadrant of the coordinate axis")]
    [TestCase(100, 100, 1000, Description = "Center in the first quadrant of the coordinate axis")]
    [TestCase(-100, 100, 10, Description = "Center in the second quadrant of the coordinate axis")]
    [TestCase(-100, 100, 100, Description = "Center in the second quadrant of the coordinate axis")]
    [TestCase(-100, 100, 1000, Description = "Center in the second quadrant of the coordinate axis")]
    [TestCase(-100, -100, 10, Description = "Center in the third quadrant of the coordinate axis")]
    [TestCase(-100, -100, 100, Description = "Center in the third quadrant of the coordinate axis")]
    [TestCase(-100, -100, 1000, Description = "Center in the third quadrant of the coordinate axis")]
    [TestCase(-100, 100, 10, Description = "Center in the fourth quadrant of the coordinate axis")]
    [TestCase(-100, 100, 100, Description = "Center in the fourth quadrant of the coordinate axis")]
    [TestCase(-100, 100, 1000, Description = "Center in the fourth quadrant of the coordinate axis")]
    public void RoundLayout_WithDifferentNumberRectanglesAndDifferentCenters(
        int xCenter,
        int yCenter,
        int countRectangles)
    {
        var center = new Point(xCenter, yCenter);
        var randomRectangles = RectangleGenerator.GenerateRandomRectangles(countRectangles);
        var layouter = new CircularCloudLayouter(center);
        layouter.PutRectangles(randomRectangles);

        var maxX = GetMaxX(layouter.Rectangles);
        var minX = GetMinX(layouter.Rectangles);
        var maxY = GetMaxY(layouter.Rectangles);
        var minY = GetMinY(layouter.Rectangles);

        var errorRate = Math.Abs(maxX - center.X + minX - center.X + maxY - center.Y + minY - center.Y);
        errorRate.Should().BeInRange(0, 70);
    }

    private static int GetMaxX(IEnumerable<Rectangle> rectangles) => rectangles.Max(r => r.Location.X);

    private static int GetMaxY(IEnumerable<Rectangle> rectangles) => rectangles.Max(r => r.Location.Y);

    private static int GetMinX(IEnumerable<Rectangle> rectangles) => rectangles.Min(r => r.Location.X);

    private static int GetMinY(IEnumerable<Rectangle> rectangles) => rectangles.Min(r => r.Location.Y);
}