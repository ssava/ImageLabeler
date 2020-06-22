using ImageLabeler.Services;
using System.Collections.Generic;
using System.Linq;

namespace ImageLabeler.Objects
{
    public sealed class EditorDataSet : EditorObject<IDataSet>
    {
        public string Path { get; set; }

        public string Name => DlibObject.Name;
        public string Comment => DlibObject.Comment;

        public List<EditorImage> Images { get; private set; }
        public string[] Labels { get; internal set; }

        public EditorDataSet(IDataSet dataSet) :
            base(dataSet)
        {
            SortedSet<string> setLabels = new SortedSet<string>();
            Images = new List<EditorImage>();

            foreach (DataImage dImg in DlibObject.Images)
            {
                EditorImage wrappedImage = new EditorImage(dImg);
                Images.Add(wrappedImage);

                foreach (EditorBox b in wrappedImage.Boxes)
                    if (!string.IsNullOrEmpty(b.Label))
                        if (!setLabels.Contains(b.Label))
                            setLabels.Add(b.Label);
            }

            Labels = setLabels.ToArray();
        }

        public override string ToString()
        {
            return DlibObject.ToString();
        }

        public static EditorDataSet Load(IDatasetService datasetService, string fileName)
        {
            EditorDataSet loaded = new EditorDataSet(datasetService.Load(fileName));
            loaded.Path = fileName;

            return loaded;
        }

        public EditorImage AddImage(string fileName)
        {
            DataImage dImg = new DataImage
            {
                File = fileName,
                Boxes = new List<DataBox>()
            };

            EditorImage edImg = new EditorImage(dImg);

            DlibObject.Images.Add(dImg);
            Images.Add(edImg);

            return edImg;
        }
    }
}
