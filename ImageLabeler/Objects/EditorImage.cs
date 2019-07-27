using System;
using System.Collections.Generic;
using System.Drawing;

namespace ImageLabeler.Objects
{
    public sealed class EditorImage : EditorObject<DataImage>
    {
        public string File => wrapped.File;
        public List<EditorBox> Boxes { get; private set; }

        private readonly Bitmap bmpImage;

        public EditorImage(DataImage image) :
            base(image)
        {
            //string fPath = @"C:\Users\Simone\Desktop\" + File;
            string fPath = File;

            bmpImage = new Bitmap(fPath);
            Boxes = new List<EditorBox>();

            foreach (DataBox b in wrapped.Boxes)
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
            wrapped.Boxes.Add(editorBox.GetDlibObject());
            this.Boxes.Add(editorBox);

            return editorBox;
        }
    }
}
