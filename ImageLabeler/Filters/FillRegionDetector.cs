using System.Collections.Generic;
using System.Drawing;

namespace ImageLabeler.Filters
{
    public class FillRegionDetector : AbstractImageFilter
    {
        int minX, maxX,
            minY, maxY;

        Queue<Point> to_be_visited;

        Point point;

        public Rectangle Detected { get; private set; }

        public FillRegionDetector(Point point)
        {
            this.point = point;
            to_be_visited = new Queue<Point>();
        }

        public override Bitmap Apply(Bitmap src)
        {
            int width = 0;
            int height = 0;
            Bitmap bmpMask = new Bitmap(src);

            minX = maxX = point.X;
            minY = maxY = point.Y;

            to_be_visited.Enqueue(point);

            while (to_be_visited.Count > 0)
                BuildRectangle(bmpMask, to_be_visited.Dequeue(), to_be_visited);

            width = maxX - minX;
            height = maxY - minY;

            bmpMask.Dispose();

            Detected = new Rectangle(minX, minY, width, height);

            return src;
        }

        private void BuildRectangle(Bitmap img, Point point, Queue<Point> to_be_visited)
        {
            if (!IsPointValid(point, img))
                return;


            if (!img.GetPixel(point.X, point.Y).ToArgb().Equals(Color.White.ToArgb()))
                return;

            img.SetPixel(point.X, point.Y, Color.Red);

            if (point.X < minX)
                minX = point.X;

            if (point.X >= maxX)
                maxX = point.X;

            if (point.Y < minY)
                minY = point.Y;

            if (point.Y >= maxY)
                maxY = point.Y;

            int x = point.X,
                y = point.Y;

            to_be_visited.Enqueue(new Point(x - 1, y));        // left
            to_be_visited.Enqueue(new Point(x - 1, y - 1));    // top-left
            to_be_visited.Enqueue(new Point(x, y - 1));        // top
            to_be_visited.Enqueue(new Point(x + 1, y - 1));    // top-right
            to_be_visited.Enqueue(new Point(x + 1, y));        // right
            to_be_visited.Enqueue(new Point(x + 1, y + 1));    // bottom-right
            to_be_visited.Enqueue(new Point(x, y + 1));        // bottom
            to_be_visited.Enqueue(new Point(x - 1, y + 1));    // bottom-left
        }
    }
}
