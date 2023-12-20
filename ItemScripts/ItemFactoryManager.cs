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
        spriteRenderer.sortingOrder = 1;

        itemObject.transform.position = position;
        itemObject.tag = "Item";

        switch (id) {
            case 0:
                itemObject.name = "FlowerBomb";
                CircleCollider2D circleCollider0 = itemObject.AddComponent<CircleCollider2D>();
                circleCollider0.radius = 0.85f;
                itemObject.AddComponent<FlowerBombItem>();
                break;
            case 1:
                itemObject.name = "FlowerShield";
                CircleCollider2D circleCollider1 = itemObject.AddComponent<CircleCollider2D>();
                circleCollider1.radius = 0.7f;
                itemObject.AddComponent<FlowerShieldItem>();
                break;
            case 2:
                itemObject.name = "Bee";
                BoxCollider2D boxCollider2 = itemObject.AddComponent<BoxCollider2D>();
                itemObject.AddComponent<BeeItem>();
                break;
            case 3:
                itemObject.name = "MiniBug";
                CircleCollider2D circleCollider3 = itemObject.AddComponent<CircleCollider2D>();
                itemObject.AddComponent<MiniBugItem>();
                break;
            case 7:
                itemObject.name = "Cosmos2";
                CircleCollider2D circleCollider7 = itemObject.AddComponent<CircleCollider2D>();
                itemObject.AddComponent<Cosmos2Item>();
                break;
        }

        return itemObject;
    }
}
//¿ŒªÁ«œ∞Ì ¿Ã∏ß. ±ËπŒ√∂ µÂ∏≤.