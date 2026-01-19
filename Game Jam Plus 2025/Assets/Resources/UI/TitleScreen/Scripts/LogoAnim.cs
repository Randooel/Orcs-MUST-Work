using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [ReadOnly]
    [SerializeField] private Vector3 _originalScale;
    [SerializeField] private float _newScaleModifier;
    [SerializeField, Range(0, 10)] float _duration;

    void Start()
    {
        _originalScale = transform.localScale;

        DoLogoAnim();
    }

    private void DoLogoAnim()
    {
        var newScale = new Vector3(_originalScale.x * _newScaleModifier, _originalScale.y * _newScaleModifier);

        this.transform.DOScale(newScale, _duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

}
