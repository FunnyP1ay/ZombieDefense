using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Weapon : MonoBehaviour
{
    public float FireRate;
    public float Damage;
    public float detectionRange = 10f;  // Å½Áö °Å¸®
    public Transform FirePosition;

    public virtual void Using(Character target)
    {
        target.TakeDamage(Damage);
    }

}
