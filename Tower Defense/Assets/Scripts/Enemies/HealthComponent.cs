using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private float value;
    private Action onHealthDepleted;

    public Action OnHealthDepleted { get => onHealthDepleted; set => onHealthDepleted = value; }

    public float GetValue()
    {
        return value;
    }

    public void SetValue(float value)
    {
        this.value = value;
    }

    public void Damage(float damageValue)
    {
        value -= damageValue;

        if (value <= 0.0f)
            onHealthDepleted?.Invoke();
    }
}
