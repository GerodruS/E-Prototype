using System.Collections;
using UnityEngine;

public class BarrierDebug : MonoBehaviour
{
    public LevelController levelController;
    public Renderer art;
    public Renderer placeHolder;

    // Update is called once per frame 
    private void Update()
    {
        if (levelController != null)
        {
            if (art != null)
            {
                art.enabled = levelController.showArt;
            }
            if (placeHolder != null)
            {
                placeHolder.enabled = levelController.showPlaceholders;
            }
        }
    }
}