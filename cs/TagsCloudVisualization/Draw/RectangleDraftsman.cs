using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Draw;

public class RectangleDraftsman
{
    private readonly Bitmap bitmap;
    private readonly Size shiftToBitmapCenter;

    public RectangleDraftsman(int width, int height)
    {
        #pragma warning disable CA1416
        bitmap = new(width, height);
        shiftToBitmapCenter = new Size(bitmap.Width / 2, bitmap.Height / 2);
        #pragma warning restore CA1416
    }

    public void CreateImage(IEnumerable<Rectangle> rectangles)
    {
        #pragma warning disable CA1416
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        foreach (var r in rectangles)
        {
            var rectangle = new Rectangle(r.Location + shiftToBitmapCenter, r.Size);
            graphics.DrawRectangle(new Pen(Color.BlueViolet), rectangle);
        }
        #pragma warning restore CA1416
    }
    
    public void SaveImageToFile(string filename)
    {
        #pragma warning disable CA1416
        bitmap.Save(filename, ImageFormat.Png);
        #pragma warning restore CA1416
        Console.WriteLine($"Tag cloud visualization saved to: {Path.GetFullPath(filename)}");
    }
}