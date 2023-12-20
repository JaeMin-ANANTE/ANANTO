using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemTextEntry
{
    public int ID;
    public string Name_English;
    public string Name_Korean;
    public string PS_English;
    public string PS_Korean;
    public string Type;
}

[System.Serializable]
public class ItemTextList
{
    public ItemTextEntry[] ItemTextData;
}