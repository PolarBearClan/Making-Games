using UnityEngine;
using Febucci.UI.Core;
using System.Collections.Generic;
using TMPro;
using Febucci.UI;
using UnityEngine.UI;
using System.Collections;
using FMODUnity;
using FMOD;
using FMOD.Studio;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TypewriterCore))]
public class LetterText : MonoBehaviour
{
    TypewriterCore typewriter;
    TextAnimator_TMP textanim;

    [Header("Typewriter")]
    [SerializeField] private GameObject acceptInvitation;
    [SerializeField] private KeyCode inputKey = KeyCode.E;


    [Header("Scene Objects")]
    [SerializeField] private Animator transition;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private PlayableAsset bus;

    public EventReference letterAcceptSound;
    public EventReference busSoundvroomvroom;
    public GameObject beginningMusic;
    public GameObject beginningMusic2;

    private void Start()
    {
        typewriter = GetComponent<TypewriterCore>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipText();
        }
    }

    void OnEnable()
    {
        
    }

    public void ShowAccept()
    {
        StartCoroutine(FadeTextIn(acceptInvitation));
        StartCoroutine(StartGame());
    }

    public void SkipText()
    {
        StopAllCoroutines();
        typewriter.SkipTypewriter();

        StartCoroutine(FadeTextIn(acceptInvitation));
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(inputKey));
        PlayLetterSound();
        transition.transform.gameObject.SetActive(true);
        transition.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        playableDirector.Stop();
        playableDirector.playableAsset = null;
        
        playableDirector.Play(bus);
        PlayBusSound();
        Destroy(beginningMusic);
        Destroy(beginningMusic2);
        yield return new WaitForSeconds((float)bus.duration);
        FinishPrologue();
    }

    IEnumerator FadeTextIn(GameObject obj)
    {
        CanvasGroup alpha = obj.GetComponent<CanvasGroup>();
        float step = Time.deltaTime / 1f;
        while (alpha.alpha < 1f)
        {
            alpha.alpha += step;
            yield return null;
        }
    }


    public void FinishPrologue()
    {
        SceneManager.LoadScene("Day0_Outside");
    }
    
    
    public void PlayLetterSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(letterAcceptSound);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
    
    public void PlayBusSound()
    {
        EventInstance  soundOnInteract = RuntimeManager.CreateInstance(busSoundvroomvroom);
        RuntimeManager.AttachInstanceToGameObject(soundOnInteract, transform);
        soundOnInteract.start();
        soundOnInteract.release();
    }
    
}
