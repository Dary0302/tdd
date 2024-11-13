using System.Drawing;
using TagsCloudVisualization.CloudLayouter;

var center = new Point(0, 0);
var layouter = new CircularCloudLayouter(center);
var rectangles = RectangleGenerator.GenerateRandomRectangles(layouter, 50);