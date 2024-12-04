using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    FMOD.Studio.Bus m_Master, m_BGM, m_SFX;
    public Slider m_MasterVolSlider, m_SFXVolSlider, m_BGMVolSlider;
    private float m_MasterVolume = 1f;
    private float m_SFXVolume = 1f;
    private float m_BGMVolume = 1f;

    Bank[] bank;
    Bus[] buses;


    private void Awake()
    {
        Debug.Log(FMODUnity.RuntimeManager.StudioSystem.getBankList(out bank));

        bank[0].getBusList(out buses);
        foreach (var bus in buses)
        {
            Debug.Log(bus);
        }

        m_Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        m_SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        m_BGM = FMODUnity.RuntimeManager.GetBus("bus:/Master/BGM");
    }

    private void Start()
    {
        m_MasterVolSlider.onValueChanged.AddListener(delegate { MasterVolumeLevel(); });
        m_SFXVolSlider.onValueChanged.AddListener(delegate { SFXVolumeLevel(); });
        m_BGMVolSlider.onValueChanged.AddListener(delegate { BGMVolumeLevel(); });
    }

    public void MasterVolumeLevel()
    {
        m_MasterVolume = m_MasterVolSlider.value;
        Debug.Log("Master Vol Changed to: " + m_MasterVolume);
    }
    public void SFXVolumeLevel()
    {
        m_SFXVolume = m_SFXVolSlider.value;
    }
    public void BGMVolumeLevel()
    {
        m_BGMVolume = m_BGMVolSlider.value;
    }
}
