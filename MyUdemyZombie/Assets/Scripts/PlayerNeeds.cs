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
        // 플레이어의 체력이 0에 도달했는지 확인한 다음 Die 함수를 호출합니다.
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

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Drink(float amount)
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
    
