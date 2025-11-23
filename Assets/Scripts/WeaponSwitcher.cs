using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int currentWeaponIndex = 0;
    void Start()
    {
        SetWeaponActive();
    }

    // Update is called once per frame
    void Update()
    {
        int prevWeapon = currentWeaponIndex;
        ProcessKeyInput();
        ProcessScorll();

        if(currentWeaponIndex != prevWeapon)
        {
            SetWeaponActive();
        }
        
    }
    void SetWeaponActive()
    {
        int weaponIndex = 0;
        foreach (Transform weapon in transform)
        {
            if (weaponIndex == currentWeaponIndex)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            weaponIndex++;
        }
    }

    void ProcessKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            //SetWeaponActive();
            currentWeaponIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount > 1)
        {
            //SetWeaponActive();
            currentWeaponIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >2)
        {
            //SetWeaponActive();
            currentWeaponIndex = 2;
        }
    }

    void ProcessScorll()
    {
        if(Input.mouseScrollDelta.y < 0f)
        {
            if(currentWeaponIndex >= transform.childCount - 1)
            {
                currentWeaponIndex = 0;
            }
            else
            {
                currentWeaponIndex++;
            }
        }
        if(Input.mouseScrollDelta.y > 0f)
        {
            if(currentWeaponIndex <= 0)
            {
                currentWeaponIndex = transform.childCount - 1;
            }
            else
            {
                currentWeaponIndex --;
            }
        }
    }
}
