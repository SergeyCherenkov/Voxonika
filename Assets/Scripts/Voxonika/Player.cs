using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private int selectedSlot;
    private Inventory inventory;
    [Header("Survival Info")]
    [SerializeField] private float health;
    [SerializeField] private float foodPoint;
    [SerializeField] private float saturation;
    [Header("Items")]
    [SerializeField] private Transform itemHandler;

    private void Start() 
    {
        inventory = new Inventory();
        Debug.Log(inventory.items[selectedSlot]);
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            ScreenCapture.CaptureScreenshot("screenshot_Vaxonika_" + System.DateTime.Now.ToString("dd.MM.yyyy.hh_mm_ss") + ".png", 4);
        }
    }

    public void ChangeSlot(int indexSlot)
    {
        selectedSlot = indexSlot;
    }
}
