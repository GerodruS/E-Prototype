using System.Collections;
using UnityEngine;

public class PigeonCollider : MonoBehaviour
{
    public System.Action<GameObject> CallbackFinish;

    public GameObject holder;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Colliders/Hero"))
        {
            if (holder != null)
            {
                holder.SetActive(false);
                if (CallbackFinish != null)
                {
                    CallbackFinish.Invoke(gameObject);
                }
            }
        }
    }
}