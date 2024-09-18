using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource jumpAS;
    public AudioSource positiveAS;
    public AudioSource ambientAS;
    public AudioSource deadAS;
    public AudioSource carHornAS;

    private void Start()
    {
        ambientAS.Play();
    }
    public void PlayJumpSound()
    {
        jumpAS.Play();
    }
    public void PlayPositiveSound()
    {
        positiveAS.Play();
    }
    public void PlayAmbientSound()
    {
        ambientAS.Play();
    }
    public void PlayDeadSound()
    {
        deadAS.Play();
    }
    public void PlayHornSound()
    {
        carHornAS.Play();
    }
}
