using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmoaebaUtils;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Alien : MonoBehaviour
{
    public enum AlienAnimations
    {
        FadeInToBackground,
        MoveToFront,
        Talk,
        Idle,
        LeaveAngry,
        LeaveHappy,
        LeaveHurt,
    }

    [SerializeField]
    private Sprite normalFace;

    [SerializeField]
    private Sprite happyFace;

    [SerializeField]
    private Sprite angryFace;

    [SerializeField]
    private Sprite talkFace;

    
    [SerializeField]
    private Sprite shockFace;
    
    [SerializeField]
    private SpriteRenderer face;

    [SerializeField]
    [Range(0.02f,0.1f)]
    private float angerPerSecond = 0.1f;
    public float AngerPerSecond => angerPerSecond;

    [SerializeField]
    [Range(0.1f,0.5f)]

    private float angerPerQuestion = 0.175f;
    public float AngerPerQuestion => angerPerQuestion;

    public Action currentAnimationCallback;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private SoundSystem soundSystem;

    [SerializeField]
    private AudioClip[] talkClips;

    [SerializeField]
    private AudioClip[] hurtClips;

    [SerializeField]
    private AudioClip[] angryClips;

    [SerializeField]
    private AudioClip[] happyClips;

    [SerializeField]
    private SpriteRenderer body;

    [SerializeField]
    private bool jumbleColor = true;
    public bool JumbleColor => jumbleColor;

    [SerializeField]
    private bool jumbleShape = true;
    public bool JumbleShape => jumbleShape;

    [SerializeField]
    private string[] textFormats = new string[] 
    {
        "I think it be {0} and {1}", 
        "It's like totally {0} and like {1}",
        "... {0} ... {1}...",
        "Gimme {0} ... maybe {1}"
    };

    [SerializeField]
    private string[] happyTexts = new string[0];

    [SerializeField]
    private string[] angryTexts = new string[0];

    [SerializeField]
    private string[] painTexts = new string[0];

    [SerializeField]
    private string[] fakerTexts = new string[0];


    private void Start() 
    {
        body.sortingOrder = 0;
        face.sortingOrder = 1;
    }

    public void Animate(AlienAnimations animationType, Action endCallback = null)
    {
        this.currentAnimationCallback = endCallback;
        switch(animationType)
        {
            case AlienAnimations.Idle:
                currentAnimationCallback =  null;
                face.sprite = normalFace;
                break;

            case AlienAnimations.Talk:
                if(anim.GetBool("IsTalking"))
                {   
                    anim.playbackTime = 0;
                }
                else
                {
                    anim.SetTrigger("IsTalking");
                }
                face.sprite = talkFace;
                break;
            case AlienAnimations.MoveToFront:

                body.sortingOrder = 2;
                face.sortingOrder = 3;
                anim.SetTrigger("MoveForward");
                face.sprite = normalFace;
                break;
            case AlienAnimations.LeaveAngry:
                face.sprite = angryFace;
                anim.SetTrigger("LeaveAngry");
                break;
            case AlienAnimations.LeaveHappy:
            face.sprite = happyFace;
                anim.SetTrigger("LeaveHappy");
                break;
            case AlienAnimations.LeaveHurt:
                face.sprite = shockFace;
                anim.SetTrigger("LeaveHurt");
                break;
        }
    }

    public void OnMoveEnd()
    {
        body.sortingOrder = 4;
        face.sortingOrder = 5;
        currentAnimationCallback?.Invoke();
    }

    public void OnLeaveEnd()
    {
        currentAnimationCallback?.Invoke();
        OnLeft();
    }
                
    public void OnDefaultEnd()
    {
        currentAnimationCallback?.Invoke();
    }

    private void PlayTalkSpeech()
    {
        PlaySpeechSound(talkClips);
    }

    private void PlayAngrySpeech()
    {
        PlaySpeechSound(angryClips);
    }

    private void PlayHappySpeech()
    {
        PlaySpeechSound(happyClips);
    }

    private void PlayElectricNoise()
    {
        PlaySpeechSound(hurtClips);
    }

    private void PlaySpeechSound(AudioClip[] clips)
    {
        soundSystem.StopSound("Speech");
        if(clips == null || clips.Length == 0)
        {
            return;
        }
        
        AudioClip clip = clips[UnityEngine.Random.Range(0, clips.Length)];
        soundSystem.PlaySound(clip, "Speech", false);
    }


    private void OnLeft()
    {
        Destroy(this.gameObject);
    }

    public string GetTextFormat()
    {
        if(textFormats.Length == 0)
        {
            return "UNINTELLIGEABLE {0} UNINTELLIGEABLE {1}!?";
        }

        return GetRandomText(textFormats);
        
    }

    private string GetRandomText(string[] options)
    {
        if(options == null || options.Length == 0)
        {
            return "";
        }

        int index = UnityEngine.Random.Range(0, options.Length);
        return options[index];
    }

    public string GetHappyText()
    {
        return GetRandomText(happyTexts);
    }

    public string GetAngryText()
    {
        return GetRandomText(angryTexts);
    }

    public string GetPainText()
    {
        return GetRandomText(painTexts);
    }

    public string GetFakerText()
    {
        return GetRandomText(fakerTexts);
    }
}
