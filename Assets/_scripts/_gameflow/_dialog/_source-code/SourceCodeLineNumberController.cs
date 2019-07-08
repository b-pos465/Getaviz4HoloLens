using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SourceCodeLineNumberController : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    public void SetLineNumbers(int startNumber, int endNumber)
    {
        if (this.textMeshProUGUI == null)
        {
            this.textMeshProUGUI = this.GetComponent<TextMeshProUGUI>();
        }

        string result = "";

        for (int i = startNumber; i <= endNumber; i++)
        {
            if (i == startNumber)
            {
                result += i;
                continue;
            }

            result += "\n" + i;
        }

        this.textMeshProUGUI.text = result;
    }
}
