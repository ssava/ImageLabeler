namespace ImageLabeler.Objects
{
    public abstract class EditorObject<TObject> where TObject : IDlibObject
    {
        public TObject DlibObject { get; protected set; }

        protected EditorObject(TObject tbw)
        {
            DlibObject = tbw;
        }
    }
}
