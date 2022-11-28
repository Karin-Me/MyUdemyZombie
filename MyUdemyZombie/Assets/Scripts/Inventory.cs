using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public ItemSlopUI[] uiSlots;        // modifying slot ui
    public ItemSlot[] slots;            // holding slot data
    public GameObject inventoryWindow;  // to open and close inventory
    public Transform dropPosition;      // when we drop the item, that item will drop in that posion

    [Header("Selected Items")]
    private ItemSlot selectedItem;      // selecting the item
    private int selectedItemIndex;      // getting the index number of that item
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemdescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject dropButton;
    public GameObject equipButton;
    public GameObject upequipButton;
    private PlayerController controller;
    private int curEquipIndex;
    [Header("Events")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    // singleton ╫л╠шео
    public static Inventory instance;

    private void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerController>();
    }

}

public class ItemSlot
{
    public ItemData item;
    public int quantity;
}
