using UnityEngine;

public class Mover : MonoBehaviour
{
    public System.Action<GameObject> CallbackFinish;

    public LevelController levelController;
    public float speedMultiplier = 1.0f;
    public Transform startTransform;
    public Transform finishTransform;
    public bool IsLoop = true;

    [Header("Barriers")]
    public GameObject antennaBarrier;
    public GameObject ledgeBarrier;
    public GameObject flagpoleBarrier;
    public GameObject balconyBarrier;
    public GameObject flagpoleWithRopeBarrier;

    [HideInInspector]
    public Vector3 newStartPosition = Vector3.zero;

    [HideInInspector]
    public bool spawnNext = false;

    private float currentTime = 0.0f;

    private Transform thisTransform;

    public LevelSettings.Start.Position Side
    {
        get
        {
            if (0.0f < thisTransform.localScale.x)
            {
                return LevelSettings.Start.Position.Left;
            }
            else
            {
                return LevelSettings.Start.Position.Right;
            }
        }

        set
        {
            switch (value)
            {
                case LevelSettings.Start.Position.Left:
                    {
                        var scale = thisTransform.localScale;
                        scale.x = Mathf.Abs(scale.x);
                        thisTransform.localScale = scale;
                    }
                    break;

                case LevelSettings.Start.Position.Right:
                    {
                        var scale = thisTransform.localScale;
                        scale.x = -1 * Mathf.Abs(scale.x);
                        thisTransform.localScale = scale;
                    }
                    break;

                default:
                    Debug.LogError("Mover.Side");
                    break;
            }
        }
    }

    public LevelSettings.BarrierType Size
    {
        get
        {
            if (antennaBarrier.activeSelf)
            {
                return LevelSettings.BarrierType.Antenna;
            }
            else if (ledgeBarrier.activeSelf)
            {
                return LevelSettings.BarrierType.Ledge;
            }
            else if (flagpoleBarrier.activeSelf)
            {
                return LevelSettings.BarrierType.Flagpole;
            }
            else if (balconyBarrier.activeSelf)
            {
                return LevelSettings.BarrierType.Balcony;
            }
            else if (flagpoleWithRopeBarrier.activeSelf)
            {
                return LevelSettings.BarrierType.FlagpoleWithRope;
            }
            return LevelSettings.BarrierType.None;
        }

        set
        {
            antennaBarrier.SetActive(false);
            ledgeBarrier.SetActive(false);
            flagpoleBarrier.SetActive(false);
            balconyBarrier.SetActive(false);
            flagpoleWithRopeBarrier.SetActive(false);

            switch (value)
            {
                case LevelSettings.BarrierType.Antenna:
                    antennaBarrier.SetActive(true);
                    break;

                case LevelSettings.BarrierType.Ledge:
                    ledgeBarrier.SetActive(true);
                    break;

                case LevelSettings.BarrierType.Flagpole:
                    flagpoleBarrier.SetActive(true);
                    break;

                case LevelSettings.BarrierType.Balcony:
                    balconyBarrier.SetActive(true);
                    break;

                case LevelSettings.BarrierType.FlagpoleWithRope:
                    flagpoleWithRopeBarrier.SetActive(true);
                    break;

                default:
                    Debug.LogError("Mover.Size");
                    break;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // collider on another object (child) 
        if (other.CompareTag("Colliders/Spawn Next"))
        {
            Debug.Log("Spawn Next Collider");
            //Debug.Break();
            spawnNext = true;
        }
    }

    public void Reset()
    {
        thisTransform = transform;
        thisTransform.position = startTransform.position;
        currentTime = 0.0f;
        spawnNext = false;
    }

    private void Start()
    {
        thisTransform = transform;
    }

    private void Update()
    {
        Vector3 startPosition = newStartPosition != Vector3.zero ?
                                newStartPosition :
                                startTransform.position;
        currentTime += Time.deltaTime;
        float allTime = Vector3.Distance(startPosition, finishTransform.position) / (speedMultiplier * levelController.currentFallSpeed);

        thisTransform.position = Vector3.Lerp(startPosition,
                                              finishTransform.position,
                                              currentTime / allTime);
        if (allTime <= currentTime)
        {
            if (IsLoop)
            {
                currentTime = 0.0f;
            }
            else
            {
                Debug.Log("allTime <= currentTime");
                if (CallbackFinish != null)
                {
                    CallbackFinish.Invoke(thisTransform.parent.gameObject);
                }
                enabled = false;
            }
        }
    }
}