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
        // destroy game object after interacting with it
        Destroy(gameObject);
    }
}
