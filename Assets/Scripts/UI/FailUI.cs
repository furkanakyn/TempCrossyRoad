using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FailUI : MonoBehaviour
{
    public GameDirector gameDirector;
    public Button restartButton;
    private CanvasGroup canvasGroup;

    public void RestartFailUI()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.DOFade(1, .4f);
        restartButton.transform.localScale = Vector3.one;
        restartButton.transform.DOScale(1.1f, .6f).SetLoops(-1, LoopType.Yoyo);
    }
    public void Hide()
    {
        canvasGroup.DOFade(0, .2f).OnComplete(SetActiveFalse);
    }
    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void RestartButtonClicked()
    {
        gameDirector.mainMenu.Show();
        Hide();
    }

}
