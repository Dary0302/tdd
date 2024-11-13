using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTest
{
    private Point center;
    private static CircularCloudLayouter? layouter;
    private readonly Random random = new(1);
    private readonly List<Rectangle> rectangles = new();

    [SetUp]
    public void SetUp()
    {
        center = new Point(0, 0);
        layouter = new CircularCloudLayouter(center);
    }

    [TestCase(0, 0)]
    [TestCase(-1, -1)]
    public void PutNextRectangle_WhenIncorrectSize_Throw(int sizeX, int sizeY)
    {
        Action action = () => layouter?.PutNextRectangle(new Size(sizeX, sizeY));
        action.Should().Throw<ArgumentException>().WithMessage("Width and height should be greater than zero.");
    }

    [TestCase(1, 1, 10)]
    public void PutNextRectangle_AddRectangles_ReturnTrue(int sizeX, int sizeY, int count)
    {
        for (var i = 0; i < count; i++)
        {
            layouter?.PutNextRectangle(new Size(sizeX, sizeY));
        }

        layouter?.Rectangles.Count.Should().Be(count);
    }

    [TestCase(30)]
    [TestCase(50)]
    [TestCase(100)]
    [TestCase(1000)]
    public void PlacedRectangles_WhenCorrectNotIntersects_ReturnTrue(int countRectangles)
    {
        var isIntersectsWith = 0;

        for (var i = 0; i < countRectangles; i++)
        {
            var size = new Size(random.Next(20, 100), random.Next(40, 100));
            var rectangle = layouter!.PutNextRectangle(size);
            if (rectangle.IsNotIntersectOthersRectangles(rectangles))
            {
                isIntersectsWith += 1;
            }
            rectangles.Add(rectangle);
        }
        isIntersectsWith.Should().Be(countRectangles);

        rectangles.Clear();
    }
}