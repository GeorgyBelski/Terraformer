using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void ApplyDamage(int value, Vector3 shootPoint, Vector3 direction);
    float GetHealthRatio();
    Vector3 GetPosition();
}
