using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private DeliveryManager manager;
    [SerializeField] AudioClipsSO audios;
    private const float DEFAULT_VOLUME = 0.5f;
    private Vector3 cameraPos;

    private void Start()
    {
        manager.OnOrderCompleted += Manager_OnOrderCompleted;
        manager.OnWrongPlateDelivered += Manager_OnWrongPlateDelivered;
        cameraPos = Camera.main.transform.position;
    }

    private void Manager_OnWrongPlateDelivered(object sender, System.EventArgs e)
    {
        PlaySound(audios.deliveryFail, cameraPos);
    }

    private void Manager_OnOrderCompleted(object sender, DeliveryManager.OnOrderCompletedEventArgs e)
    {
        PlaySound(audios.deliverySuccess, cameraPos);
    }

    // si el sonido se produce en una posición lejos de la cámara, se escuchará lejano
    private void PlaySound(AudioClip audio, Vector3 position, float volumne = DEFAULT_VOLUME)
    {
        AudioSource.PlayClipAtPoint(audio, position, volumne);
    }
    private void PlaySound(AudioClip[] audioArray, Vector3 position, float volumne = DEFAULT_VOLUME)
    {
        int rd = Random.Range(0, audioArray.Length);
        PlaySound(audioArray[rd], position, volumne);
    }
}
