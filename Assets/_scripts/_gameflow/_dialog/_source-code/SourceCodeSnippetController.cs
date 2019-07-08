using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SourceCodeSnippetController : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    private readonly string[] keywords1 = { "public", "private", "protected", "void", "new", "return"};
    private readonly string[] keywords2 = { "package", "import" };

    public void SetText(string text)
    {
        this.ReplaceWithRichText(text);
    }

    private void ReplaceWithRichText(string text)
    {
        if (this.textMeshProUGUI == null)
        {
            this.textMeshProUGUI = this.GetComponent<TextMeshProUGUI>();
        }

        foreach (string keyword in this.keywords1)
        {
            string replacement = this.Highlight(keyword, "#3987D6ff");
            text = text.Replace(keyword, replacement);
        }

        foreach (string keyword in this.keywords2)
        {
            string replacement = this.Highlight(keyword, "#3DC981ff");
            text = text.Replace(keyword, replacement);
        }

        this.textMeshProUGUI.text = text;
    }

    private string Highlight(string text, string color)
    {
        return "<color=" + color + ">" + text + "</color>";
    }
}
