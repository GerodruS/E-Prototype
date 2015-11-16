using System.Collections;
using UnityEngine;

public class PigeonController : MonoBehaviour
{
    [System.Serializable]
    public struct Pigeon
    {
        public GameObject holder;
        public Mover mover;
        public PigeonCollider collider;
    }

    public int seed = 0;
    public Pigeon[] pigeons;

    private int lastIndex = -1;

    private void Start()
    {
        Random.seed = seed;
        foreach (var p in pigeons)
        {
            p.holder.SetActive(false);
            p.mover.IsLoop = false;
        }
        Invoke("SpawnRandom", 1.0f);
    }

    private void SpawnRandom()
    {
        int index = Random.Range(0, pigeons.Length);
        while (index == lastIndex)
        {
            index = Random.Range(0, pigeons.Length);
        }
        lastIndex = index;
        var p = pigeons[index];
        p.holder.SetActive(true);
        p.mover.Reset();
        p.mover.CallbackFinish += OnFinish;
        p.collider.CallbackFinish += OnFinish;
    }

    private void OnFinish(GameObject pigeon)
    {
        foreach (var p in pigeons)
        {
            p.holder.SetActive(false);
            p.mover.CallbackFinish -= OnFinish;
            p.collider.CallbackFinish -= OnFinish;
        }
        Invoke("SpawnRandom", 1.0f);
    }
}