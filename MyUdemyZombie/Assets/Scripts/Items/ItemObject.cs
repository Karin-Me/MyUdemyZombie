using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData item;

    public string GetInteractPromp()
    {
        // show the name of item when our haircross is on the object
        // ���ũ�ν��� ������Ʈ ���� ���� �� �������� �̸��� �����ݴϴ�.
        return string.Format("pickup {0}", item.ItemName);
    }

    public void OnInteract()
    {
        Inventory.instance.AddItem(item);
        // destroy game object after interacting with it
        // ���� ������Ʈ�� ��ȣ�ۿ��� �� �ı���Ű�� �޼����Դϴ�.
        Destroy(gameObject);
    }
}
