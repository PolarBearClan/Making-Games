using UnityEngine;

public class ScreenNoteManager : MonoBehaviour
{
    public delegate void NoteEnd();
    public NoteEnd NoteEndCallback;
    
    [SerializeField] private TMPro.TMP_Text mainText;
    
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
        mainText.text = storyText;
    }

    public void CloseNote()
    {
        NoteEndCallback();
        gameObject.SetActive(false);
    }
}
