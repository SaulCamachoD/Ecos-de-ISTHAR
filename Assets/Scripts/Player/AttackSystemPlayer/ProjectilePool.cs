using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;
    private Dictionary<GameObject, Queue<GameObject>> projectilePools = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetProjectile(GameObject prefab)
    {
        if (!projectilePools.ContainsKey(prefab))
        {
            projectilePools[prefab] = new Queue<GameObject>();
        }

        Queue<GameObject> pool = projectilePools[prefab];
        if (pool.Count > 0)
        {
            GameObject projectile = pool.Dequeue();
            projectile.SetActive(true);
            return projectile;
        }
        else
        {
            return Instantiate(prefab);
        }
    }

    public void ReturnProjectile(GameObject projectile, GameObject prefab)
    {
        projectile.SetActive(false);

        if (!projectilePools.ContainsKey(prefab))
        {
            projectilePools[prefab] = new Queue<GameObject>();
        }

        projectilePools[prefab].Enqueue(projectile);
    }
}
