﻿using System.Collections.Generic;
using UnityEngine;

public class EnableAfterImport : MonoBehaviour
{
    [Tooltip("Those scripts will get activated when there is at least on child GameObject.")]
    public List<MonoBehaviour> scriptsToActivate;

    void Start()
    {
        this.SetEnabledForScripts(false);
    }

    void Update()
    {
        if (this.transform.childCount > 0)
        {
            this.SetEnabledForScripts(true);
            this.enabled = false;
        }
    }

    void SetEnabledForScripts(bool enabled)
    {
        foreach (MonoBehaviour monoBehaviour in this.scriptsToActivate)
        {
            monoBehaviour.enabled = enabled;
        }
    }
}
