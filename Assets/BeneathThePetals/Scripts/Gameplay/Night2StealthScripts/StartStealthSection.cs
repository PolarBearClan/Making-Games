using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;


public class StartStealthSection : MonoBehaviour
{
    [SerializeField]
    private Transform _lookAtObject;

    [SerializeField]
    private float _duration;

    [SerializeField]
    private float _tweenDuration = .1f;

    private GameObject _player;
    private FirstPersonController _playerController;
    public GameObject deactivatePart1Leader;
    public GameObject activatePart2Leader;
    public GameObject destroyKillBox;
    public GameObject destroyKillBox2;
    public KillDoorController killDoorController;

    private bool _triggered = false;
    private float _timeElapsed = 0;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (_triggered)
        {
            if (_timeElapsed == 0)
            {
                    _playerController.playerCamera.transform.DOLookAt(_lookAtObject.transform.position, _tweenDuration);
                    _playerController.transform.DOLookAt(_lookAtObject.transform.position, _tweenDuration);
            }

            if (_timeElapsed >= _tweenDuration)
            {
                    _playerController.playerCamera.transform.DOLookAt(_lookAtObject.transform.position, 0);
            }

            if (_timeElapsed > _duration)
            {
                    _playerController.EnableInput();
                Destroy(gameObject);
            }
            _timeElapsed += Time.deltaTime;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject == _player) && !_triggered)
        {
            killDoorController.magicNr = -9999;
            deactivatePart1Leader.gameObject.SetActive(false);
            activatePart2Leader.gameObject.SetActive(true);
            destroyKillBox.gameObject.SetActive(false);
            destroyKillBox2.gameObject.SetActive(false);
            _triggered = true;
            _playerController.isWalking = false;
            _playerController.DisableInput();

            Debug.Log("Jumpscare!");

        }
    }
}
