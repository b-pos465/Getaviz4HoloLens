using Gaze;
using Import;
using Model.Tree;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

public class AutoCompleteController : MonoBehaviour
{
    [Inject]
    private TreeModelProvider treeModelProvider;

    [Inject]
    private DiContainer diContainer;

    [Inject]
    private RayCaster rayCaster;

    [Inject]
    private TapService tapService;

    [Inject]
    private ButtonClickSoundService buttonClickSoundService;

    [Inject]
    private FilterContentIndicator filterContentIndicator;


    public GameObject autoCompleteEntryPrefab;
    public GameObject backButtonEntryPrefab;

    public Transform scrollViewContent;

    public EntityNode CurrentEntityNode { get; private set; }

    private void Awake()
    {
        this.DeleteExistingAutoCompleteEntries();
    }

    private void Start()
    {
        this.tapService.Register(this.OnTap);

        this.CurrentEntityNode = this.treeModelProvider.ProvideTree().Root;
        this.UpdateAutoCompleteEntries();
    }

    private void OnTap(TappedEventArgs tappedEventArgs)
    {
        if (!this.rayCaster.Hits)
        {
            return;
        }

        if (!this.TargetIsUIAndInsideViewport(this.rayCaster.Target))
        {
            return;
        }

        AutoCompleteEntryController entry = this.rayCaster.Target.GetComponent<AutoCompleteEntryController>();
        AutoCompleteBackButtonIndicator backButton = this.rayCaster.Target.GetComponent<AutoCompleteBackButtonIndicator>();

        if (entry != null)
        {
            this.buttonClickSoundService.PlayButtonClickSound();
            this.SelectEntry(entry.EntityNode);
        }
        else if (backButton != null)
        {
            this.buttonClickSoundService.PlayButtonClickSound();
            this.GoBack();
        }
    }

    private bool TargetIsUIAndInsideViewport(GameObject target)
    {
        RectTransform targetRectTransform = target.GetComponent<RectTransform>();

        if (targetRectTransform == null)
        {
            return false;
        }

        float contentOffset = this.filterContentIndicator.GetComponent<RectTransform>().localPosition.y;
        float midOfTargetElement = Mathf.Abs(targetRectTransform.localPosition.y) + Mathf.Abs(targetRectTransform.rect.height / 2f);

        return midOfTargetElement > contentOffset;
    }

    private void SelectEntry(EntityNode entityNode)
    {
        string name = entityNode.Name;

        if (entityNode == this.CurrentEntityNode)
        {
            this.CurrentEntityNode = this.CurrentEntityNode.Ancestor;
        }
        else if (this.CurrentEntityNode.IsLeaf())
        {
            this.CurrentEntityNode = this.CurrentEntityNode.Ancestor.Descandents[name];
        }
        else
        {
            this.CurrentEntityNode = this.CurrentEntityNode.Descandents[name];
        }

        this.UpdateAutoCompleteEntries();
    }

    public void GoBack()
    {
        if (!this.CurrentEntityNode.IsLeaf())
        {
            this.CurrentEntityNode = this.CurrentEntityNode.Ancestor;
        }
        else if (this.CurrentEntityNode.IsLeaf())
        {
            this.CurrentEntityNode = this.CurrentEntityNode.Ancestor.Ancestor;
        }

        this.UpdateAutoCompleteEntries();
    }

    private bool AncestorIsRoot(EntityNode node)
    {
        return node.Ancestor == null || node.Ancestor.Name == null;
    }

    private void UpdateAutoCompleteEntries()
    {
        this.DeleteExistingAutoCompleteEntries();
        this.AddBackButtonIfNecessary();

        if (this.CurrentEntityNode.Descandents.Count > 0)
        {
            this.CreateAutoCompleteEntriesFromNodes(this.CurrentEntityNode.Descandents);
        }
        else
        {
            this.CreateAutoCompleteEntriesFromNodes(this.CurrentEntityNode.Ancestor.Descandents);
        }

    }

    private void DeleteExistingAutoCompleteEntries()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in this.scrollViewContent)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(child => Destroy(child));
    }

    private void AddBackButtonIfNecessary()
    {
        if (this.CurrentEntityNode != this.treeModelProvider.ProvideTree().Root)
        {
            GameObject entry = this.diContainer.InstantiatePrefab(this.backButtonEntryPrefab);
            entry.transform.SetParent(this.scrollViewContent, false);
        }
    }

    private void CreateAutoCompleteEntriesFromNodes(Dictionary<string, EntityNode> entityNodes)
    {
        foreach (EntityNode entityNode in entityNodes.Values.OrderBy(entityNode => entityNode.Name).ToList())
        {
            this.InstantiateEntry(entityNode, this.autoCompleteEntryPrefab);
        }
    }

    private void InstantiateEntry(EntityNode entityNode, GameObject prefab)
    {
        GameObject entry = this.diContainer.InstantiatePrefab(prefab);
        entry.transform.SetParent(this.scrollViewContent, false);
        entry.GetComponent<AutoCompleteEntryController>().EntityNode = entityNode;
    }
}
