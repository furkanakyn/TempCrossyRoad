using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    public TextMeshProUGUI playerScoreTMP;
    public void UpdatePlayerScore(int playerScore)
    {
        playerScoreTMP.text = playerScore.ToString();
        playerScoreTMP.DOKill();
        playerScoreTMP.transform.localScale = Vector3.one;
        playerScoreTMP.transform.DOScale(1.5f, .1f).SetLoops(2, LoopType.Yoyo);
    }
}
