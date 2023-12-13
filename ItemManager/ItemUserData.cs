using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemUserEntry
{
    public int ID;
    public int Level;
    public int? duration;
    public int? speed;
    public int? count;
}

[System.Serializable]
public class ItemUserList
{
    public List<ItemUserEntry> itemUserData = new List<ItemUserEntry>();
}
