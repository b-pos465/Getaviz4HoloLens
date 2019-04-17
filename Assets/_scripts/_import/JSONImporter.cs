using Model;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Import
{
    public class JsonImporter
    {
        private string path;
        private JsonArrayDeserializer jsonArrayDeserializer;

        public JsonImporter(string path)
        {
            this.path = path;
            this.jsonArrayDeserializer = new JsonArrayDeserializer();
        }

        public Dictionary<ID, MetaData> Import()
        {
            string metaDataAsJSON = File.ReadAllText(this.path, Encoding.UTF8);
            MetaData[] metaDatas = this.jsonArrayDeserializer.FromJson<MetaData>(metaDataAsJSON);

            Dictionary<ID, MetaData> metaDataDict = new Dictionary<ID, MetaData>();

            foreach (MetaData metaData in metaDatas)
            {
                ID id = ID.From(metaData.id);

                // The same key is not supposed to occure twice in the meta-data json.
                // This is a bug in "Getaviz" according to David Baum.
                if (!metaDataDict.ContainsKey(id))
                {
                    metaDataDict.Add(id, metaData);
                }
            }
            return metaDataDict;
        }
    }
}
