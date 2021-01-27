using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : ShipComponent
{
    private int weaponDamage;
    private int currentWeaponDamage;
    private int baseFireRate;
    private bool offCooldown;
    private float coolDownTimeRemaining;
    private float firingRange;
    
    public int WeaponDamage { get => weaponDamage; set => weaponDamage = value; }
    public int CurrentWeaponDamage { get => currentWeaponDamage; set => currentWeaponDamage = value; }
    public int BaseFireRate { get => baseFireRate; set => baseFireRate = value; }
    public bool OffCooldown { get => offCooldown; set => offCooldown = value; }
    public float CoolDownTimeRemaining { get => coolDownTimeRemaining; set => coolDownTimeRemaining = value; }
    public float FiringRange { get => firingRange; set => firingRange = value; }
    public WeaponComponent() {
        weaponDamage = currentWeaponDamage = 10;

        baseFireRate = 1;
        firingRange = 50;

        offCooldown = true;
    }

    public void ReduceCoolDownTime() {
        if (!offCooldown && coolDownTimeRemaining < baseFireRate) {
            coolDownTimeRemaining += Time.deltaTime;
        }
        else {
            offCooldown = true;
            coolDownTimeRemaining = 0;
        }
    }
}
