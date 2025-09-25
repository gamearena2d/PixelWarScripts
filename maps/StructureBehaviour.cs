using UnityEngine;

public class StructureBehaviour : MonoBehaviour
{
    public float attackCooldown;
    public float range;
    public GameObject projectilePrefab;

    void Attack() { Instantiate(projectilePrefab); }
}
