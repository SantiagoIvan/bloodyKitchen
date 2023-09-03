using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderTemplateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeTitle;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipe)
    {
        recipeTitle.text = recipe.GetRecipeName();

        foreach (Transform child in iconContainer)
        {
            if (child != iconTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        // genero las recetas, las spawneo en el contenedor
        foreach (RecipeSO.Ingredient ingredient in recipe.GetIngredients())
        {
            for (int i = 0; i < ingredient.amount; i++)
            {
                Transform spawned = Instantiate(iconTemplate, iconContainer);
                spawned.GetComponent<Image>().sprite = ingredient.typeIngredient.GetSprite();
                spawned.gameObject.SetActive(true);
            }
        }
    }
}
