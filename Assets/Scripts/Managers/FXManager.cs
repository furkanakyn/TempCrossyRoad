using UnityEngine;

public class FXManager : MonoBehaviour
{
    public ParticleSystem coinCollectPSPrefab;



    public void CollectCoinParticles(Vector3 pos)
    {
        var newPS = Instantiate(coinCollectPSPrefab);

        newPS.transform.position = pos;
        newPS.Play();
    }
}
