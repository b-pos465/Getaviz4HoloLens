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
        private ModelRootIndicator modelRootIndicator;

        [Inject]
        DiContainer diContainter;

        public GameObject entityPrefab;

        [Header("Debug")]
        public bool importOnStartUp = false;

        private Dictionary<ID, Entity> entityDict;

        private void Start()
        {
            if (this.importOnStartUp)
            {
                this.Import();
            }
        }

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
            GameObject model = this.modelRootIndicator.gameObject;
            model.transform.parent = this.modelRootIndicator.gameObject.transform;

            foreach (KeyValuePair<ID, TransformAndColorInformation> entry in transformAndColorInformationDict)
            {
                TransformAndColorInformation transformAndColorInformation = entry.Value;

                GameObject entity = this.diContainter.InstantiatePrefab(this.entityPrefab);
                entity.transform.parent = model.transform;
                entity.transform.position = transformAndColorInformation.Position;
                entity.transform.localScale = transformAndColorInformation.Scale;
                entity.name = entry.Key.ToString();
                entity.GetComponent<MeshRenderer>().material.SetColor("_Color", transformAndColorInformation.Color);
                this.entityDict.Add(ID.From(entry.Key.ToString()), entity.GetComponent<Entity>());
            }

            this.MoveChildsToCenterPivot();

            return model;
        }

        private void MoveChildsToCenterPivot()
        {
            Bounds bounds = new Bounds(this.modelRootIndicator.transform.position, Vector3.zero);
            BoxCollider[] colliders = this.modelRootIndicator.GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider collider in colliders)
            {
                bounds.Encapsulate(collider.bounds);
            }

            foreach (BoxCollider collider in colliders)
            {
                Vector3 pivotOffsetWithoutY = new Vector3(bounds.size.x / 2f, 0, bounds.size.z / 2f);
                collider.gameObject.transform.position -= pivotOffsetWithoutY;
            }
        }

        private void FillMetaDataInformation(Dictionary<ID, MetaData> metaDataDict)
        {
            foreach (KeyValuePair<ID, Entity> entry in this.entityDict)
            {
                Entity entity = entry.Value;
                ID id = entry.Key;

                MetaData metaData = metaDataDict[id];

                entity.id = id;
                entity.qualifiedName = metaData.qualifiedName;
                entity.name = metaData.name;
                entity.type = metaData.type;
                entity.modifiers = this.ParseModifiers(metaData.modifiers);
                entity.signature = metaData.signature;
                entity.calls = this.FindByJsonID(metaData.calls);
                entity.calledBy = this.FindByJsonID(metaData.calledBy);
                entity.accesses = this.FindByJsonID(metaData.accesses);
                entity.belongsTo = this.FindByJsonID(metaData.belongsTo);
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
