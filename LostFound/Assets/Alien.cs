using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField]
    [Range(0.02f,0.1f)]
    private float angerPerSecond = 0.1f;
    public float AngerPerSecond => angerPerSecond;

    [SerializeField]
    [Range(0.1f,0.5f)]
    private float angerPerQuestion = 0.175f;
    public float AngerPerQuestion => angerPerQuestion;
}
