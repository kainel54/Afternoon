using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;

public class Attack : Poolable
{
    public WeaponData weaponData;
    private void OnEnable()
    {
        Invoke("Release",weaponData.atkRemain);
    }

    public override void Release()
    {
        transform.parent = null;
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        base.Release();
    }

    public void SetWeaponData(WeaponData newData)
    {
        weaponData = new WeaponData();
        weaponData.atkDamage = 3f;
        weaponData.atkRemain = 0.5f;
        weaponData.coolTime = 1.35f;
        weaponData = newData;
    }
}
