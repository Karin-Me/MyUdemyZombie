using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] uiSlots;        // modifying slot ui
    public ItemSlot[] slots;            // holding slot data
    public GameObject inventoryWindow;  // to open and close inventory
    public Transform dropPosition;      // when we drop the item, that item will drop in that posion

    [Header("Selected Items")]
    private ItemSlot selectedItem;      // selecting the item
    private int selectedItemIndex;      // getting the index number of that item
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemdescription;
    public TextMeshProUGUI selectedItemAStatName, selectedItemBStatName;
    public TextMeshProUGUI selectedItemAStatValue, selectedItemBStatValue; 
    public GameObject useButton;
    public GameObject dropButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    private PlayerController controller;
    private int curEquipIndex;
    [Header("Events")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    // singleton 싱글턴
    public static Inventory instance;

    private void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        // Close the inventory window when play
        inventoryWindow.SetActive(false);
        // same size of our ui slots
        slots = new ItemSlot[uiSlots.Length];

        // initialize the slots 슬롯 초기화
        for (int x = 0; x < slots.Length; x++)
        {
            // creating new instance of item slot for that specefic slot in the array
            // 배열의 해당 특정 슬롯에 대한 아이템 슬롯의 새 인스턴스 생성
            slots[x] = new ItemSlot();
            uiSlots[x].index = x;
            uiSlots[x].ClearSlot();
        }

        ClearSelectedItemWindow();
    }

    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        Toggle();
    }

    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
            onCloseInventory.Invoke();
            controller.ToggleCurcor(false);
        }else
        {
            inventoryWindow.SetActive(true);
            onOpenInventory.Invoke();
            ClearSelectedItemWindow();
            controller.ToggleCurcor(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem(ItemData item)
    {
        if (item.canStack)
        {
            // slotToStackTo is going to find an item that we can stack that item to it
            ItemSlot slotToStackTo = GetItemSTack(item);

            // check if that item exist
            if (slotToStackTo != null)
            {
                // then we are going to add to that item quantity and updating UI
                slotToStackTo.quantity ++;
                UpdateUI();
                return;
            }
        }        

        ItemSlot emptySlot = GetEmptySlot();

        // if empty slot exist then 
        if (emptySlot != null)
        {
            // add the item to it
            emptySlot.item = item;
            // add 1 to quantity
            emptySlot.quantity = 1;
            // update UI
            UpdateUI();
            return;
        }
        // if we dont have empty slot then we are going to throw that item
        ThrowItem(item);
    }

    void ThrowItem(ItemData item)
    {
        // instantiate an item from prefab and throw that item in drop position
        // 프리팹에서 항목을 인스턴스화하고 드롭 위치에 해당 항목을 집어넣습니다.
        Instantiate(item.dropPrefab, dropPosition.position, dropPosition.rotation);
    }

    void UpdateUI()
    {
        // loop through all our slot
        for (int x = 0; x < slots.Length; x++)
        {
            // check if slot has item
            if (slots[x].item != null)
            {
                // set the item
                uiSlots[x].Set(slots[x]);
            }else    // if there is not item inside
            {
                // clear that slot
                uiSlots[x].ClearSlot();
            }
        }
    }

    ItemSlot GetItemSTack(ItemData item)
    {
        // loop through all our slot
        for (int x = 0; x < slots.Length; x++)
        {

            // check if its same item that we want to stack and also quantity of that item is not max
            //  확인합니다. 우리는 아이템수량이 최대치에 도달하기 전까지 아이템수량이 쌓이는걸 원합니다.(의역하였습니다.)
            if (slots[x].item == item && slots[x].quantity < item.maxStackAmount)
            {
                // return the item slot
                // 아이템 슬롯을 반환합니다.
                return slots[x];
            }
        }

        // if there is on item to stack just return null
        // item to stack 이면 (아이템이 쌓이면) null 을 반환합니다.
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        // loop through all our slot
        for (int x = 0; x < slots.Length; x++)
        {

            // check if its same item that we want to stack and also quantity of that item is not max
            // 우리는 아이템수량이 최대치 도달하기 전까지 아이템수량이 쌓이는걸 원합니다.(의역하였습니다.)
            if (slots[x].item == null)
            {
                // return the item slot
                // 아이템 슬롯을 반환합니다.
                return slots[x];
            }
        }

        // if there is on item just return null
        // item 이면 null 을 반환합니다.
        return null;
    }

    public void SelectItem(int index)
    {
        // if we dont have any item in inventory slot dont do anything
        // inventory slot 에 아이템이 없으면 아무 작업도 하지 않습니다.
        if (slots[index].item == null)
        {
            return;
        }
            

        // selected item is equal to index of that slot
        // selecteditem 은 해당 selectedItemIndex 의 index 와 같습니다.
        selectedItem = slots[index];
        selectedItemIndex = index;

        // set the name and description of our item
        // item name 과 description 을 설정합니다.
        selectedItemName.text = selectedItem.item.ItemName;
        selectedItemdescription.text = selectedItem.item.itemDescribtion;

        // set stat value and stat name

        // activate use button if selected item is consumable
        // 선택한 item 이 consumable(소모품)일 경우 button 을 활성화합니다.
        // ItemType.Consumable 은 ItemData 스크립트의 public enum ItemType 에서 가져옵니다.
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);

        // activate equip button if selected item is equipable type and still we didnt already equip it
        // 선택한 item 이 equipable(장착가능) 한 type 이고
        // 아직 equip 하지 않은 경우 equipbutton 을 활성화합니다.
        // ItemType.equipable 은 ItemData 스크립트의 public enum ItemType 에서 가져옵니다.
        equipButton.SetActive(selectedItem.item.type == ItemType.equipable && !uiSlots[index].equipped);

        // activate unequip button if selected item is equipable type and we already equip it
        // 선택한 item 이 equipable 한 type 이고
        // 이미 equip 한 경우 unequipbutton 을 활성화합니다.
        // ItemType.equipable 은 ItemData 스크립트의 public enum ItemType 에서 가져옵니다.
        unequipButton.SetActive(selectedItem.item.type == ItemType.equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    void ClearSelectedItemWindow()
    {
        // clear the text elements
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemdescription.text = string.Empty;
        selectedItemAStatName.text = string.Empty;
        selectedItemBStatName.text = string.Empty;
        selectedItemAStatValue.text = string.Empty;
        selectedItemBStatValue.text = string.Empty;

        // disable all buttons 모든 버튼 비활성화
        dropButton.SetActive(false);
        useButton.SetActive(false);
        unequipButton.SetActive(false);
        equipButton.SetActive(false);
    }

    public void OnUseButton()
    {
        
    }

    public void OnEquipButton()
    {

    }

    public void OnUnequipButton()
    {

    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }

    private void UnEquip(int index)
    {

    }

    void RemoveSelectedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity == 0)
        {
            if (uiSlots[selectedItemIndex].equipped == true)
            {
                UnEquip(selectedItemIndex);
            }
                selectedItem.item = null;
                ClearSelectedItemWindow();
            

            UpdateUI();
        }
    }

    public void RemoveItem(ItemData item)
    {

    }

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }
}

public class ItemSlot
{
    public ItemData item;
    public int quantity;
}
