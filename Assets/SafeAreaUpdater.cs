using System;
using UniRx;
using UnityEngine;

public class SafeAreaUpdater : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private ScreenObserver _screenObserver;

    private IDisposable _subscription;

    #region MonoBehaiour

    private void OnValidate()
    {
        _rectTransform ??= GetComponent<RectTransform>();
        _screenObserver ??= FindObjectOfType<ScreenObserver>();
    }

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
        _subscription = Observable
            .CombineLatest(_screenObserver.ScreenOrientation, _screenObserver.ScreenResolution,
                (orientation, resolution) => (orientation, resolution))
            .ThrottleFrame(1)
            .Subscribe(tuple => UpdateArea());
    }

    private void StopObserving()
    {
        _subscription?.Dispose();
    }

    private void UpdateArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
    }
}
