using System.Collections;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public Transform leftTransform;
    public LevelController levelController;
    public Transform rightTransform;

    private float currentPosition = 0.0f;
    private Coroutine movingCoroutine = null;
    private Transform thisTransform;

    private void MoveTo(bool left)
    {
        if (movingCoroutine != null)
        {
            StopCoroutine(movingCoroutine);
            movingCoroutine = null;
        }
        movingCoroutine = StartCoroutine(MovingCoroutine(left));
    }

    private IEnumerator MovingCoroutine(bool left)
    {
        if (left)
        {
            float targetPosition = 0.0f;
            for (; targetPosition < currentPosition; currentPosition -= Time.deltaTime / levelController.currentHorisontalTime)
            {
                thisTransform.position = Vector3.Lerp(leftTransform.position,
                                                      rightTransform.position,
                                                      currentPosition);
                yield return null;
            }
            thisTransform.position = leftTransform.position;
            currentPosition = targetPosition;
        }
        else
        {
            float targetPosition = 1.0f;
            for (; currentPosition < targetPosition; currentPosition += Time.deltaTime / levelController.currentHorisontalTime)
            {
                thisTransform.position = Vector3.Lerp(leftTransform.position,
                                                      rightTransform.position,
                                                      currentPosition);
                yield return null;
            }
            thisTransform.position = rightTransform.position;
            currentPosition = targetPosition;
        }
        movingCoroutine = null;
    }

    // Use this for initialization 
    private void Start()
    {
        thisTransform = this.transform;

        thisTransform.position = leftTransform.position;
    }

    // Update is called once per frame 
    private void Update()
    {
        bool left = Input.GetKeyDown("left");
        bool right = Input.GetKeyDown("right");
        //Debug.LogFormat("currentPosition=" + currentPosition);
        if ((left && !right) ||
            (right && !left))
        {
            MoveTo(left);
        }
    }
}