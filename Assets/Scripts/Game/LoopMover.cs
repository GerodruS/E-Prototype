using UnityEngine;

public class LoopMover : MonoBehaviour
{
    public Transform finishTransform;
    public LevelController levelController;

    public Transform startTransform;
    private float currentTime = 0.0f;
    private Transform thisTransform;

    private void Start()
    {
        thisTransform = this.transform;
        thisTransform.position = startTransform.position;
        currentTime = 0.0f;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        float allTime = Vector3.Distance(startTransform.position, finishTransform.position) / levelController.currentFallSpeed;

        thisTransform.position = Vector3.Lerp(startTransform.position,
                                          finishTransform.position,
                                          currentTime / allTime);
        if (allTime <= currentTime)
        {
            currentTime = 0.0f;
        }
    }
}