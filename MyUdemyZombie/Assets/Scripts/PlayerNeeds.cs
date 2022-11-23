using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerNeeds : MonoBehaviour
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
        // ���簪�� ���۰��� �����ϴ�.
        health.currentValue = health.startValue;
        hunger.currentValue = hunger.startValue;
        thirst.currentValue = thirst.startValue;
    }

    private void Update()
    {
        // reduce the value over time
        // �ð��� ������ ���� ���� ���Դϴ�.
        hunger.Subtract(hunger.decayRate * Time.deltaTime);
        thirst.Subtract(thirst.decayRate * Time.deltaTime);

        // check if hunger bar reach zero then
        //  hunger ���밡 0�� �����ϸ� 
        if (hunger.currentValue == 0.0f)
        {
            // reduce health depend on the value of noHungerHPDecay
            // noHungerHPDecay ���� ���� health ����
            health.Subtract(NoHungerHPDecay * Time.deltaTime);
        }

        if (thirst.currentValue == 0.0f)
        {
            thirst.Subtract(noThirstHPDecay * Time.deltaTime);
        }
        // check if player health reached to zero then call Did funtion
        // �÷��̾��� ü���� 0�� �����ߴ��� Ȯ���� ���� Did �Լ��� ȣ���մϴ�.
        if (health.currentValue == 0.0f)
        {
            Die();
        }

        // update sliders
        health.uiSlider.fillAmount = health.GetPercentage();
        hunger.uiSlider.fillAmount = hunger.GetPercentage();
        thirst.uiSlider.fillAmount = thirst.GetPercentage();
    }

    public void Heal(float amount)
    {
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
        health.Subtract(damageAmount);
    }

    public void Die()
    {
        Debug.Log("Player Died");
        getDamage?.Invoke();

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
                       // �� �̻��� �� �� ���� ���� ���� ��ȯ�մϴ�.
        currentValue = Mathf.Min(currentValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
                       // �� �̻��� �� �� ���� ū ���� ��ȯ�մϴ�.
        currentValue = Mathf.Max(currentValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        return currentValue / maxValue;
    }

}