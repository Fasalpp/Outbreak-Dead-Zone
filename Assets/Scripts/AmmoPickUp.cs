using KINEMATION.FPSAnimationPack.Scripts.Player;
using KINEMATION.FPSAnimationPack.Scripts.Weapon;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    public int bulletAmount = 10;
    public AmmoType ammoType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerSettings = other.GetComponentInChildren<FPSPlayer>();
            AmmoInventory inventory = other.GetComponentInChildren<AmmoInventory>();
            if (inventory != null)
            {
                inventory.AddAmmo(ammoType, bulletAmount);
            }
            if(playerSettings != null)
            {
                foreach(var weapon in playerSettings._weapons)
                {
                    
                    if(weapon != null)
                    {
                        weapon.InitializeAmmo(inventory);
                    }
                }
            }

            Destroy(gameObject);
        }
    }

    
}
