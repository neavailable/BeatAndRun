using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(CanvasRenderer), typeof(Image))]
[RequireComponent(typeof(Button))]

public abstract class ButtonClicked : MonoBehaviour
{
    protected virtual void ButtonEvent() {}
}