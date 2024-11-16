using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.RectangleGenerator;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class RectangleGeneratorTest
{
    [Test]
    public void GenerateRandomRectangles_CheckNumberRectangles_NumberRectanglesGeneratedMatchesRequested()
    {
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);
        var rectangles = RectangleGenerator.GenerateRandomRectangles(10).ToList();
        layouter.PutRectangles(rectangles);

        rectangles.Count.Should().Be(10);
    }
}