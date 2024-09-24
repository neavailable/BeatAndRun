using System;
using UnityEngine;
using UnityEngine.UI;

public class DoubleAttackButtonClicked : ButtonClicked
{
    public static Action DoubleAttackButtonClicked_;

    [SerializeField] private float cooldownTime;
    private Button button;
    private Image image;
    private bool isCooldown = false, startCooldown = false;

    private void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (isCooldown)
        {
            ButtonCooldown();
        }
    }

    private void OnEnable()
    {
        Player.IsEnemyNear += SetStateOfButton;
        Player.IsntAttacking += SetStateOfButton;
    }

    private void OnDisable()
    {
        Player.IsEnemyNear -= SetStateOfButton;
        Player.IsntAttacking -= SetStateOfButton;
    }

    private void ButtonCooldown()
    {
        if (image.fillAmount >= 1)
        {
            if (startCooldown)
            {
                image.fillAmount = 0;
                startCooldown = false;
            }
            else
            {
                isCooldown = false;
                button.enabled = true;
            }
        }

        image.fillAmount += 1 / cooldownTime * Time.deltaTime;
    }

    private void SetStateOfButton(bool turnOn)
    {
        if (isCooldown) return;

        button.enabled = turnOn;
        image.fillAmount = turnOn ? 1 : 0;
    }

    protected override void ButtonEvent() 
    {
        if (!button.enabled) return;

        DoubleAttackButtonClicked_?.Invoke();
        isCooldown = true; startCooldown = true;
        button.enabled = false;
    }
}
