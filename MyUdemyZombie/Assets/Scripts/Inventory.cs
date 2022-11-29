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

    // singleton �̱���
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

        // initialize the slots ���� �ʱ�ȭ
        for (int x = 0; x < slots.Length; x++)
        {
            // creating new instance of item slot for that specefic slot in the array
            // �迭�� �ش� Ư�� ���Կ� ���� ������ ������ �� �ν��Ͻ� ����
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
        // �����տ��� �׸��� �ν��Ͻ�ȭ�ϰ� ��� ��ġ�� �ش� �׸��� ����ֽ��ϴ�.
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
            //  Ȯ���մϴ�. �츮�� �����ۼ����� �ִ�ġ�� �����ϱ� ������ �����ۼ����� ���̴°� ���մϴ�.(�ǿ��Ͽ����ϴ�.)
            if (slots[x].item == item && slots[x].quantity < item.maxStackAmount)
            {
                // return the item slot
                // ������ ������ ��ȯ�մϴ�.
                return slots[x];
            }
        }

        // if there is on item to stack just return null
        // item to stack �̸� (�������� ���̸�) null �� ��ȯ�մϴ�.
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        // loop through all our slot
        for (int x = 0; x < slots.Length; x++)
        {

            // check if its same item that we want to stack and also quantity of that item is not max
            // �츮�� �����ۼ����� �ִ�ġ �����ϱ� ������ �����ۼ����� ���̴°� ���մϴ�.(�ǿ��Ͽ����ϴ�.)
            if (slots[x].item == null)
            {
                // return the item slot
                // ������ ������ ��ȯ�մϴ�.
                return slots[x];
            }
        }

        // if there is on item just return null
        // item �̸� null �� ��ȯ�մϴ�.
        return null;
    }

    public void SelectItem(int index)
    {
        // if we dont have any item in inventory slot dont do anything
        // inventory slot �� �������� ������ �ƹ� �۾��� ���� �ʽ��ϴ�.
        if (slots[index].item == null)
        {
            return;
        }
            

        // selected item is equal to index of that slot
        // selecteditem �� �ش� selectedItemIndex �� index �� �����ϴ�.
        selectedItem = slots[index];
        selectedItemIndex = index;

        // set the name and description of our item
        // item name �� description �� �����մϴ�.
        selectedItemName.text = selectedItem.item.ItemName;
        selectedItemdescription.text = selectedItem.item.itemDescribtion;

        // set stat value and stat name

        // activate use button if selected item is consumable
        // ������ item �� consumable(�Ҹ�ǰ)�� ��� button �� Ȱ��ȭ�մϴ�.
        // ItemType.Consumable �� ItemData ��ũ��Ʈ�� public enum ItemType ���� �����ɴϴ�.
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);

        // activate equip button if selected item is equipable type and still we didnt already equip it
        // ������ item �� equipable(��������) �� type �̰�
        // ���� equip ���� ���� ��� equipbutton �� Ȱ��ȭ�մϴ�.
        // ItemType.equipable �� ItemData ��ũ��Ʈ�� public enum ItemType ���� �����ɴϴ�.
        equipButton.SetActive(selectedItem.item.type == ItemType.equipable && !uiSlots[index].equipped);

        // activate unequip button if selected item is equipable type and we already equip it
        // ������ item �� equipable �� type �̰�
        // �̹� equip �� ��� unequipbutton �� Ȱ��ȭ�մϴ�.
        // ItemType.equipable �� ItemData ��ũ��Ʈ�� public enum ItemType ���� �����ɴϴ�.
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

        // disable all buttons ��� ��ư ��Ȱ��ȭ
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
