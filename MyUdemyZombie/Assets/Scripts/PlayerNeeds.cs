using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerNeeds : MonoBehaviour, IDamageable
{
    public Need health;
    public Need hunger;
    public Need thirst;
    public float NoHungerHPDecay;
    public float noThirstHPDecay;
    public UnityEvent getDamage;

    private void Start()
    {
        // current value is equal to the start value
        // 현재값은 시작값과 같습니다.
        health.currentValue = health.startValue;
        hunger.currentValue = hunger.startValue;
        thirst.currentValue = thirst.startValue;
    }

    private void Update()
    {
        // reduce the value over time
        // 시간이 지남에 따라 값을 줄입니다.
        hunger.Subtract(hunger.decayRate * Time.deltaTime);
        thirst.Subtract(thirst.decayRate * Time.deltaTime);

        // check if hunger bar reach zero then
        //  hunger 막대가 0에 도달하면 
        if (hunger.currentValue == 0.0f)
        {
            // reduce health depend on the value of noHungerHPDecay
            // noHungerHPDecay 값에 따라 health 감소
            health.Subtract(NoHungerHPDecay * Time.deltaTime);
        }

        // check if thirst bar reach zero then
        //  thirst 막대가 0에 도달하면 
        if (thirst.currentValue == 0.0f)
        {
            thirst.Subtract(noThirstHPDecay * Time.deltaTime);
        }
        // check if player health reached to zero then call Did funtion
        // 플레이어의 체력이 0에 도달했는지 확인한 다음 Did 함수를 호출합니다.
        if (health.currentValue == 0.0f)
        {
            Die();
        }

        // update sliders
        // 슬라이더 업데이트
        health.uiSlider.fillAmount = health.GetPercentage();
        hunger.uiSlider.fillAmount = hunger.GetPercentage();
        thirst.uiSlider.fillAmount = thirst.GetPercentage();
    }

    public void Heal(float amount)
    {
        // add to health depend on that amount
        // amount 에 따라 health 가 추가됩니다..
        health.Add(amount);
    }

    public void Hunger(float amount)
    {
        hunger.Add(amount);
    }

    public void Thirst(float amount)
    {
        thirst.Add(amount);
    }

    public void TakeDamage(int damageAmount)
    {
        // reduce health depend on damage amount
        // damage 에 따라 health 가 감소합니다.
        health.Subtract(damageAmount);
        // if we get damage then invoke
        // damage 를 받으면 invoke 함수를 호출합니다.
        getDamage?.Invoke();
    }

    public void Die()
    {
        Debug.Log("Player Died");        

    }
}

[System.Serializable]
public class Need
{
    public float currentValue, maxValue,
                 startValue, regenerateRate, decayRate;
    public Image uiSlider;

    public void Add(float amount)
    {
                        // surrent value is the maximum value of 2 number
                        // 현재값은 최소값의 2 숫자입니다.
                       // (Mathf.Min 둘 이상의 값 중 가장 작은 값을 반환합니다.)
        currentValue = Mathf.Min(currentValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
                        // surrent value is the maximum value of 2 number
                        // 현재값은 최대값의 2 숫자입니다.
                       // (Mathf.Max 둘 이상의 값 중 가장 큰 값을 반환합니다.)
        currentValue = Mathf.Max(currentValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        // return us the result divided between current value and max value
        // 현재 값과 최대 값 사이에서 나눈 결과를 반환합니다.
        return currentValue / maxValue;
    }

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

