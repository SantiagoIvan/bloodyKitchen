using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    private const float MAX_VOLUME = 1f;
    private float currentVolume = .5f;

    public void ChangeSound()
    {
        currentVolume = (currentVolume + .1f) % (MAX_VOLUME + .1f); // si pongo modulo n, el resultado nunca va a dar n sino hasta n-1. Yo quiero que el volumen llegue a 10 inclusive
        audioSource.volume = currentVolume;
    }

    public float GetCurrentVolume() { return currentVolume; }
}
