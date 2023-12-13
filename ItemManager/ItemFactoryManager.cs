using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemFactoryManager
{
    private static ItemTable[] itemTable;
    public static void Initialize(ItemTable[] table)
    {
        itemTable = table;
    }
    
    public static GameObject CreateItem(int id, Vector3 position)
    {
        GameObject itemObject = new GameObject();
        ItemTable data = itemTable[id];
        SpriteRenderer spriteRenderer = itemObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.itemActiveSprite;

        itemObject.transform.position = position;
        itemObject.tag = "Item";

        switch (id) {
            case 0:
                itemObject.name = "FlowerBomb";
                CircleCollider2D circleCollider = itemObject.AddComponent<CircleCollider2D>();
                circleCollider.radius = 0.85f;
                itemObject.AddComponent<FlowerBombItem>();
                break;
        }

        return itemObject;
    }
}
