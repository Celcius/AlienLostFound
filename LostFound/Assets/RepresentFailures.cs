﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepresentFailures : MonoBehaviour
{

    [SerializeField]
    private Image[] images;

    [SerializeField]
    private IntVar failures;

    [SerializeField]
    private Sprite failSprite;

    [SerializeField]
    private Sprite fakeSprite;

    [SerializeField]
    private GameController controller;

    void Start()
    {
        failures.OnChange += OnFailuresChange;
        OnFailuresChange(0, failures.Value);
    }

    private void OnDestroy() 
    {
        failures.OnChange -= OnFailuresChange;    
    }

    void OnFailuresChange(int oldVal, int newVal)
    {
        int maxIndex = Mathf.Min(newVal, images.Length);
        for(int i = 0; i < images.Length; i++)
        {
            if(i< newVal)
            {
                images[i].sprite = controller.IsFailureFaker[i]? fakeSprite : failSprite;
                images[i].enabled = true;
            }
            else
            {
                images[i].enabled = false;
            }
        }
    }
}
