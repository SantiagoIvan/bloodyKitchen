using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipsSO : ScriptableObject
{
    // para cada tipo de sonido hay diferentes variaciones. Diferentes tipos de por ejemplo deliveryFails, y asi con todo
    // estan en la carpeta de Assets/Sounds/SFX
    public AudioClip[] chop;
    public AudioClip[] deliveryFail;
    public AudioClip[] deliverySuccess;
    public AudioClip[] footsteps;
    public AudioClip[] drop;
    public AudioClip[] pickUp;
    public AudioClip stoveCooking;
    public AudioClip[] thrash;
    public AudioClip countdown;
    public AudioClip stoveWarning;
    public AudioClip stoveBurning;
}
