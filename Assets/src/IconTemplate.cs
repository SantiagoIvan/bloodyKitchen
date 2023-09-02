using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconTemplate : MonoBehaviour
{
    [SerializeField] private Image img;

    public void SetKitchenObjectSOImg(KitchenObjectSO ingredient)
    {
        img.sprite = ingredient.GetSprite();
    }
}
