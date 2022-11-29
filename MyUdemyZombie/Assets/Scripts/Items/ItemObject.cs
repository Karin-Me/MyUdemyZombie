using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData item;

    public string GetInteractPromp()
    {
        // show the name of item when our haircross is on the object
        // 헤어크로스가 오브젝트 위에 있을 때 아이템의 이름을 보여줍니다.
        return string.Format("pickup {0}", item.ItemName);
    }

    public void OnInteract()
    {
        Inventory.instance.AddItem(item);
        // destroy game object after interacting with it
        // 게임 오브젝트와 상호작용한 후 파괴시키는 메서드입니다.
        Destroy(gameObject);
    }
}
