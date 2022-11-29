using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum Ÿ���� ����� �������̴�.
public enum ItemType
{
    equipable,  // ��� ����, ��� ������ ��� ����.
    Consumable, // �Ҹ�ǰ, �����̳� ���İ��� �Ҹ�ǰ��.
    Resource    // �ڿ���.
}


// A ScriptableObject is a data container that you can use to save large amounts of data, 
// independent of class instances. One of the main use cases for ScriptableObjects is to 
// reduce your Project's memory usage by avoiding copies of values. This is useful if your Project has a Prefab.

// ScriptableObject(��ũ��Ʈ ������ ������Ʈ)��
// ���� ���� data �� �����ϴ� �� ����� �� �ִ� data container �̸�,
// class instances �� �������Դϴ�.
// ScriptableObjects �� �ֿ� ��� ��� �� �ϳ���
// values, ������ copy �� �������ν� Project's �� Reduce memory usage. �޸� ��뷮�� ���Դϴ�.

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
