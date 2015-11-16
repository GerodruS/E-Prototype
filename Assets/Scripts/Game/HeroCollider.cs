using UnityEngine;

public class HeroCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Barrier"))
        {
            Debug.Log("Boom!");
        }
        else if (other.CompareTag("Colliders/Pigeon"))
        {
            Debug.Log("Pigeon catched!");
        }
    }
}