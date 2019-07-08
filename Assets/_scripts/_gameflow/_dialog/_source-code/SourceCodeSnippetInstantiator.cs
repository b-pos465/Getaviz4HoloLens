using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SourceCodeSnippetInstantiator : MonoBehaviour
{
    [Inject]
    private DiContainer diContainer;

    [Inject]
    private SourceCodeScrollViewIndicator sourceCodeScrollViewIndicator;

    public GameObject sourceCodeSnippetPrefab;

    public readonly int linesOfCodePerSnippet = 26;

    public void InstantiateSourceCodeSnippets(string completeSourceCode)
    {
        this.DeleteExistingSnippets();

        List<string> codeInLines = new List<string>(completeSourceCode.Split('\n'));

        int iteration = 0;
        while (codeInLines.Count > 0)
        {
            List<string> codeForOneSnippet;
            if (codeInLines.Count >= this.linesOfCodePerSnippet)
            {
                codeForOneSnippet = codeInLines.GetRange(0, this.linesOfCodePerSnippet);
            }
            else
            {
                codeForOneSnippet = codeInLines.GetRange(0, codeInLines.Count);
            }

            this.InstantiateSourceCodeSnippet(iteration, codeForOneSnippet);

            if (codeInLines.Count <= this.linesOfCodePerSnippet)
            {
                return;
            }

            codeInLines.RemoveRange(0, this.linesOfCodePerSnippet);
            iteration++;
        }
    }

    private void DeleteExistingSnippets()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void InstantiateSourceCodeSnippet(int iteration, List<string> codeForOneSnippet)
    {
        GameObject sourceCodeSnippetGameObject = this.diContainer.InstantiatePrefab(this.sourceCodeSnippetPrefab, this.transform);
        sourceCodeSnippetGameObject.transform.localScale = Vector3.one;

        int startLine = iteration * this.linesOfCodePerSnippet + 1;
        int endLine = startLine + Math.Min(this.linesOfCodePerSnippet, codeForOneSnippet.Count) - 1;
        sourceCodeSnippetGameObject.gameObject.name = string.Format("TMText - Source Code Snippet [{0} - {1}]", startLine, endLine);
        sourceCodeSnippetGameObject.GetComponentInChildren<SourceCodeLineNumberController>().SetLineNumbers(startLine, endLine);

        SourceCodeSnippetController sourceCodeSnippetController = sourceCodeSnippetGameObject.GetComponentInChildren<SourceCodeSnippetController>();
        sourceCodeSnippetController.SetText(string.Join("\n", codeForOneSnippet.ToArray()));

        this.sourceCodeScrollViewIndicator.GetComponent<ScrollRect>().verticalNormalizedPosition = 1.0f;
    }
}
