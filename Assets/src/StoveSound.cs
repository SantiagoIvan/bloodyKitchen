using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveSound : MonoBehaviour
{
    [SerializeField] AudioSource sizzleAudio;
    [SerializeField] StoveCounter parent;

    private void Awake()
    {
        sizzleAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        parent.OnStoveActive += StoveCounter_OnStoveActive;
        parent.OnStovePasive += StoveCounter_OnStovePasive;
    }

    private void StoveCounter_OnStovePasive(object sender, System.EventArgs e)
    {
        sizzleAudio.Stop();
    }

    private void StoveCounter_OnStoveActive(object sender, System.EventArgs e)
    {
        sizzleAudio.Play();
    }
}
