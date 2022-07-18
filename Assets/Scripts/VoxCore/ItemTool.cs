using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tool", fileName = "New Tool", order = 51)]
public class ItemTool : Item
{
    [SerializeField] private int damage;
    [SerializeField] private int strength;
}
