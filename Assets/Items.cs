using UnityEngine;

[System.Serializable]
public class Items
{
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable;

    // Additional fields like quantity or type can be added
    public Items(string name, Sprite icon, bool stackable)
    {
        itemName = name;
        itemIcon = icon;
        isStackable = stackable;
    }
}
