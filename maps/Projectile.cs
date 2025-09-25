using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;

    private void Update()
    {
        // placeholder: movimento proiettile
    }

    private void DealDamage(GameObject target)
    {
        Debug.Log($"{name} deals {damage} damage to {target.name}");
        // qui aggiungerai logica di vita/danno
    }

    private void PaintCells()
    {
        Debug.Log($"{name} paints cells on impact");
        // qui aggiungerai logica di colorazione griglia
    }
}
