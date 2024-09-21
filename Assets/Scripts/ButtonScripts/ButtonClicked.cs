using UnityEngine;

public abstract class ButtonClicked : MonoBehaviour
{
    [SerializeField] protected Player Player;

    protected virtual void ButtonEvent() {}
}