using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Weapon[] weapons; 
    private int currentWeaponIndex = 0;

    public Weapon GetCurrentWeapon()
    {
        return weapons[currentWeaponIndex];
    }

    public void SwitchWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weapons.Length)
        {
            currentWeaponIndex = weaponIndex;
            Debug.Log("Switched to weapon: " + weapons[currentWeaponIndex].name);
        }
    }

    public void PickUpWeapon(Weapon newWeapon)
    {
        
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                weapons[i] = newWeapon;
                Debug.Log("Picked up weapon: " + newWeapon.name);
                return;
            }
        }
        Debug.Log("Inventory is full!");
    }
}