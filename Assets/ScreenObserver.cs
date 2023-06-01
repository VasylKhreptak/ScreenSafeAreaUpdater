using System;
using UniRx;
using UnityEngine;

public class ScreenObserver : MonoBehaviour
{
    [Header("Preferences")]
    [SerializeField] private float _updateInterval = 1 / 10f;

    private ReactiveProperty<ScreenOrientation> _screenOrientation = new ReactiveProperty<ScreenOrientation>();
    private ReactiveProperty<Vector2> _screenResolution = new ReactiveProperty<Vector2>();

    public IReadOnlyReactiveProperty<ScreenOrientation> ScreenOrientation => _screenOrientation;
    public IReadOnlyReactiveProperty<Vector2> ScreenResolution => _screenResolution;

    private IDisposable _intervalSubscription;

    #region MonoBehaviour

    private void OnEnable()
    {
        StartObserving();
    }

    private void OnDisable()
    {
        StopObserving();
    }

    #endregion

    private void StartObserving()
    {
        StopObserving();

        _intervalSubscription = Observable
            .Interval(TimeSpan.FromSeconds(_updateInterval))
            .DoOnSubscribe(Observe)
            .Subscribe(_ => Observe());
    }

    private void StopObserving()
    {
        _intervalSubscription?.Dispose();
    }

    private void Observe()
    {
        _screenOrientation.Value = Screen.orientation;
        _screenResolution.Value = new Vector2(Screen.width, Screen.height);
    }
}
