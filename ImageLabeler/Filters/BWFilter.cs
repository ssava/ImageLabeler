using System;
using System.Drawing;

namespace ImageLabeler.Filters
{
    public sealed class BWFilter : AbstractImageFilter
    {
        public override Bitmap Apply(Bitmap src)
        {
            Int32 cutThreshold = Convert.ToInt32(Color.FromArgb(128, 128, 128).Name, 16);

            Bitmap masked = new Bitmap(src);

            for (int x = 0; x < masked.Width; x++)
            {
                for (int y = 0; y < masked.Height; y++)
                {
                    Int32 pxColor = Convert.ToInt32(masked.GetPixel(x, y).Name, 16);

                    if (pxColor < cutThreshold)
                        masked.SetPixel(x, y, Color.Black);
                    else
                        masked.SetPixel(x, y, Color.White);
                }
            }

            return masked;
        }
    }
}
