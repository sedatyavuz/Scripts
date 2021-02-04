using UnityEngine;

[CreateAssetMenu(menuName = "GameItems/GunItem")]
public class GunItem : BaseItem
{
    [Space]
    [Header("Gun Item Settings")]
    public GunType gunType;
    public int damage;
    [Range(0, 100)] public float accuracy;
    public float fireRate;
    public float bulletSpeed;
    public float reloadTime;
    public int magSize;
    public float range;
}
