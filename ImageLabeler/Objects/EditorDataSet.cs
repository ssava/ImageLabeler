using System;
using System.Collections.Generic;

namespace ImageLabeler.Objects
{
    public sealed class EditorDataSet : EditorObject<DataSet>
    {
        public string Path { get; set; }

        public string Name => wrapped.Name;
        public string Comment => wrapped.Comment;

        public List<EditorImage> Images { get; private set; }

        public EditorDataSet(DataSet dataSet) :
            base(dataSet)
        {
            Images = new List<EditorImage>();

            foreach (DataImage dImg in wrapped.Images)
                Images.Add(new EditorImage(dImg));
        }

        public override string ToString()
        {
            return wrapped.ToString();
        }

        public static EditorDataSet Load(string fileName)
        {
            EditorDataSet loaded = new EditorDataSet(DlibObject.Load<DataSet>(fileName));
            loaded.Path = fileName;

            return loaded;
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(Path))
                //showdlgsave
                return;

            DlibObject.Save<DataSet>(wrapped, Path);
        }

        public EditorImage AddImage(string fileName)
        {
            DataImage dImg = new DataImage
            {
                File = fileName,
                Boxes = new List<DataBox>()
            };

            EditorImage edImg = new EditorImage(dImg);

            wrapped.Images.Add(dImg);
            Images.Add(edImg);

            return edImg;
        }
    }
}
