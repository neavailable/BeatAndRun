using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(CanvasRenderer), typeof(Image))]

public class HeathBarFiller : MonoBehaviour
{
    private Image heathBarFillerImage;

    private void Start()
    {
        heathBarFillerImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        Player.ChangeHp += FillHealthBar;
    }

    private void OnDisable()
    {
        Player.ChangeHp -= FillHealthBar;
    }

    private void FillHealthBar(float hp, float maxHp)
    {
        heathBarFillerImage.fillAmount = hp / maxHp;
    }
}
