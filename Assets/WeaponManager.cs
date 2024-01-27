using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoSingleton<WeaponManager>
{
    public List<WeaponDef> WeaponDefs;

    public WeaponDef GetRandomWeapon()
    {
        return WeaponDefs[Random.Range(0, WeaponDefs.Count)];
    }
}
