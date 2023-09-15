using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

/* Asi como tengo este Sound manager para los efectos visuales, tambien puedo tener un Sound manager para cada prefab. Depende como lo quiera encarar.
 * En este caso, aca capture los efectos de sonido aca y para la cocina, hice un SoundManager en su prefab. Lo mismo sucederá para los footsteps. Voy a hacer un componente adentro que se encargue de todos los sonidos
 * del personaje, que por ahora es uno solo pero puedo tener mas.
 */
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private DeliveryManager manager;
    [SerializeField] AudioClipsSO audios;
    [SerializeField] DeliveryCounter deliveryCounter; // como tengo uno solo, lo linkeo manualmente. Si mañana agrego mas y que cada uno gestione sus pedidos, lo refactorizaré. Lo puedo hacer un evento estatico.


    private const float DEFAULT_VOLUME_MULTIPLIER = 1f;
    private float currentVolume = .5f;
    private const float MAX_VOLUME = 1f;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        manager.OnOrderCompleted += Manager_OnOrderCompleted;
        manager.OnWrongPlateDelivered += Manager_OnWrongPlateDelivered;
        CuttingCounter.OnGlobalCuttingActionTriggered += CuttingCounter_OnGlobalCuttingActionTriggered;
        PlayerController.Instance.OnPickUp += Player_OnPickUp;
        BaseCounter.OnDrop += BaseCounter_OnDrop;
        ThrashCounter.OnItemDropped += ThrashCounter_OnItemDropped;
    }

    private void ThrashCounter_OnItemDropped(object sender, System.EventArgs e)
    {
        ThrashCounter origin = (ThrashCounter)sender;
        PlaySound(audios.thrash, origin.transform.position);
    }

    private void BaseCounter_OnDrop(object sender, System.EventArgs e)
    {
        BaseCounter counter = (BaseCounter)sender;
        PlaySound(audios.drop, counter.transform.position);
    }

    private void Player_OnPickUp(object sender, System.EventArgs e)
    {
        PlayerController player = PlayerController.Instance;
        PlaySound(audios.pickUp, player.transform.position);
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
    public void PlayWarningSound(Vector3 pos)
    {
        PlaySound(audios.stoveWarning, pos, 8); // CAMBIAR POR OTRO SONIDO MAS PIOLA
    }
    public void PlayCountdownSound()
    {
        PlaySound(audios.countdown, Vector3.zero);
    }
    public void PlayStoveWarning(Vector3 pos)
    {
        PlaySound(audios.stoveWarning, pos);
    }
    public void PlayStoveBurning(Vector3 pos)
    {
        PlaySound(audios.stoveBurning, pos);
    }
    // si el sonido se produce en una posición lejos de la cámara, se escuchará lejano
    private void PlaySound(AudioClip audio, Vector3 position, float volumeMultiplier = DEFAULT_VOLUME_MULTIPLIER)
    {
        AudioSource.PlayClipAtPoint(audio, position, volumeMultiplier * currentVolume);
    }
    private void PlaySound(AudioClip[] audioArray, Vector3 position, float volumeMultiplier = DEFAULT_VOLUME_MULTIPLIER)
    {
        int rd = Random.Range(0, audioArray.Length);
        PlaySound(audioArray[rd], position, volumeMultiplier * currentVolume);
    }

    public void ChangeSound()
    {
        currentVolume = (currentVolume + .1f) % (MAX_VOLUME+.1f); // si pongo modulo n, el resultado nunca va a dar n sino hasta n-1. Yo quiero que el volumen llegue a 10 inclusive
    }

    public float GetCurrentVolume() { return  currentVolume; }
}
