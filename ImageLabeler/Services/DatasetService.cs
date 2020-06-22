namespace ImageLabeler.Services
{
    public interface IDatasetService
    {
        IDataSet Load(string path);
        void Save(IDataSet dataSet);
    }

    class DatasetService : IDatasetService
    {
        public IDataSet Load(string path)
        {
            IDataSet loaded = ObjectSerializer.Load<DataSet>(path);
            loaded.Path = path;

            return loaded;
        }

        public void Save(IDataSet dataSet)
        {
            if (string.IsNullOrEmpty(dataSet.Path))
            {
                return;
            }

            ObjectSerializer.Save<DataSet>((DataSet)dataSet, dataSet.Path);
        }
    }
}
