using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SourceCodeSyntaxHighlighter : MonoBehaviour
{
    private Text text;

    private readonly string[] keywords1 = { "public", "private", "void", "this", "using", "namespace", "for", "new", "return" };
    private readonly string[] keywords2 = { "int", "float", "object" };

    private void Start()
    {
        this.text = this.GetComponent<Text>();
        this.ReplaceWithRichText();
    }

    private void ReplaceWithRichText()
    {
        string text = this.text.text;

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

        this.text.text = text;
    }

    private string Highlight(string text, string color)
    {
        return "<color=" + color + ">" + text + "</color>";
    }
}
