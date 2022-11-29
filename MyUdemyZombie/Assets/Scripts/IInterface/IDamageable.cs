using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int DamageAomunt);
}
// What is the interface???

// Interfaces are a set of methods, properties, and other members that a target class must implement.
// C# is a type-safe language, meaning it checks if every statement is using Data Types correctly,
// by using an interface as your Data Type, you are able to access the interface fields on different classes.
// The idea is to have multiple classes that implement the IDamageable class, 
// so we can reference the Interface rather than the classes themselves.
// In this example, we could have the Player, Enemies, and Props all implement the IDamegeable, 
// this way if we get a collision in Unity,
// we check if it is IDmageable, and apply corresponding damage, regardless of its class.

// Interface 는 대상 class 가 구현해야 하는 methods, properties, 그리고 other members 의 집합입니다.
// C#은 type 이 안전한 언어이며, 즉 모든 Data Type 이 올바르게 쓰이는지 확인하며, 
// interface 를 Data Type 으로 사용하면 다른 class 의 interface fields 에 access 할 수 있습니다.
// The idea 는(idea 뜻이 애매모호하여 그대로 적었습니다.)
// Idamageable classes 를 구현하는 multiple classes 를 갖는 집합체입니다.
// 따라서 classes 자체가 아닌 Interface 자체를 reference, 참조 할 수 있습니다.
// 이 예시에서는 Player, Enemies 및 Props 가 모두 IDamegeable 로 구현하도록 할 수 있습니다.
// 이 방법으로 Unity 에서 collision 이 발생하면
// IDmageable 에서 확인하고 해당 class 에 관계없이 damage 를 적용합니다.