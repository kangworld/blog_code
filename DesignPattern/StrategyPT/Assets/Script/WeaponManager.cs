using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private MyWeapon _myWeapon;

    void Start()
    {
        _myWeapon = new MyWeapon();
        _myWeapon.Weapon = new Bullet();
    }

    public void ChangeBullet()
    {
        _myWeapon.Weapon = new Bullet();
    }

    public void ChangeMissile()
    {
        _myWeapon.Weapon = new Missile();
    }
    public void ChangeArrow()
    {
        _myWeapon.Weapon = new Arrow();
    }

    public void Attack()
    {
        _myWeapon.Attack();
    }
}
