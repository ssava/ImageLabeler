using System;
using System.Drawing;

namespace ImageLabeler.Objects
{
    public sealed class EditorBoxStateChangedEventArgs : EventArgs
    {
        public EditorBoxState State { get; private set; }

        public EditorBoxStateChangedEventArgs(EditorBoxState state)
        {
            State = state;
        }
    }

    public sealed class EditorBox : EditorObject<DataBox>
    {
        public static Brush Brush = new SolidBrush(Color.DarkBlue);
        public static Pen Pen = new Pen(Brush);
        public static EditorBox None = null;

        public event EventHandler<EditorBoxStateChangedEventArgs> StateChanged;

        public Rectangle Rectangle { get; private set; }
        public bool IsSelected { get; private set; }

        public int Top => Rectangle.Top;
        public int Left => Rectangle.Left;
        public int Width => Rectangle.Width;
        public int Height => Rectangle.Height;
        public string Label => wrapped.Label;

        public EditorBoxState mState;

        public EditorBox(DataBox box)
            : base(box)
        {
            Rectangle = new Rectangle(box.Left, box.Top, box.Width, box.Height);
            mState = UnselectedBoxState.Instance;
        }

        public Bitmap Render(Image img)
        {
            return mState.Render(img, this);
        }

        public override string ToString()
        {
            return wrapped.ToString();
        }

        public Bitmap ChangeState(EditorImage img)
        {
            if (mState is UnselectedBoxState)
            {
                mState = SelectedBoxState.Instance;
                IsSelected = true;
            }
            else
            {
                mState = UnselectedBoxState.Instance;
                IsSelected = false;
            }

            StateChanged?.Invoke(this, new EditorBoxStateChangedEventArgs(mState));

            return Render(mState.Render(img.Render(), this));
        }

        public void SetRectangle(Rectangle newBox)
        {
            Rectangle = newBox;

            wrapped.Top = this.Top;
            wrapped.Left = this.Left;
            wrapped.Width = this.Width;
            wrapped.Height = this.Height;
        }

        public void SetLabel(string label)
        {
            wrapped.Label = label;
        }
    }
}