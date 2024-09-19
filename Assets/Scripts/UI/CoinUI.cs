using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI coinTMP;
    private CanvasGroup _canvasGroup; 
  
    public void Show()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.DOFade(1, .2f);
    }
    public void Hide()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.DOFade(0, .2f);
    }
    public void UpdateCoinCount()
    {
        coinTMP.text = PlayerPrefs.GetInt("CoinCount").ToString();
    }
    public void CoinBounceAnimation()
    {
        RectTransform coinRectTransform = coinTMP.GetComponent<RectTransform>();
        coinTMP.DOKill();
        coinTMP.rectTransform.DOKill(); // çözemedim
        coinTMP.transform.DOKill();
        Vector3 startPos = coinRectTransform.localPosition;
        Vector3 endPos = startPos + new Vector3(50, 0, 0);
        coinRectTransform.DOLocalMove(endPos, 0.5f).SetLoops(2, LoopType.Yoyo).OnComplete(CoinTMPSetActiveFalse);
    }
    public void CoinTMPSetActiveFalse()
    {
        coinTMP.gameObject.SetActive(false);
    }
}
