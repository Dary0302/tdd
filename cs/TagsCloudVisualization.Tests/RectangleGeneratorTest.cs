using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class RectangleGeneratorTest
{
    [Test]
    public void GenerateRandomRectangles_CheckNumberRectangles_ReturnTrue()
    {
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);
        var rectangles = RectangleGenerator.GenerateRandomRectangles(layouter, 10).ToList();

        rectangles.Count.Should().Be(10);
    }
    
    [Test]
    public void GenerateRandomRectangles_CheckIntersectRectangles_ReturnFalse()
    {
        var center = new Point(0, 0);
        var layouter = new CircularCloudLayouter(center);
        var rectangles = RectangleGenerator.GenerateRandomRectangles(layouter, 10).ToList();

        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
            }
        }
    }
}