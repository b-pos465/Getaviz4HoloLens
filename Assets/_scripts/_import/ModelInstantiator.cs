using Logging;
using Model;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Import
{
    public class ModelInstantiator : MonoBehaviour
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Inject]
        private FlatModelProvider modelProvider;

        [Inject]
        private ModelIndicator modelIndicator;

        [Inject]
        private DiContainer diContainer;

        public GameObject entityPrefab;

        private Dictionary<ID, Entity> entityDict;

        // TODO: Understand why this is necessary.
#if UNITY_EDITOR
        private float initialModelScale = 0.0005f;
#elif UNITY_WSA_10_0
        private float initialModelScale = 0.005f;
#endif

        private void Awake()
        {
            GameObject model = this.InstantiateCity();
            this.ScaleModel(model);
            this.AppendCompleteBoxCollider();
        }

        private void AppendCompleteBoxCollider()
        {
            Bounds bounds = new Bounds(this.modelIndicator.transform.position, Vector3.zero);

            BoxCollider[] boxColliders = this.modelIndicator.gameObject.GetComponentsInChildren<BoxCollider>();

            foreach (BoxCollider collider in boxColliders)
            {
                bounds.Encapsulate(collider.bounds);
            }

            BoxCollider boxCollider = this.modelIndicator.gameObject.AddComponent<BoxCollider>();
            boxCollider.size = bounds.size * 2000f;
            boxCollider.enabled = false;
        }

        private void ScaleModel(GameObject model)
        {
            log.Debug("Scaling model by factor {}.", this.initialModelScale);
            model.transform.localScale = this.initialModelScale * Vector3.one;

            foreach (LineRenderer lineRenderer in model.GetComponentsInChildren<LineRenderer>())
            {
                lineRenderer.widthMultiplier = this.initialModelScale;
            }
        }

        private GameObject InstantiateCity()
        {
            this.entityDict = new Dictionary<ID, Entity>();

            Dictionary<ID, TransformAndColorInformation> transformAndColorInformationDict = this.modelProvider.ProvideTransformAndColorInformation();
            GameObject modelRoot = this.BuildGameObjects(transformAndColorInformationDict);
            this.FillMetaDataInformation(this.modelProvider.ProvideMetaData());

            return modelRoot;
        }

        private GameObject BuildGameObjects(Dictionary<ID, TransformAndColorInformation> transformAndColorInformationDict)
        {
            GameObject model = this.modelIndicator.gameObject;
            model.transform.parent = this.modelIndicator.gameObject.transform;

            foreach (KeyValuePair<ID, TransformAndColorInformation> entry in transformAndColorInformationDict)
            {
                TransformAndColorInformation transformAndColorInformation = entry.Value;

                GameObject entity = this.diContainer.InstantiatePrefab(this.entityPrefab);
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
            Bounds bounds = new Bounds(this.modelIndicator.transform.position, Vector3.zero);
            BoxCollider[] colliders = this.modelIndicator.GetComponentsInChildren<BoxCollider>();
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
