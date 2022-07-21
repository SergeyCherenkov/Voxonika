using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Cummon", fileName = "New Item", order = 51)]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject itemPrefab;
    public int maxCount;
}
