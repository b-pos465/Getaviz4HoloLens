using Model;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SourceCodeClassNameController : MonoBehaviour
{
    private Text text;

    public void UpdateClassName(Entity entity)
    {
        if (this.text == null)
        {
            this.text = this.GetComponent<Text>();
        }
        this.text.text = entity.qualifiedName;
    }
}
