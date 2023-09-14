using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveWarningBarUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveParent;
    [SerializeField] private Animator animator;
    private const string WARNING_BAR_BOOL = "IsAboutToBurn";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        stoveParent.OnProgressChanged += StoveParent_OnProgressChanged;
        animator.SetBool(WARNING_BAR_BOOL, false);
    }

    private void StoveParent_OnProgressChanged(object sender, IObjectWithProgress.OnProgressChangedEventArgs e)
    {
        animator.SetBool(WARNING_BAR_BOOL, stoveParent.IsAboutToBurn());
    }

}
