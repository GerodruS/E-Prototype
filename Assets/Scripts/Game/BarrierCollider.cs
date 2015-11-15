using UnityEngine;

public class BarrierCollider : MonoBehaviour
{
    public Mover target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        target.OnTriggerEnter2D(other);
    }
}