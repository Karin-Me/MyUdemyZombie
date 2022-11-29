using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum 타입은 상수의 열거형이다.
public enum ItemType
{
    equipable,  // 장비 가능, 사용 가능한 장비 같다.
    Consumable, // 소모품, 물약이나 음식같은 소모품들.
    Resource    // 자원들.
}


// A ScriptableObject is a data container that you can use to save large amounts of data, 
// independent of class instances. One of the main use cases for ScriptableObjects is to 
// reduce your Project's memory usage by avoiding copies of values. This is useful if your Project has a Prefab.

// ScriptableObject(스크립트 가능한 오브젝트)는
// 많은 양의 data 를 저장하는 데 사용할 수 있는 data container 이며,
// class instances 와 독립적입니다.
// ScriptableObjects 의 주요 사용 사례 중 하나는
// values, 값들의 copy 를 피함으로써 Project's 의 Reduce memory usage. 메모리 사용량을 줄입니다.

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Information")]
    public string ItemName;
    public string itemDescribtion;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Staking Items")]
    public bool canStack;
    public int maxStackAmount;

}   
