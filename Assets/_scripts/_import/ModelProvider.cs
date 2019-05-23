using Logging;
using Model;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Import
{
    public class ModelProvider
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string modelPath;
        private string metaDataPath;

        private Dictionary<ID, TransformAndColorInformation> transformAndColorInformation;
        private Dictionary<ID, MetaData> metaData;

        public ModelProvider()
        {
            this.modelPath = Path.Combine(Application.streamingAssetsPath, "model.html");
            this.metaDataPath = Path.Combine(Application.streamingAssetsPath, "metaData.json");

            this.Import();
        }

        private void Import()
        {
            log.Debug("Importing model from {} ...", this.modelPath);
            HtmlImporter htmlImporter = new HtmlImporter(this.modelPath);
            this.transformAndColorInformation = htmlImporter.Import();

            log.Debug("Importing metadata from {} ...", this.metaDataPath);
            JsonImporter jsonImporter = new JsonImporter(this.metaDataPath);
            this.metaData = jsonImporter.Import();
        }

        public Dictionary<ID, TransformAndColorInformation> ProvideTransformAndColorInformation()
        {
            return this.transformAndColorInformation;
        }

        public Dictionary<ID, MetaData> ProvideMetaData()
        {
            return this.metaData;
        }
    }
}
