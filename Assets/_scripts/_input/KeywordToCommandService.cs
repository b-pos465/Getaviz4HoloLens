using UnityEngine.Windows.Speech;
using System.Collections.Generic;

using UnityEngine;
using Logging;
using System;

public class KeywordToCommandService : MonoBehaviour
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private KeywordRecognizer keywordRecognizer;

    public Dictionary<GetavizKeyword, List<Action>> callbackDictionary = new Dictionary<GetavizKeyword, List<Action>>();

    void Start()
    {
        this.keywordRecognizer = new KeywordRecognizer(GetavizKeyword.GetNames(typeof(GetavizKeyword)));
        this.keywordRecognizer.OnPhraseRecognized += this.OnKeyword;

        log.Debug("Starting keyword recognizer ...");
        this.keywordRecognizer.Start();
    }

    public void Register(GetavizKeyword getavizKeyword, Action action)
    {
        if (!this.callbackDictionary.ContainsKey(getavizKeyword))
        {
            this.callbackDictionary[getavizKeyword] = new List<Action>();
        }

        this.callbackDictionary[getavizKeyword].Add(action);
    }

    private void OnKeyword(PhraseRecognizedEventArgs args)
    {
        GetavizKeyword getavizKeyword = (GetavizKeyword)Enum.Parse(typeof(GetavizKeyword), args.text, true);

        log.Debug("Recognized keyword: {}.", getavizKeyword);
        foreach (Action action in this.callbackDictionary[getavizKeyword])
        {
            action.Invoke();
        }
    }
}