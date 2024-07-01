using System;
using System.Collections.Generic;


public class CraftingSystem : IItemHolder
{
    public const int GRID_SIZE = 3;
    public event EventHandler OnGridChanged;
    private Item[,] itemArray; // item[0,0] 
    private Item outputItem;
    //private Dictionary<Item.ItemType, Item.ItemType[,]> recipeDictionary;
    private List<RecipeScriptableObject> recipeScriptableObjectList;


    public CraftingSystem(List<RecipeScriptableObject> recipeScriptableObjectList)  //? colum 18 Testing.cs khoi tao
    {
        this.recipeScriptableObjectList = recipeScriptableObjectList;

        itemArray = new Item[GRID_SIZE, GRID_SIZE]; // matran 2 chiieu

        /*
        recipeDictionary = new Dictionary<Item.ItemType, Item.ItemType[,]>();
        Item.ItemType[,] recipe = new Item.ItemType[GRID_SIZE, GRID_SIZE];
        recipe[0,2] = Item.ItemType.None;           recipe[1,2] = Item.ItemType.Iron;          recipe[2,2] = Item.ItemType.None;
        recipe[0,1] = Item.ItemType.None;           recipe[1,1] = Item.ItemType.Iron;          recipe[2,1] = Item.ItemType.None;
        recipe[0,0] = Item.ItemType.None;           recipe[1,0] = Item.ItemType.Iron;          recipe[2,0] = Item.ItemType.None;
        recipeDictionary[Item.ItemType.Sword_broken] = recipe;

        recipe = new Item.ItemType[GRID_SIZE, GRID_SIZE];
        recipe[0,2] = Item.ItemType.Iron;           recipe[1,2] = Item.ItemType.None;           recipe[2,2] = Item.ItemType.Iron;
        recipe[0,1] = Item.ItemType.None;           recipe[1,1] = Item.ItemType.Sword_broken;   recipe[2,1] = Item.ItemType.None;
        recipe[0,0] = Item.ItemType.Iron;           recipe[1,0] = Item.ItemType.None;           recipe[2,0] = Item.ItemType.Iron;
        recipeDictionary[Item.ItemType.Sword_iron] = recipe;

        recipe = new Item.ItemType[GRID_SIZE, GRID_SIZE];
        recipe[0,2] = Item.ItemType.None;           recipe[1,2] = Item.ItemType.Gold;           recipe[2,2] = Item.ItemType.None;
        recipe[0,1] = Item.ItemType.Gold;           recipe[1,1] = Item.ItemType.Sword_iron;     recipe[2,1] = Item.ItemType.Gold;
        recipe[0,0] = Item.ItemType.None;           recipe[1,0] = Item.ItemType.Gold;           recipe[2,0] = Item.ItemType.None;
        recipeDictionary[Item.ItemType.Sword_gold] = recipe;
        */
    }

    public bool IsEmpty(int x, int y) => itemArray[x, y] == null;

    public void SetItem(Item item, int x, int y) {
        if (item != null) {
            item.RemoveFromItemHolder();
            item.SetItemHolder(this);
        }
        itemArray[x, y] = item;
        CreateOutput();
        OnGridChanged?.Invoke(this, EventArgs.Empty);
    }

    public Item GetItem(int x, int y) {
        return itemArray[x, y];
    }

    public void IncreaseItemAmount(int x, int y) {
        GetItem(x, y).amount++;

        OnGridChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DecreaseItemAmount(int x, int y) {
        if (GetItem(x, y) != null) {
            GetItem(x, y).amount--;
            if (GetItem(x, y).amount == 0) {
                RemoveItem(x, y);
            }
            OnGridChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public void RemoveItem(int x, int y) {
        SetItem(null, x, y);
    }

    public void RemoveItemEquipment(Item item) {
        if (item == outputItem) {
            // Removed output item
            ConsumeRecipeItems();
            CreateOutput();
            OnGridChanged?.Invoke(this, EventArgs.Empty);
        } else {
            // Removed item from grid
            for (int x = 0; x < GRID_SIZE; x++) {
                for (int y = 0; y < GRID_SIZE; y++) {
                    if (GetItem(x, y) == item) {
                        // Removed this one
                        RemoveItem(x, y);
                    }
                }
            }
        }
    }

    // khi item duoc keo tha vao grid_ cu the trong UI_craftingItemSystem
    public bool TryAddItem(Item item, int x, int y) {
        if (IsEmpty(x, y)) {
            SetItem(item, x, y);
            return true;
        } else {
            if (item.itemScriptableObject == GetItem(x, y).itemScriptableObject) {
                IncreaseItemAmount(x, y);
                return true;
            } else {
                return false;
            }
        }
    }

    public void AddItemEquipment(Item item) {}
    public bool CanAddItemEquipment() {return false;}
    public void AddItemAfterSliting(Item item) {}

    private ItemScriptableObject GetRecipeOutPut() {
        foreach (RecipeScriptableObject recipeScriptableObject in recipeScriptableObjectList){
            //Item.ItemType[,] recipe = recipeDictionary[recipeItemType];

            bool completeRecipe = true;
            for (int x = 0; x < GRID_SIZE; x++) {
                for (int y = 0; y < GRID_SIZE; y++) {
                    if(recipeScriptableObject.GetItem(x,y) != null) {
                        // Recipe has Item in this position
                        if (IsEmpty(x, y) || GetItem(x, y).itemScriptableObject != recipeScriptableObject.GetItem(x,y)) {
                            // Empty position or different itemType
                            //return Item.ItemType.None;
                            completeRecipe = false;
                        }
                    }
                }
            }

            if (completeRecipe) {
                return recipeScriptableObject.output;
            }
        }
        return null;
    }

    private void CreateOutput() {
        ItemScriptableObject recipeOutPut = GetRecipeOutPut();

        if (recipeOutPut == null) {
            outputItem = null;
        } else {
            outputItem = new Item { itemScriptableObject = recipeOutPut};
            outputItem.SetItemHolder(this);
        }
    }
    public Item GetOutputItem() {
        return outputItem;
    }

    public void ConsumeRecipeItems() {
        for (int x = 0; x < GRID_SIZE; x++) {
            for (int y = 0; y < GRID_SIZE; y++) {
                DecreaseItemAmount(x, y);
            }
        }
    }


    

}
