using System;
using System.Collections.Generic;
using System.Drawing;

namespace ImageLabeler.Objects
{
    public sealed class EditorImage : EditorObject<DataImage>
    {
        public string File => DlibObject.File;
        public List<EditorBox> Boxes { get; private set; }

        private readonly Bitmap bmpImage;

        public EditorImage(DataImage image) :
            base(image)
        {
            //string fPath = @"C:\Users\Simone\Desktop\" + File;
            string fPath = File;

            bmpImage = new Bitmap(fPath);
            Boxes = new List<EditorBox>();

            foreach (DataBox b in DlibObject.Boxes)
                Boxes.Add(new EditorBox(b));
        }

        public Image Render()
        {
            Bitmap rendered = (Bitmap)bmpImage.Clone();

            //rendered = new ImageLabeler.Filters.SobelFilter().Apply(rendered, 0.8);

            try
            {
                foreach (EditorBox box in Boxes)
                    rendered = box.Render(rendered);
            }
            catch (Exception)
            {
            }

            return rendered;
        }

        public override string ToString()
        {
            return File;
        }

        public EditorBox GetBox(int x, int y)
        {

            return Boxes.Find(b => b.Rectangle.Contains(x, y));
        }

        public EditorBox AddBox(EditorBox editorBox)
        {
            DlibObject.Boxes.Add(editorBox.DlibObject);
            this.Boxes.Add(editorBox);

            return editorBox;
        }

        public Image GetImage(EditorBox box)
        {
            Bitmap region = new Bitmap(box.Width, box.Height);

            for (int i = box.Left; i < (box.Left + box.Width); i++)
            {
                for (int j = box.Top; j < (box.Top +  box.Height); j++)
                {
                    region.SetPixel(i - box.Left, j - box.Top, bmpImage.GetPixel(i, j));
                }
            }

            return region;
        }
    }
}
