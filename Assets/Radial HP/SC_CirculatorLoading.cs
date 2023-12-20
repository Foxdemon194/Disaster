using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_CirculatorLoading : MonoBehaviour
{
    public Image loadingImage;
    public Text loadingText;
    [Range(0, 1)]
    public float loadingProgress = 0;

    public GameObject winScreen;
    public GameObject loseScreen;

    private void Start()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    private void Update()
    {
        loadingImage.fillAmount = loadingProgress;
        if(loadingProgress < 1 && loadingProgress != 0)
        {
            loadingText.text = Mathf.RoundToInt(loadingProgress * 100) + "%\nLoading...";
        }
        else if(loadingProgress == 1)
        {
            winScreen.SetActive(true);
            loadingText.text = "Done.";
        }
        else if(loadingProgress == 0)
        {
            loadingText.text = "Failure.";
            loseScreen.SetActive(true);
        }
    }
}
