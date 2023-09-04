using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private DeliveryManager manager;
    [SerializeField] AudioClipsSO audios;
    [SerializeField] DeliveryCounter deliveryCounter; // como tengo uno solo, lo linkeo manualmente. Si mañana agrego mas y que cada uno gestione sus pedidos, lo refactorizaré. Lo puedo hacer un evento estatico.

    private const float DEFAULT_VOLUME = 0.75f;
    private Vector3 cameraPos;

    private void Start()
    {
        manager.OnOrderCompleted += Manager_OnOrderCompleted;
        manager.OnWrongPlateDelivered += Manager_OnWrongPlateDelivered;
        CuttingCounter.OnGlobalCuttingActionTriggered += CuttingCounter_OnGlobalCuttingActionTriggered;
    }

    private void CuttingCounter_OnGlobalCuttingActionTriggered(object sender, System.EventArgs e)
    {
        CuttingCounter origin = sender as CuttingCounter;
        PlaySound(audios.chop, origin.transform.position);
    }

    private void Manager_OnWrongPlateDelivered(object sender, System.EventArgs e)
    {
        PlaySound(audios.deliveryFail, deliveryCounter.transform.position);
    }

    private void Manager_OnOrderCompleted(object sender, DeliveryManager.OnOrderCompletedEventArgs e)
    {
        PlaySound(audios.deliverySuccess, deliveryCounter.transform.position);
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
