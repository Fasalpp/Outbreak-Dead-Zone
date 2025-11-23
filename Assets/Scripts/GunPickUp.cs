using KINEMATION.FPSAnimationPack.Scripts.Player;
using UnityEngine;

public class GunPickUp : MonoBehaviour
{
    public int weaponIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FPSPlayer player = other.GetComponentInChildren<FPSPlayer>();
            Debug.Log(player);
            if (player != null)
            {
                player.PickupWeapon(weaponIndex);
                Destroy(gameObject.transform.root.gameObject); 
            }
        }
    }
}
