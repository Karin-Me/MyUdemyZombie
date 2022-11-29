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

// Interface �� ��� class �� �����ؾ� �ϴ� methods, properties, �׸��� other members �� �����Դϴ�.
// C#�� type �� ������ ����̸�, �� ��� Data Type �� �ùٸ��� ���̴��� Ȯ���ϸ�, 
// interface �� Data Type ���� ����ϸ� �ٸ� class �� interface fields �� access �� �� �ֽ��ϴ�.
// The idea ��(idea ���� �ָŸ�ȣ�Ͽ� �״�� �������ϴ�.)
// Idamageable classes �� �����ϴ� multiple classes �� ���� ����ü�Դϴ�.
// ���� classes ��ü�� �ƴ� Interface ��ü�� reference, ���� �� �� �ֽ��ϴ�.
// �� ���ÿ����� Player, Enemies �� Props �� ��� IDamegeable �� �����ϵ��� �� �� �ֽ��ϴ�.
// �� ������� Unity ���� collision �� �߻��ϸ�
// IDmageable ���� Ȯ���ϰ� �ش� class �� ������� damage �� �����մϴ�.