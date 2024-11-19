using UnityEngine;
using UnityEngine.UIElements;

public class StoryClueInfo : MonoBehaviour
{
    [SerializeField] private string storyClueName;
    [SerializeField, TextArea(3, 10)] private string storyClueInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string ReturnName()
    {
        return storyClueName;
    }
    public string ReturnTextInfo()
    {
        return storyClueInfo;
    }
}
