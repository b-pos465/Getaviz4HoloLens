using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Model.Tree;

[RequireComponent(typeof(Text))]
public class FQDNTextController : MonoBehaviour
{
    private static readonly string FQDN_PLACEHOLDER = "...";

    [Inject]
    private AutoCompleteController autoCompleteController;

    private Text text;

    private void Start()
    {
        this.text = this.GetComponent<Text>();
    }

    private void Update()
    {
        EntityNode currentEntityNode = this.autoCompleteController.CurrentEntityNode;
        string fqdn = FQDN_PLACEHOLDER;

        if (!currentEntityNode.IsRoot())
        {
            fqdn = currentEntityNode.MetaData.qualifiedName;
        }

        this.text.text = this.FormatFQDN(fqdn);
    }

    private string FormatFQDN(string fqdn)
    {
        return string.Format("<b>{0}</b>", fqdn);
    }
}
