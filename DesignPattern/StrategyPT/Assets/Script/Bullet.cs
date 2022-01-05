using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : IWeapon
{
    public void Attack()
    {
        Debug.Log("Bullet");
    }
}
