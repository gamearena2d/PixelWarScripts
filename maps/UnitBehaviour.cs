using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float attackRange = 1f;
    public int damage = 10;

    private void Update()
    {
        MoveTowardsTarget();
        AttackIfInRange();
    }

    private void MoveTowardsTarget()
    {
        if (target == null) return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    private void AttackIfInRange()
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= attackRange)
        {
            Debug.Log($"{name} attacks {target.name} for {damage} damage");
            // qui chiamerai DealDamage su target
        }
    }
}
