using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class ItemTable
{
    public int ID;
    public Sprite itemInActiveSprite;
    public Sprite itemActiveSprite;
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemTableData : ScriptableObject
{
    public ItemTable[] itemTableArray;

    public ItemTable GetItemData(int id)
    {
        foreach (ItemTable data in itemTableArray)
        {
            if (data.ID == id) return data;
        }
        return null;
    }
}
