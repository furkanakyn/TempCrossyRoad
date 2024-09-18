using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void Start()
    {   
        if(gameObject == null)
        {
            return;
        }
        transform.DORotate(180 * Vector3.up, 1.5f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetDelay(Random.Range(0,1f));
        transform.DOMoveY(transform.position.y + .25f, 1).SetLoops(-1, LoopType.Yoyo);
    }
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        transform.DOKill();
    }
}
