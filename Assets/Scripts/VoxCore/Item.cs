using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Cummon", fileName = "New Item", order = 51)]
public class Item : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int maxCount;
}
