        using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButton : MonoBehaviour
{
    public enum ButtonType
    {
        Green,
        Red,
        Question
    }

    [SerializeField]
    private Sprite normalButton;

    [SerializeField]
    private Sprite pressButton;

    [SerializeField]
    private ButtonType buttonType;

    [SerializeField]
    private GameController controller;

    private SpriteRenderer rend;
    private void Start() 
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = normalButton;
    }

    private void OnMouseDown() 
    {
        rend.sprite = pressButton;

        switch(buttonType)
        {
            case ButtonType.Green:
                controller.DeliverItem();
                break;
            
            case ButtonType.Red:
                controller.RejectCustomer();
                break;

            case ButtonType.Question:
                controller.QuestionButton();
                break;
        }
    }

    private void OnMouseUp() 
    {
        rend.sprite = normalButton;          
    }
}
