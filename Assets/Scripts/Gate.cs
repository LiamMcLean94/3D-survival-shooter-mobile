using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public enum GateType { Duplication, Upgrade}
    public GateType gateType;
    public int requiredShots = 3;
    public int currentShots = 0;

    public void OnHitByBullet()
    {
        currentShots++;
        if(currentShots >= requiredShots)
        {
            if(gateType == GateType.Duplication)
            {
                DuplicatePlayer();
            }
            else if (gateType == GateType.Upgrade)
            {
                UpgradeWeapon();
            }
        }
        Destroy(gameObject);
    }

    void DuplicatePlayer()
    {
        //need to add some logic here
        Debug.Log("Player Duplicated!");
    }

    void UpgradeWeapon()
    {
        //need to add logic here
        Debug.Log("Upgraded Weapon");
    }




}
