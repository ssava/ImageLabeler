using System;
using System.Drawing;

namespace ImageLabeler.Filters
{
    public class NormalizeFilter : AbstractImageFilter
    {
        UInt32 min,    // Light Color
               max;    // Dark Color

        private static UInt32 GetColor(Bitmap img, int x, int y)
        {
            string sColor = img.GetPixel(x, y).Name.Substring(2);

            return Convert.ToUInt32(sColor, 16);
        }

        public override Bitmap Apply(Bitmap src)
        {
            Bitmap masked;

            masked = new Bitmap(src);
            min = max = GetColor(masked, 0, 0);

            for (int x = 0; x < masked.Width; x++)
            {
                for (int y = 0; y < masked.Height; y++)
                {
                    UInt32 pxColor = GetColor(masked, x, y);

                    if (pxColor <= min)
                        min = pxColor;

                    if (pxColor >= max)
                        max = pxColor;
                }
            }

            for (int x = 0; x < masked.Width; x++)
            {
                for (int y = 0; y < masked.Height; y++)
                {
                    UInt32 pxColor = GetColor(masked, x, y);
                    UInt32 norm = pxColor + (0x00ffffff - max - min);

                    if (pxColor == norm)
                        continue;

                    norm += 0xff000000;

                    masked.SetPixel(x, y, Color.FromName(norm.ToString()));
                }
            }

            return masked;
        }
    }
}
