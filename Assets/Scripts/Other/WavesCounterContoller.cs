using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(CanvasRenderer), typeof(Text))]

public class WavesCounterContoller : MonoBehaviour
{
    private const int wavesNumber = 4;

    [SerializeField] private Text waveText;

    private void OnEnable()
    {
        SceneManager.NewWaveStarted += UpdateText;
    }

    private void OnDisable()
    {
        SceneManager.NewWaveStarted -= UpdateText;
    }

    public void UpdateText(int currentWaves) 
    {
        waveText.text = "Wave: " + currentWaves + "/" + wavesNumber;
    }
}
