using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu()]
public class RecipesListSO : ScriptableObject
{
    public static RecipesListSO Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    [SerializeField] private List<RecipeSO> availableRecipes;
    
    public RecipeSO GetRandomRecipeSO()
    {
        System.Random rd = new System.Random();
        int generated = rd.Next(0, availableRecipes.Count);
        return availableRecipes[generated];
    }
    public RecipeSO GetRecipeSOByName(string name)
    {
        return availableRecipes.Find(recipe => recipe.GetRecipeName() == name);
    }
}
