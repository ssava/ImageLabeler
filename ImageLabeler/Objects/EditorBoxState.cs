using System;
using System.Drawing;

namespace ImageLabeler.Objects
{
    public abstract class EditorBoxState
    {
        protected EditorBox box;

        protected EditorBoxState()
        {
        }

        public abstract Bitmap Render(Image prev, EditorBox box);
    }

    public sealed class UnselectedBoxState : EditorBoxState
    {
        public static EditorBoxState Instance { get; private set; }

        static UnselectedBoxState()
        {
            if (Instance == null)
                Instance = new UnselectedBoxState();
        }

        private UnselectedBoxState()
        {

        }

        public override Bitmap Render(Image prev, EditorBox box)
        {
            Bitmap boxMask = new Bitmap(prev);

            using (Graphics g = Graphics.FromImage(boxMask))
            {
                g.DrawRectangle(EditorBox.Pen, box.Rectangle);
                g.DrawString(box.Label, new Font("Verdana", 10.0f), EditorBox.Brush, new PointF(box.Left + box.Width, box.Top));
            }

            return boxMask;
        }
    }

    public sealed class SelectedBoxState : EditorBoxState
    {
        public static EditorBoxState Instance { get; private set; }

        private static Color mSelected = Color.Yellow;
        const int Opacity = 56;
        
        static SelectedBoxState()
        {
            if (Instance == null)
                Instance = new SelectedBoxState();
        }

        private SelectedBoxState()
        {
            
        }

        public override Bitmap Render(Image prev, EditorBox box)
        {
            Bitmap boxMask = new Bitmap(prev);

            Brush b = new SolidBrush(Color.FromArgb(Opacity, mSelected.R, mSelected.G, mSelected.B));

            Brush fc = new SolidBrush(Color.White);
            Pen bc = (Pen)EditorBox.Pen.Clone();

            Rectangle[] anchors = GetAnchors(box);

            bc.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            using (Graphics g = Graphics.FromImage(boxMask))
            {
                g.FillRectangle(b, box.Rectangle);
                g.DrawRectangle(bc, box.Rectangle);
                
                bc.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                g.FillRectangles(fc, anchors);
                g.DrawRectangles(bc, anchors);
            }

            return boxMask;
        }

        private Rectangle[] GetAnchors(EditorBox box)
        {
            /* Anchors */
            int anchorSize = 8; // 10px
            int hAnchorSize = anchorSize / 2;
            int cX = box.Left + (box.Width / 2);
            int cY = box.Top + (box.Height / 2);

            return new Rectangle[]
            {
                // top - left
                new Rectangle(box.Left - hAnchorSize, box.Top - hAnchorSize, anchorSize, anchorSize),

                // top - right
                new Rectangle(box.Left + box.Width - hAnchorSize, box.Top - hAnchorSize, anchorSize, anchorSize),

                // bottom - left
                new Rectangle(box.Left - hAnchorSize, box.Top + box.Height - hAnchorSize, anchorSize, anchorSize),

                // bottom - right
                new Rectangle(box.Left + box.Width - hAnchorSize, box.Top + box.Height - hAnchorSize, anchorSize, anchorSize),

                // top - center
                new Rectangle(cX - hAnchorSize, box.Top - hAnchorSize, anchorSize, anchorSize),

                // bottom - center
                new Rectangle(cX - hAnchorSize, box.Top + box.Height - hAnchorSize, anchorSize, anchorSize),

                // left - center
                new Rectangle(box.Left - hAnchorSize, cY - anchorSize, anchorSize, anchorSize),

                // right - center
                new Rectangle(box.Left + box.Width - hAnchorSize, cY - anchorSize, anchorSize, anchorSize),
            };
        }
    }
}
