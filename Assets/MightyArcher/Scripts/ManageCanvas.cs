using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCanvas : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    private void OnEnable()
    {
        TurnOffCanvas();
        LoadingFadeEffect.Instance.fadeOutCompleted += TurnOnCanvas;
    }

    private void OnDestroy()
    {
        //Debug.LogError("Destroy!!!! Canvas");
        LoadingFadeEffect.Instance.fadeOutCompleted -= TurnOnCanvas;
    }


    public void TurnOnCanvas()
    {
        canvas.SetActive(true);
    }

    public void TurnOffCanvas()
    {
        canvas.SetActive(false);
    }

}

