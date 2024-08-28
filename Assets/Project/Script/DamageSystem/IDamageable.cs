using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public bool IsPlayer { get; }
    public bool IsDead { get; }
    public bool CanTakeDamage { get; set; }
    public void TakeDamage(DamageInfo info);
    public void Death();
    public Vector3 Position { get;  }
}
