using UnityEngine;
using DG.Tweening;

public class ScreenShakeManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    #region Shake related variables
    [SerializeField] private int _duration = 3;
    [SerializeField] private int _strength = 10;
    [SerializeField] private int _vibrato = 10;
    [SerializeField] private int _randomness = 90;
    #endregion

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public void ScreenShake(float intensity)
    {
        _mainCamera.transform.DOLocalRotate(new Vector3(0, 0, 0), _duration);
        _mainCamera.transform.DOShakeRotation(_duration, _strength, _vibrato, _randomness, true, ShakeRandomnessMode.Full);
    }
}
