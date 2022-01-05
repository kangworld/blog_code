using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : IWeapon
{
    public void Attack()
    {
        Debug.Log("Missile");
    }
}
