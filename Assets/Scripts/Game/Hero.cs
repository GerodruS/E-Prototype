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
    private float currentSpeedMultiplier = 0.0f;
    private bool currentDirectionLeft = false;

    private void MoveTo(bool left)
    {
        if (movingCoroutine != null)
        {
            StopCoroutine(movingCoroutine);
            movingCoroutine = null;
            if (currentDirectionLeft != left)
            {
                currentSpeedMultiplier *= levelController.levelSettings.heroMoving.switchSpeedMultiplier;
            }
        }
        else
        {
            currentSpeedMultiplier = 1.0f;
        }
        movingCoroutine = StartCoroutine(MovingCoroutine(left));
    }

    private IEnumerator MovingCoroutine(bool left)
    {
        currentDirectionLeft = left;
        if (left)
        {
            float targetPosition = 0.0f;
            for (; targetPosition < currentPosition; currentPosition -= Time.deltaTime * currentSpeedMultiplier / levelController.currentHorisontalTime)
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
            for (; currentPosition < targetPosition; currentPosition += Time.deltaTime * currentSpeedMultiplier / levelController.currentHorisontalTime)
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

    private void Start()
    {
        thisTransform = this.transform;

        thisTransform.position = leftTransform.position;
    }

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