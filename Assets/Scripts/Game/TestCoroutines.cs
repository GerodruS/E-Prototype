using UnityEngine;

public class TestCoroutines : MonoBehaviour
{
    /*
    // Use this for initialization 
    private void Start()
    {
        enabled = false;
        //StartCoroutine(NewCoroutine());
        StartCoroutine(StartExample());
    }

    // Update is called once per frame 
    private IEnumerator NewCoroutine()
    {
        int count = 5;
        for (int i = 0; i < count; ++i)
        {
            Debug.LogFormat("NewCoroutine : {0} [{1}]", i, Time.time);
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator WaitAndPrint()
    {
        // suspend execution for 5 seconds 
        yield return new WaitForSeconds(5);
        print("WaitAndPrint " + Time.time);
    }

    private IEnumerator StartExample()
    {
        print("Starting " + Time.time);

        // Start function WaitAndPrint as a coroutine 
        yield return StartCoroutine("WaitAndPrint");
        print("Done " + Time.time);
    }
    */
}