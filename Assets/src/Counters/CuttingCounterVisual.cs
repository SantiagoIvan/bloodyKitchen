using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingContainerVisual : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Animator animator;
    private const string CUT_TRIGGER_ANIMATION = "Cut";

    private void Awake()
    {
        //cuttingCounter = GetComponent<CuttingCounter>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        cuttingCounter.OnCuttingActionTriggered += CuttingContainer_OnCuttingActionTriggered;
    }

    private void CuttingContainer_OnCuttingActionTriggered(object sender, EventArgs e)
    {
        animator.SetTrigger(CUT_TRIGGER_ANIMATION);
    }
}
