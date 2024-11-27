using DG.Tweening;
using UnityEngine;

public class ScreenNoteManager : MonoBehaviour
{
    public delegate void NoteEnd();
    public NoteEnd NoteEndCallback;
    
    [SerializeField] private TMPro.TMP_Text mainText;
    [SerializeField] private GameObject continueButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNote(string storyText)
    {
        gameObject.SetActive(true);
        continueButton.SetActive(true);
        mainText.text = storyText;
    }

    public void CloseNote()
    {
        NoteEndCallback();
        gameObject.SetActive(false);
    }

    public void ShowNoteNotification(string notificationText, int duration)
    {
        continueButton.SetActive(false);
        gameObject.SetActive(true);

        mainText.text = notificationText;
        
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.15f);
        
        Invoke(nameof(DisableNote), duration);
    }

    private void DisableNote()
    {
        gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.35f).OnComplete(() =>
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1;
            gameObject.SetActive(false);
        });
    }
}
