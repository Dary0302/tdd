using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.Draw;
using TagsCloudVisualization.Extension;
using TagsCloudVisualization.RectangleGenerator;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTest
{
    private List<Rectangle> rectanglesForCrashTest = new();

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            var drawer = new RectangleDraw(1500, 1500);
            var filename = $"{TestContext.CurrentContext.WorkDirectory}CrashTest.png";
            drawer.CreateImage(rectanglesForCrashTest, filename);
                
            Console.WriteLine($"Tag cloud visualization saved to file {filename}");
        }
    }
    
    [Test]
    public void PutNextRectangle_ReturnsFirstRectangleThatIsInCenterOfCloud()
    {
        var center = new Point(1, 1);
        var layouter = new CircularCloudLayouter(center);
        var nextRectangle = layouter.PutNextRectangle(new Size(1, 1));
        rectanglesForCrashTest.Add(nextRectangle);
        nextRectangle.Location.Should().Be(center);
    }

    [TestCase(0, 0)]
    [TestCase(-1, -1)]
    public void PutNextRectangle_WhenIncorrectSize_Throw(int sizeX, int sizeY)
    {
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);

        Action action = () => layouter.PutNextRectangle(new Size(sizeX, sizeY));
        action.Should().Throw<ArgumentException>().WithMessage("Width and height should be greater than zero.");
    }

    [TestCase(1, 1, 10)]
    public void PutNextRectangle_AddRectangles(int sizeX, int sizeY, int count)
    {
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);

        for (var i = 0; i < count; i++)
        {
            rectanglesForCrashTest.Add(layouter.PutNextRectangle(new Size(sizeX, sizeY)));
        }

        layouter.Rectangles.Count.Should().Be(count);
    }

    [Test]
    public void PutNextRectangle_CheckIntersectRectangles_ReturnFalse()
    {
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);
        var rectangles = RectangleGenerator.GenerateRandomRectangles(10).ToList();
        layouter.PutRectangles(rectangles);
        rectanglesForCrashTest = layouter.Rectangles;

        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                layouter.Rectangles[i].IntersectsWith(layouter.Rectangles[j]).Should().BeFalse();
            }
        }
    }

    [TestCase(30)]
    [TestCase(50)]
    [TestCase(100)]
    [TestCase(1000)]
    public void PlacedRectangles_WhenCorrectNotIntersects_ReturnTrue(int countRectangles)
    {
        var isIntersectsWith = 0;
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);
        var rectangles = new List<Rectangle>();
        var randomRectangles = RectangleGenerator.GenerateRandomRectangles(countRectangles).ToList();

        foreach (var randomRectangle in randomRectangles)
        {
            var rectangle = layouter.PutNextRectangle(randomRectangle.Size);
            if (!rectangle.IsIntersectOthersRectangles(rectangles))
            {
                isIntersectsWith += 1;
            }
            rectangles.Add(rectangle);
            rectanglesForCrashTest.Add(rectangle);
        }

        isIntersectsWith.Should().Be(countRectangles);
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
    public void RectanglesDensity_WithDifferentNumberRectanglesAndDifferentCenters(
        int xCenter,
        int yCenter,
        int countRectangles)
    {
        var center = new Point(xCenter, yCenter);
        var randomRectangles = RectangleGenerator.GenerateRandomRectangles(countRectangles);
        var layouter = new CircularCloudLayouter(center);
        layouter.PutRectangles(randomRectangles);
        rectanglesForCrashTest = layouter.Rectangles;

        var distancesFromCenterToRectangles = GetDistancesFromCenterToRectangles(layouter.Rectangles, center);
        var averageDistanceFromCenterToRectangles = distancesFromCenterToRectangles.Average();
        var mostDistantCoordinateFromCenter = GetMostDistantCoordinateFromCenter(layouter.Rectangles, center);
        var radiusOfCircleAroundRectangles = GetDistanceFromPointToPoint(mostDistantCoordinateFromCenter, center);
        var areaOfCircle = GetAreaOfCircle(radiusOfCircleAroundRectangles);
        var areaOfRectangles = GetAreaOfRectangles(layouter);
        var densityCoefficient = GetDensityCoefficient(areaOfRectangles, areaOfCircle, averageDistanceFromCenterToRectangles, radiusOfCircleAroundRectangles);
        
        densityCoefficient.Should().BeLessThan(0.33);
    }

    private static IEnumerable<double> GetDistancesFromCenterToRectangles(
        IEnumerable<Rectangle> rectangles,
        Point center) =>
        rectangles
            .Select(r => Math.Sqrt(Math.Pow(r.Location.X - center.X, 2) + Math.Pow(r.Location.Y - center.Y, 2)));

    private static int GetMostDistantCoordinateFromCenter(IEnumerable<Rectangle> rectangles, Point center) =>
        rectangles
            .Select(r => Math.Max(Math.Abs(r.Location.X - center.X), Math.Abs(r.Location.Y - center.Y)))
            .Max();

    private static double GetDistanceFromPointToPoint(int mostDistantCoordinateFromCenter, Point center) =>
        Math.Sqrt(Math.Pow(mostDistantCoordinateFromCenter - center.X, 2) +
            Math.Pow(mostDistantCoordinateFromCenter - center.Y, 2));

    private static double GetAreaOfCircle(double radius) => Math.PI * Math.Pow(radius, 2);

    private static int GetAreaOfRectangles(CircularCloudLayouter layouter) =>
        layouter.Rectangles.Sum(r => r.Width * r.Height);

    private static double GetDensityCoefficient(
        double areaOfRectangles,
        double areaOfCircle,
        double averageDistance,
        double radius) =>
        areaOfRectangles / areaOfCircle * (1 - averageDistance / radius);
}