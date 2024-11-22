using DG.Tweening;
using UnityEngine;

public class JumpscareInteract : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Jumpscare _jumpscareObject;

    [SerializeField]
    private float _duration;

    [SerializeField]
    private float _tweenDuration = .1f;

    [SerializeField] private string actionName;

    private GameObject _player;
    private FirstPersonController _playerController;
    private Jumpscare _jumpscare;

    private bool _triggered = false;
    private float _timeElapsed = 0;

    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<FirstPersonController>();
        _jumpscare = _jumpscareObject.GetComponent<Jumpscare>();
    }

    void Update()
    {
        if (_triggered)
        {
            if (_timeElapsed == 0)
            {
                if (_jumpscare.GetType() != typeof(JumpscareSpawn))
                {
                    _playerController.playerCamera.transform.DOLookAt(_jumpscareObject.transform.position, _tweenDuration);
                    _playerController.transform.DOLookAt(_jumpscareObject.transform.position, _tweenDuration);
                }
            }

            if (_timeElapsed >= _tweenDuration)
            {
                if (_jumpscare.GetType() != typeof(JumpscareSpawn))
                    _playerController.playerCamera.transform.DOLookAt(_jumpscareObject.transform.position, 0);
            }

            if (_timeElapsed > _duration)
            {
                if (_jumpscare.GetType() != typeof(JumpscareSpawn))
                    _playerController.EnableInput();
                Destroy(gameObject);
            }
            _timeElapsed += Time.deltaTime;
        }
    }

    private void TriggerJumpscare()
    {
        if (!_triggered)
        {
            _triggered = true;

            _playerController.DisableInput();

            _jumpscare.Scare();

            Debug.Log("Jumpscare!");

        }
    }

    public void Interact()
    {
        TriggerJumpscare();
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
        return actionName;
    }

    public string GetName()
    {
        return " ";
    }

    public void PlayInteractSound()
    {
        throw new System.NotImplementedException();
    }
}
