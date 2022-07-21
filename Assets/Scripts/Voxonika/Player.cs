using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private int selectedSlot;
    public Inventory inventory;
    [SerializeField] private Image[] backgroundSlots;
    [SerializeField] private Image[] iconSlots;
    [SerializeField] private Color defaultBackground;
    [SerializeField] private Color selectBackground;
    [SerializeField] private GameObject fullInventory;

    [Header("Survival Info")]
    [SerializeField] private float health;
    [SerializeField] private float foodPoint;
    [SerializeField] private float saturation;

    [Header("Hand")]
    [SerializeField] private GameObject hand;
    [SerializeField] private Transform itemHandler;

    private void Start() 
    {
        inventory = new Inventory();
        ChangeSelectedItem(0);
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ScreenCapture.CaptureScreenshot("screenshot_Vaxonika_" + System.DateTime.Now.ToString("dd.MM.yyyy.hh_mm_ss") + ".png", 4);
        }

        if (Input.mouseScrollDelta.y > 0)
            ChangeSelectedItem(selectedSlot + 1);
        else if (Input.mouseScrollDelta.y < 0)
            ChangeSelectedItem(selectedSlot - 1);

        if (Input.GetKeyDown(KeyCode.E))
        {
            bool isOpen = fullInventory.activeSelf;
            Cursor.visible = !isOpen;
            fullInventory.SetActive(!isOpen);
        }

        if (Input.GetKeyDown(KeyCode.O))
            SetItem(Resources.Load<ItemTool>("Items/Tools/IronShovel"), 0);
    }

    public void SetItem(Item item, int indexSlot)
    {
        inventory.items[indexSlot] = item;

        iconSlots[indexSlot].sprite = item.icon;
        iconSlots[indexSlot].color = Color.white;

        if (indexSlot == selectedSlot)
            UpdateItem();
    }

    public void ChangeSelectedItem(int indexSlot)
    {
        backgroundSlots[selectedSlot].color = defaultBackground;

        if (indexSlot < 0)
            selectedSlot = 6;
        else    
            selectedSlot = indexSlot % 7; 

        backgroundSlots[selectedSlot].color = selectBackground;
        
        UpdateItem();
    }

    private void UpdateItem()
    {
        if (inventory.items[selectedSlot] == null)
        {
            hand.SetActive(false);

            for(int i = itemHandler.childCount - 1; i >= 0; i--)
                Destroy(itemHandler.GetChild(i).gameObject);
        }
        else
        {
            hand.SetActive(true);
            Instantiate(inventory.items[selectedSlot].itemPrefab).transform.SetParent(itemHandler, false);
        }
    }
}
