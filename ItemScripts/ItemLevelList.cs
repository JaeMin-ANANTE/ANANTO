using System.Collections;
using UnityEngine;

[System.Serializable]
public class ItemAttributeEntry
{
    public int Level;
    public int Value;
}

[System.Serializable]
public class ItemAttributeList
{
    public ItemAttributeEntry[] DurationByLevel;
    public ItemAttributeEntry[] SpeedByLevel;
    public ItemAttributeEntry[] CountByLevel;
}

[System.Serializable]
public class ItemLevelEntry
{
    public int ID;
    public ItemAttributeList Attributes;
}

[System.Serializable]
public class ItemLevelList {
    public ItemLevelEntry[] ItemLevelData;
}


