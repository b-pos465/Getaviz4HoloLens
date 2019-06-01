using Logging;
using System;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class TapService : MonoBehaviour
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private GestureRecognizer gestureRecognizer;

    private void Awake()
    {
        log.Debug("Starting 'GestureRecognizer' ...");
        this.gestureRecognizer = new GestureRecognizer();
        this.gestureRecognizer.StartCapturingGestures();
    }

    public void Register(Action<TappedEventArgs> OnTap)
    {
        log.Debug("Register action for air tap event: {} in {}.", OnTap.Method.Name, OnTap.Method.DeclaringType);
        this.gestureRecognizer.Tapped += OnTap;
    }

    public void Unregister(Action<TappedEventArgs> OnTap)
    {
        log.Debug("Unregister action for air tap event: {} in {}.", OnTap.Method.Name, OnTap.Method.DeclaringType);
        this.gestureRecognizer.Tapped -= OnTap;
    }

    private void OnDestroy()
    {
        log.Debug("Stopping 'GestureRecognizer' ...");
        this.gestureRecognizer.StopCapturingGestures();
    }
}
