using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class CultistOutsideRun : MonoBehaviour
{
    [Header("Chasing Settings")]
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private bool isRunning = true;

    [Header("Leader Cultist")]
    [SerializeField] private bool isLeader = false;
    [SerializeField] private float distanceBetween = 10f;
    [SerializeField] private Volume globalVolumeBase;

    private GameObject player;
    private PauseMenu pauseMenu;
    private PlayerController playerController;
    private NavMeshAgent navMeshAgent;
    public EventReference killSounds;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        pauseMenu = FindAnyObjectByType<PauseMenu>();
        if (isRunning) SetupNavMeshAgent();

        RandomizeAnimation();
    }

    private void SetupNavMeshAgent()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = runSpeed;
        navMeshAgent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        if (pauseMenu.isPaused || playerController.DialogueBox.activeSelf)
            return;

        if (isLeader)
        {
            float sqrDistance = (transform.position - player.transform.position).sqrMagnitude;
            float sqrThreshold = distanceBetween * distanceBetween;
            float targetWeight = (sqrDistance <= sqrThreshold) ? 0.5f : 1f;

            globalVolumeBase.weight = Mathf.MoveTowards(globalVolumeBase.weight, targetWeight, 0.1f * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (pauseMenu.isPaused || playerController.DialogueBox.activeSelf)
            return;

        if (isRunning)
            navMeshAgent.SetDestination(player.transform.position);
    }
    
    private void RandomizeAnimation()
    {
        Animation anim = GetComponentInChildren<Animation>();

        if (anim != null && anim.clip != null)
        {
            anim[anim.clip.name].time = Random.Range(0f, anim.clip.length);

            anim.Sample();
            anim.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<FirstPersonController>().DisableInput();
            StartCoroutine(StartGameOver());
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    private IEnumerator StartGameOver()
    {
        playerController.GetComponentInChildren<PauseMenu>().StartGameOver();
        yield return new WaitForSeconds(4f);
        playKillSound();
        yield return new WaitForSeconds(2f);

        yield return new WaitForSeconds(7f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private void playKillSound()
    {
        EventInstance  soundOnKill = RuntimeManager.CreateInstance(killSounds);
        RuntimeManager.AttachInstanceToGameObject(soundOnKill, transform);
        soundOnKill.start();
        soundOnKill.release();
    }
}
