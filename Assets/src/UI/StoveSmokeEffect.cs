using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveSmokeEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private StoveCounter parent;

    private void Awake()
    {
        smoke = GetComponent<ParticleSystem>();
    }
    private void Start()
    {
        smoke.Stop();
        parent.OnFoodBurned += Parent_OnFoodBurned;
        parent.OnStateChanged += Parent_OnStateChanged;
    }

    private void Parent_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if(e.state != StoveCounter.State.BURNED)
        {
            smoke.Stop();
        }
    }

    private void Parent_OnFoodBurned(object sender, System.EventArgs e)
    {
        smoke.Play();
    }
    
}
