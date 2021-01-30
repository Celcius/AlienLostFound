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

    private SpriteRenderer[] renderers;

    private void Start() {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer renderer in renderers)
        {
            renderer.sortingOrder = 0;
        }
    }

    public void Animate(AlienAnimations animationType, Action endCallback = null)
    {
        if(renderers == null)
        {
            renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        this.currentAnimationCallback = endCallback;
        switch(animationType)
        {
            case AlienAnimations.Idle:
                currentAnimationCallback =  null;
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
                break;
            case AlienAnimations.MoveToFront:
                foreach(SpriteRenderer renderer in renderers)
                {
                    renderer.sortingOrder = 1;
                }
                anim.SetTrigger("MoveForward");
                break;
            case AlienAnimations.LeaveAngry:
                anim.SetTrigger("LeaveAngry");
                break;
            case AlienAnimations.LeaveHappy:
                anim.SetTrigger("LeaveHappy");
                break;
            case AlienAnimations.LeaveHurt:
                anim.SetTrigger("LeaveHurt");
                break;
        }
    }

    public void OnMoveEnd()
    {
        foreach(SpriteRenderer renderer in renderers)
        {
            renderer.sortingOrder = 2;
        }
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
}
