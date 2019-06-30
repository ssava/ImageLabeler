namespace ImageLabeler.Objects
{
    public abstract class EditorObject<T> where T : DlibObject
    {
        protected T wrapped;

        protected EditorObject(T tbw)
        {
            wrapped = tbw;
        }

        public T GetDlibObject()
        {
            return wrapped;
        }
    }
}
