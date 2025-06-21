using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int maxHealth { set; get; }
    int currentHealth { set; get; }
    public void TakeDamage(int damage);
    bool IsDead { set; get; }
    void CheckIsDead();
    bool isDamage { set; get; }

}
