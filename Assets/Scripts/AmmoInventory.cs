using System.Collections.Generic;
using UnityEngine;

public class AmmoInventory : MonoBehaviour
{
    private Dictionary<AmmoType, int> ammoDict = new Dictionary<AmmoType, int>();

    private void Awake()
    {
        
        foreach (AmmoType type in System.Enum.GetValues(typeof(AmmoType)))
        {
            ammoDict[type] = 0;
        }
    }

    public void AddAmmo(AmmoType type, int amount)
    {
        ammoDict[type] += amount;
        Debug.Log($"Added {amount} ammo for {type}. Total: {ammoDict[type]}");
    }

    public int GetAmmo(AmmoType type)
    {
        return ammoDict[type];
    }

    public bool UseAmmo(AmmoType type, int amount)
    {
        Debug.Log("Initiol : " + ammoDict[type]);
        
        if (ammoDict[type] >= amount)
        {

            ammoDict[type] -= amount;
            Debug.Log("After : " + ammoDict[type]);
            return true;
        }
        else
        {

            ammoDict[type] = 0;
            return true;
        }
    }
}
