using System;

public class LiteAttackButtonClicked : ButtonClicked
{
    public static Action LiteAttackButtonClicked_;

    public void ButtonEvent()
    {
        LiteAttackButtonClicked_?.Invoke();
    }
}