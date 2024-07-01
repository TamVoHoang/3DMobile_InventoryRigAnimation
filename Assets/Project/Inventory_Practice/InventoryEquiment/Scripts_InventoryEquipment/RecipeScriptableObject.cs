using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RecipeScriptableObject")]
public class RecipeScriptableObject : ScriptableObject
{
    public ItemScriptableObject output;

    public ItemScriptableObject item_00;
    public ItemScriptableObject item_10;
    public ItemScriptableObject item_20;

    public ItemScriptableObject item_01;
    public ItemScriptableObject item_11;
    public ItemScriptableObject item_21;

    public ItemScriptableObject item_02;
    public ItemScriptableObject item_12;
    public ItemScriptableObject item_22;


    public ItemScriptableObject GetItem(int x, int y) {
        if (x == 0 && y == 0) return item_00;
        if (x == 1 && y == 0) return item_10;
        if (x == 2 && y == 0) return item_20;

        if (x == 0 && y == 1) return item_01;
        if (x == 1 && y == 1) return item_11;
        if (x == 2 && y == 1) return item_21;

        if (x == 0 && y == 2) return item_02;
        if (x == 1 && y == 2) return item_12;
        if (x == 2 && y == 2) return item_22;

        return null;
    }
}
