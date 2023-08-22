using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>(); // tambien se puede hacer con el campo serializable, y linkearlos desde el editor
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, playerController.IsWalking());
    }
}
