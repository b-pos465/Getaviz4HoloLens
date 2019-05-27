using Import;
using Model.Tree;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FilterDialogController : MonoBehaviour
{
    public static readonly string BACK_BUTTON_TEXT = "< Back";
    private static readonly string FQDN_PLACEHOLDER = "...";

    [Inject]
    private TreeModelProvider treeModelProvider;

    [Inject]
    private DiContainer diContainer;

    public Text fullQualifiedDomainName;

    public GameObject autoCompleteEntryPrefab;
    public GameObject backButtonEntryPrefab;

    public Transform scrollViewContent;

    private EntityNode entityNode;

    private void Start()
    {
        this.fullQualifiedDomainName.text = this.FormatFQDN(FQDN_PLACEHOLDER);

        this.entityNode = this.treeModelProvider.ProvideTree().Root;
        this.UpdateAutoCompleteEntries();
    }

    public void SelectEntry(string name)
    {
        // If the user clicks on the active leaf do nothing.
        if (name == this.entityNode.Name)
        {
            return;
        }

        if (name == BACK_BUTTON_TEXT && !this.entityNode.IsLeaf())
        {
            this.entityNode = this.entityNode.Ancestor;
        }
        else if (name == BACK_BUTTON_TEXT && this.entityNode.IsLeaf())
        {
            this.entityNode = this.entityNode.Ancestor.Ancestor;
        }
        else if (this.entityNode.IsLeaf())
        {
            this.entityNode = this.entityNode.Ancestor.Descandents[name];
        }
        else
        {
            this.entityNode = this.entityNode.Descandents[name];
        }

        this.UpdateFQDN(name);
        this.UpdateAutoCompleteEntries();
    }

    private void UpdateFQDN(string name)
    {
        if (this.entityNode.Ancestor == null)
        {
            this.fullQualifiedDomainName.text = this.FormatFQDN(FQDN_PLACEHOLDER);
            return;
        }

        EntityNode current = this.entityNode;
        string result = current.Name;

        while (!this.AncestorIsRoot(current))
        {
            result = current.Ancestor.Name + "." + result;
            current = current.Ancestor;
        }

        this.fullQualifiedDomainName.text = this.FormatFQDN(result);
    }

    private bool AncestorIsRoot(EntityNode node)
    {
        return node.Ancestor == null || node.Ancestor.Name == null;
    }

    private void UpdateAutoCompleteEntries()
    {
        this.DeleteExistingAutoCompleteEntries();
        this.AddBackButtonIfNecessary();

        if (this.entityNode.Descandents.Count > 0)
        {
            this.CreateAutoCompleteEntriesFromNodes(this.entityNode.Descandents);
        }
        else
        {
            this.CreateAutoCompleteEntriesFromNodes(this.entityNode.Ancestor.Descandents);
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
        if (this.entityNode != this.treeModelProvider.ProvideTree().Root)
        {
            this.InstantiateEntry(BACK_BUTTON_TEXT, this.backButtonEntryPrefab);
        }
    }

    private void CreateAutoCompleteEntriesFromNodes(Dictionary<string, EntityNode> nodes)
    {
        foreach (string name in nodes.Keys.OrderBy(q => q).ToList())
        {
            this.InstantiateEntry(name);
        }
    }

    private void InstantiateEntry(string text)
    {
        this.InstantiateEntry(text, this.autoCompleteEntryPrefab);
    }

    private void InstantiateEntry(string text, GameObject prefab)
    {
        GameObject entry = this.diContainer.InstantiatePrefab(prefab);
        entry.transform.SetParent(this.scrollViewContent, false);
        entry.GetComponentInChildren<Text>().text = text;
    }

    private string FormatFQDN(string fqdn)
    {
        return string.Format("<b>{0}</b>", fqdn);
    }
}
