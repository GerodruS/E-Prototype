using UnityEngine;

public class HeroCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Barrier"))
        {
            Debug.Log("Boom!");
        }
    }
}