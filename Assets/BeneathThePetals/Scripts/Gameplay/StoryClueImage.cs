using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;
using System.Collections;

public class StoryClueImage : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isLongTextClue;
    [SerializeField] private string storyclueName;
    [SerializeField] private Sprite clueImage;
    [SerializeField, TextArea] private string monologueText;

    private GameObject storyclueUI;

    private TMP_Text titleText;
    private Image displayImage;
    private TMP_Text monologueTextArea;

    private Image displayImage2;
    private TMP_Text monologueTextArea2;

    private bool isInteracting = false;
    private bool inputLocked = false;

    private PlayerController playerController;
    private FirstPersonController firstPersonController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        storyclueUI = GameObject.FindGameObjectWithTag("StoryClueUI");
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        firstPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();

        if (storyclueUI != null)
        {
            titleText = storyclueUI.transform.Find("TitleText").GetComponent<TMP_Text>();
            displayImage = storyclueUI.transform.Find("ClueImage").GetComponent<Image>();
            monologueTextArea = storyclueUI.transform.Find("MonologueText").GetComponent<TMP_Text>();

            displayImage2 = storyclueUI.transform.Find("ClueImage_TEXT").GetComponent<Image>();
            monologueTextArea2 = storyclueUI.transform.Find("MonologueText_TEXT").GetComponent<TMP_Text>();

            foreach (Transform child in storyclueUI.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (storyclueUI != null)
        {
            if (!inputLocked && isInteracting && storyclueUI.activeSelf && Input.GetKeyDown(KeyCode.E))
            {
                foreach (Transform child in storyclueUI.transform)
                {
                    child.gameObject.SetActive(false);
                }
                isInteracting = false;
                playerController.EnableInput();
                firstPersonController.EnableInput();
            }
        }
    }
    public void Activate()
    {
        //throw new System.NotImplementedException();
    }

    public void Deactivate()
    {
        //throw new System.NotImplementedException();
    }

    public string GetActionName()
    {
        return "Pick Up";
    }

    public string GetName()
    {
        return storyclueName;
    }

    public void Interact()
    {
        firstPersonController.isWalking = false;
        if (storyclueUI != null && !isInteracting)
        {
            UpdateUI();

            int childCount = storyclueUI.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = storyclueUI.transform.GetChild(i);

                if (isLongTextClue && i >= 5)
                    child.gameObject.SetActive(true);
                else if (!isLongTextClue && i < 5)
                    child.gameObject.SetActive(true);
                else
                    child.gameObject.SetActive(false);
            }

            isInteracting = true;
            playerController.DisableInput();
            firstPersonController.DisableInput();

            StartCoroutine(LockInputForDuration(0.2f));
        }
    }

    public void PlayInteractSound()
    {
        //
    }
    private void UpdateUI()
    {
        if (titleText != null)
            titleText.text = storyclueName;
        if (displayImage != null)
            displayImage.sprite = clueImage;
        if (monologueTextArea != null)
            monologueTextArea.text = monologueText;
        if (displayImage2 != null)
            displayImage2.sprite = clueImage;
        if (monologueTextArea2 != null)
            monologueTextArea2.text = monologueText;
    }

    private IEnumerator LockInputForDuration(float duration)
    {
        inputLocked = true;
        yield return new WaitForSeconds(duration);
        inputLocked = false;
    }
    
    public string GetActionType()
    {
        return "Press";
    }
}
