using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeapon
{
    private IWeapon _weapon;

    public IWeapon Weapon
    { 
        get 
        {
            return _weapon;
        } 
        set 
        {
            _weapon = value;
        } 
    }

    public void Attack()
    {
        _weapon.Attack();
    }
}
