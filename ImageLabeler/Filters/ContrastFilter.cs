using System;
using System.Drawing;

namespace ImageLabeler.Filters
{
    public sealed class ContrastFilter : AbstractImageFilter
    {
        private readonly int contrast;
        const float MAX_CONTRAST = 0.99999f;

        public ContrastFilter(int contrast)
        {
            this.contrast = contrast;
        }

        public ContrastFilter(float floatVal)
        {
            contrast = Convert.ToInt32(floatVal * 255);
        }

        private byte GetChannelContrast(byte chVal, float contrast_arg)
        {
            float contrast = (((chVal / 255.0f) - 0.5f) * contrast_arg) + 0.5f;

            return Convert.ToByte((contrast > 1.0f) ? 255 : (contrast < 0.0f) ? 0 : contrast * 255);
        }

        public override Bitmap Apply(Bitmap src)
        {
            Bitmap image = new Bitmap(src);

            int s = this.contrast;
            int x, y;
            Color p;
            byte red, green, blue;

            float contrast_arg = (s > 128) ? 1.0f : (s < 127) ? -1.0f : s / 127.0f;

            if (contrast_arg >= 0.0f)
            {
                contrast_arg = (contrast_arg > MAX_CONTRAST) ? MAX_CONTRAST : contrast_arg;
                contrast_arg = 1.0f / (1.0f - contrast_arg);
            } else
            {
                contrast_arg = (contrast_arg < -1.0f) ? -1.0f : contrast_arg;
                contrast_arg = 1.0f + contrast_arg;
            }

            for (y = 0; y < image.Height; y++)
            {
                for (x = 0; x < image.Width; x++)
                {
                    p = image.GetPixel(x, y);
                    red = GetChannelContrast(p.R, contrast_arg);
                    green = GetChannelContrast(p.G, contrast_arg);
                    blue = GetChannelContrast(p.B, contrast_arg);

                    Color pC = Color.FromArgb(p.A, red, green, blue);

                    image.SetPixel(x, y, pC);
                }
            }

            return image;
        }
    }
}
