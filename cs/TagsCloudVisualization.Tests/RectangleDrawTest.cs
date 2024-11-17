using System.Drawing;
using System.Runtime.InteropServices;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.Draw;
using TagsCloudVisualization.RectangleGenerator;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class RectangleDrawTest
{
    private Point center;
    private CircularCloudLayouter layouter;
    private IEnumerable<Rectangle> rectangles;
    private RectangleDraw rectangleDraw;

    [SetUp]
    public void SetUp()
    {
        center = new Point(0, 0);
        layouter = new CircularCloudLayouter(center);
        rectangles = RectangleGenerator.GenerateRandomRectangles(10);
        layouter.PutRectangles(rectangles);
        rectangleDraw = new RectangleDraw(1500, 1500);
    }

    [TestCase(null)]
    public void CreateImage_OnInvalidParameters_ThrowsArgumentException(string filename)
    {
        var action = () => rectangleDraw.CreateImage(layouter.Rectangles, filename);
        action.Should().Throw<ArgumentException>();
    }

    [TestCase("12\\")]
    [TestCase("@#$\\")]
    public void CreateImage_OnInvalidParameters_ThrowsDirectoryNotFoundException(string filename)
    {
        var action = () => rectangleDraw.CreateImage(layouter.Rectangles, filename);
        action.Should().Throw<DirectoryNotFoundException>();
    }

    [TestCase("abc|123")]
    [TestCase("123|abc")]
    [TestCase("123\n")]
    [TestCase("123\r")]
    [TestCase("\\")]
    [TestCase("")]
    public void CreateImage_OnInvalidParameters_ThrowsExternalException(string filename)
    {
        var action = () => rectangleDraw.CreateImage(layouter.Rectangles, filename);
        action.Should().Throw<ExternalException>();
    }

    [Test]
    public void CreateImage_WhenListOfRectanglesIsNull_ThrowsArgumentException()
    {
        var action = () => rectangleDraw.CreateImage(null, "test");
        action.Should().Throw<NullReferenceException>();
    }

    [TestCase(-1, 1)]
    [TestCase(1, -1)]
    [TestCase(1, 0)]
    [TestCase(0, 1)]
    public void Constructor_OnInvalidArguments_ThrowsArgumentException(int width, int height)
    {
        Action action = () => new RectangleDraw(width, height);
        action.Should().Throw<ArgumentException>();
    }
}