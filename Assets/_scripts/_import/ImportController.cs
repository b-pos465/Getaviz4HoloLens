using Logging;
using Model;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Import
{
    public class ImportController : MonoBehaviour
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string MODEL_PATH = @"Assets/_generator-data/model.html";
        private static readonly string METADATA_PATH = @"Assets/_generator-data/metaData.json";

        [Inject]
        private MeshRootIndicator meshRootIndicator;

        public GameObject entityPrefab;

        private Dictionary<ID, Entity> entityDict;

        public GameObject Import()
        {
            this.entityDict = new Dictionary<ID, Entity>();

            log.Debug("Importing model from {} ...", MODEL_PATH);
            HtmlImporter htmlImporter = new HtmlImporter(MODEL_PATH);
            Dictionary<ID, TransformAndColorInformation> transformAndColorInformationDict = htmlImporter.Import();

            GameObject modelRoot = this.BuildGameObjects(transformAndColorInformationDict);

            log.Debug("Importing metadata from {} ...", METADATA_PATH);
            JsonImporter jsonImporter = new JsonImporter(METADATA_PATH);
            Dictionary<ID, MetaData> metaDataDict = jsonImporter.Import();

            this.FillMetaDataInformation(metaDataDict);

            return modelRoot;
        }

        private GameObject BuildGameObjects(Dictionary<ID, TransformAndColorInformation> transformAndColorInformationDict)
        {
            GameObject model = new GameObject("Model");
            model.transform.parent = this.meshRootIndicator.gameObject.transform;

            foreach (KeyValuePair<ID, TransformAndColorInformation> entry in transformAndColorInformationDict)
            {
                TransformAndColorInformation transformAndColorInformation = entry.Value;

                GameObject entity = Instantiate(this.entityPrefab);
                entity.transform.parent = model.transform;
                entity.transform.position = transformAndColorInformation.Position;
                entity.transform.localScale = transformAndColorInformation.Scale;
                entity.name = entry.Key.ToString();
                entity.GetComponent<MeshRenderer>().material.SetColor("_Color", transformAndColorInformation.Color);
                this.entityDict.Add(ID.From(entry.Key.ToString()), entity.GetComponent<Entity>());
            }

            return model;
        }

        private void FillMetaDataInformation(Dictionary<ID, MetaData> metaDataDict)
        {
            foreach (KeyValuePair<ID, Entity> entry in entityDict)
            {
                Entity entity = entry.Value;
                ID id = entry.Key;

                MetaData metaData = metaDataDict[id];

                entity.id = id;
                entity.qualifiedName = metaData.qualifiedName;
                entity.name = metaData.name;
                entity.type = metaData.type;
                entity.modifiers = ParseModifiers(metaData.modifiers);
                entity.signature = metaData.signature;
                entity.calls = FindByJsonID(metaData.calls);
                entity.calledBy = FindByJsonID(metaData.calledBy);
                entity.accesses = FindByJsonID(metaData.accesses);
                entity.belongsTo = FindByJsonID(metaData.belongsTo);
            }
        }

        List<string> ParseModifiers(string modifiersInJson)
        {
            if (string.IsNullOrEmpty(modifiersInJson))
            {
                return new List<string>();
            }

            string[] modifiers = modifiersInJson.Split(',');

            for (int i = 0; i < modifiers.Length; i++)
            {
                modifiers[i] = modifiers[i].Trim();
            }
            return new List<string>(modifiers);
        }

        GameObject FindByJsonID(string jsonId)
        {
            if (string.IsNullOrEmpty(jsonId))
            {
                return null;
            }

            ID id = ID.From(jsonId);

            // The meta-data json also contains methods, attributes and other objects which are not represented in the HTML data.
            // Therefore, we need to check whether the dictionary contains this key.
            if (!this.entityDict.ContainsKey(id))
            {
                return null;
            }

            return this.entityDict[id].gameObject;
        }
    }
}
