using System.Drawing;

namespace ImageLabeler.Filters
{
    public abstract class AbstractImageFilter
    {
        protected bool IsPointValid(Point point, Bitmap img)
        {
            if (point.X < 0)
                return false;

            if (point.X >= img.Width)
                return false;

            if (point.Y < 0)
                return false;

            if (point.Y >= img.Height)
                return false;

            return true;
        }

        public abstract Bitmap Apply(Bitmap src);
    }
}
