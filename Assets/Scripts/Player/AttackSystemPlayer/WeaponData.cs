using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/Weapos")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject projectilePrefab;
    public float fireRate;
    public int maxAmmo;
    public bool isAutomatic;
}
