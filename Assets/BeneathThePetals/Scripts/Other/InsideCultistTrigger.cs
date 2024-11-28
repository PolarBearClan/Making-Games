using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class InsideCultistTrigger : MonoBehaviour
{
    [SerializeField] private CultistInsideRun cultist;
    [SerializeField] private EventReference soundToPlayOnPickUp;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cultist != null)
            cultist.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (cultist != null)
            {
                cultist.gameObject.SetActive(true);
                cultist.canWalk = true;
            }
            PlayInteractSound();
            transform.gameObject.SetActive(false);
        }
    }

    private void PlayInteractSound()
    {
        if (!soundToPlayOnPickUp.IsNull)
        {
            EventInstance soundWhenSceneChange = RuntimeManager.CreateInstance(soundToPlayOnPickUp);
            RuntimeManager.AttachInstanceToGameObject(soundWhenSceneChange, transform);
            soundWhenSceneChange.start();
            soundWhenSceneChange.release();
        }
    }
}
