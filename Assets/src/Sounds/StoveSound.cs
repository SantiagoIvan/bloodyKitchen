using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveSound : MonoBehaviour
{
    [SerializeField] private AudioSource sizzleAudio;
    [SerializeField] private StoveCounter parent;
    private float warningSoundTimer = 1; // para no tirar PlaySound en cada frame y que se rebugee todo. Cuando esté habilitado para tirar el sonidito, lo tiro cada un segundo
    private float currentTimer = 0;

    private void Awake()
    {
        sizzleAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (parent.IsAboutToBurn())
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > warningSoundTimer)
            {
                SoundManager.Instance.PlayWarningSound(parent.transform.position);
                currentTimer = 0;
            }
        }
    }

    private void Start()
    {
        parent.OnStoveActive += StoveCounter_OnStoveActive;
        parent.OnStovePasive += StoveCounter_OnStovePasive;
        parent.OnFoodBurned += Parent_OnFoodBurned;
    }

    private void Parent_OnFoodBurned(object sender, System.EventArgs e)
    {
        Debug.Log("Playing PII PIII PIIII Sound");
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
