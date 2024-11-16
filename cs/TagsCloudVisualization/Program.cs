using System.Drawing;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.RectangleGenerator;

var center = new Point(0, 0);
var randomRectangles = RectangleGenerator.GenerateRandomRectangles(50);
var layouter = new CircularCloudLayouter(center);
layouter.PutRectangles(randomRectangles);