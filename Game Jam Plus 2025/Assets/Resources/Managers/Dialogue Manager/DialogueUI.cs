using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    [Header("UI Bars")]
    [SerializeField] Image topBar;
    [SerializeField] Image bottomBar;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void DOIntroBars()
    {
        topBar.transform.DOLocalMoveY(478, 0.5f).SetEase(Ease.OutQuad);
        bottomBar.transform.DOLocalMoveY(-478, 0.5f).SetEase(Ease.OutQuad);
    }

    public void DOExitBars()
    {
        topBar.transform.DOLocalMoveY(613, 0.5f).SetEase(Ease.OutQuad);
        bottomBar.transform.DOLocalMoveY(-613, 0.5f).SetEase(Ease.OutQuad);
    }
}
