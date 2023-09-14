using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoveWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveParent;


    private void Start()
    {
        Hide();
        stoveParent.OnProgressChanged += StoveParent_OnProgressChanged;
        stoveParent.OnStateChanged += StoveParent_OnStateChanged;
    }

    private void StoveParent_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        Hide();
    }

    private void StoveParent_OnProgressChanged(object sender, IObjectWithProgress.OnProgressChangedEventArgs e)
    {
        if (stoveParent.IsAboutToBurn()){
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
