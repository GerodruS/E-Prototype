using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelSettings levelSettings;
    public int seed = 0;
    public GameObject barrier;

    [ReadOnly]
    public float currentFallSpeed = 1.0f;

    [ReadOnly]
    public float currentHorisontalTime = 0.0f;

    [ReadOnly]
    public LevelSettings.Start.Position currentSide = LevelSettings.Start.Position.Left;

    [Header("Debug")]
    public bool showArt = true;
    public bool showPlaceholders = false;

    private int barriersInBlockLeft = 0;
    private List<GameObject> barriersPoolBusy = new List<GameObject>();
    private List<GameObject> barriersPoolFree = new List<GameObject>();
    private int barriersPoolStartCount = 1;
    private int currentBlockIndex = 0;
    private Mover lastBarrierMover = null;
    private System.Random random;

    public IEnumerator SpawnBarrier(LevelSettings.BarrierType size,
                                    float deltaPosition,
                                    float sideProbability,
                                    bool waitForEnd)
    {
        Debug.Log("SpawnBarrier start");
        var barrierMover = GetFreeBarrier();
        if (barrierMover)
        {
            barrierMover.Reset();
            barrierMover.Size = size;
            barrierMover.enabled = true;

            if (lastBarrierMover != null)
            {
                var pos = barrierMover.transform.position;
                pos.y = lastBarrierMover.transform.position.y - deltaPosition;
                //Debug.LogFormat("Position: {0} {1} {2}", barrierMover.transform.position, pos, lastBarrierMover.transform.position.y);
                barrierMover.transform.position = pos;
                barrierMover.newStartPosition = pos;
                //Debug.Break();
            }

            {
                double r = random.NextDouble();
                if (r < sideProbability)
                {
                    switch (currentSide)
                    {
                        case LevelSettings.Start.Position.Left:
                            currentSide = LevelSettings.Start.Position.Right;
                            break;

                        case LevelSettings.Start.Position.Right:
                            currentSide = LevelSettings.Start.Position.Left;
                            break;

                        default:
                            Debug.LogError("Mover.Side");
                            break;
                    }
                }
            }

            barrierMover.Side = currentSide;

            while (!barrierMover.spawnNext)
            {
                //Debug.Log("SpawnBarrier " + barrierMover.spawnNext);
                yield return null;
            }
            lastBarrierMover = barrierMover;
            //Debug.Log("SpawnBarrier " + barrierMover.spawnNext);

            if (waitForEnd)
            {
                while (barrierMover.enabled)
                {
                    //Debug.Log("barrierMover enabled = " + barrierMover.enabled);
                    yield return null;
                }
            }
        }
        Debug.Log("SpawnBarrier finish");
    }

    private void AddBarrier()
    {
        var obj = Instantiate(barrier);
        barriersPoolFree.Add(obj);
        //obj.GetComponentInChildren<Mover>().enabled = false;
        obj.GetComponentInChildren<Mover>().CallbackFinish += OnBarrierFinish;
    }

    private IEnumerator ChangeSpeedTo(float targetSpeed)
    {
        float time = 1.0f;
        float startSpeed = currentFallSpeed;
        for (float i = 0.0f; i < time; i += Time.deltaTime)
        {
            currentFallSpeed = Mathf.Lerp(startSpeed, targetSpeed, i);
            yield return null;
        }
        currentFallSpeed = targetSpeed;
    }

    private Mover GetFreeBarrier()
    {
        if (0 == barriersPoolFree.Count)
        {
            AddBarrier();
        }
        var barrierObject = barriersPoolFree[0];
        barriersPoolFree.RemoveAt(0);
        barriersPoolBusy.Add(barrierObject);
        return barrierObject.GetComponentInChildren<Mover>();
    }

    private void OnBarrierFinish(GameObject sender)
    {
        Debug.Log("OnBarrierFinish");
        if (sender != null)
        {
            SetBarrierFree(sender);
        }
    }

    private void SetBarrierFree(GameObject sender)
    {
        //int index = barriersPoolBusy.FindIndex(obj => obj.GetInstanceID() == sender.GetInstanceID());
        int index = barriersPoolBusy.IndexOf(sender);
        Debug.Log("SetBarrierFree " + index);
        if (index != -1)
        {
            barriersPoolBusy.RemoveAt(index);
            barriersPoolFree.Add(sender);
        }
    }

    private void Start()
    {
        random = new System.Random(seed);

        for (int i = 0; i < barriersPoolStartCount; ++i)
        {
            AddBarrier();
        }

        StartCoroutine(StartLevel(levelSettings));
    }

    private IEnumerator StartBlock(LevelSettings levelSettings, int blockIndex)
    {
        lastBarrierMover = null;

        Debug.LogFormat("StartBlock({0})", blockIndex);
        LevelSettings.Block block = levelSettings.blocks[blockIndex];
        LevelSettings.DifficultyLevel difficultyLevel = levelSettings.difficultyLevels[block.difficultyLevel];
        currentHorisontalTime = block.switchDelay;

        yield return StartCoroutine(ChangeSpeedTo(block.startSpeed));

        int count = block.quantity;
        float distanceToNextBarrier = difficultyLevel.gapMultiplier;
        for (int i = 0; i < count; ++i)
        {
            var size = difficultyLevel.sizeProbability.getBarrierSize(random.NextDouble());
            Debug.LogFormat("SpawnBarrier({0})", i);
            var coroutine = SpawnBarrier(size, distanceToNextBarrier, difficultyLevel.sideProbability, i + 1 == count);
            yield return StartCoroutine(coroutine);
            Debug.LogFormat("SpawnBarrier({0}) finish", i);
            //yield return new WaitForSeconds(difficultyLevel.gapMultiplier);
        }
        Debug.LogFormat("StartBlock({0}) finish", blockIndex);
    }

    private IEnumerator StartLevel(LevelSettings levelSettings)
    {
        if (levelSettings.blocks != null)
        {
            int blocksCount = levelSettings.blocks.Length;
            for (int i = 0; i < blocksCount; ++i)
            {
                yield return StartCoroutine(StartBlock(levelSettings, i));
            }
        }
    }
}