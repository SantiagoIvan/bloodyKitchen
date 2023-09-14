using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private AudioClipsSO audios;
    [SerializeField] private float volume = 1f;

    private float footstepTimer;
    private float footstepTimerMax = .1f; // para que no se active en cada frame el audio y no se entienda nada. Ademas se instancian un monton de objetos

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0)
        {
            footstepTimer = footstepTimerMax;
            if (player.IsWalking())
            {
                PlaySound();
            }
        }
    }

    private void PlaySound()
    {
        AudioSource.PlayClipAtPoint(audios.footsteps[0], player.transform.position, volume);
    }
}
