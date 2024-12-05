using System.Data.Common;
using FMOD.Studio;
using HauntedPSX.RenderPipelines.PSX.Runtime;
using PSX;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    FMOD.Studio.Bus m_Master, m_BG, m_SFX;
    public Slider m_MasterVolSlider, m_SFXVolSlider, m_BGVolSlider;
    public Toggle m_VHSToggle, m_GlitchToggle, m_NoiseToggle;
    private float m_MasterVolume = 1f;
    private float m_SFXVolume = 1f;
    private float m_BGVolume = 1f;

    Volume m_Volume;
    VolumeProfile m_VolumeProfile;
    GameObject m_GlitchOverlay;

    private void Awake()
    {
        m_MasterVolSlider.value = m_MasterVolume;
        m_SFXVolSlider.value = m_SFXVolume;
        m_BGVolSlider.value = m_BGVolume;

        m_VHSToggle.isOn = true;
        m_GlitchToggle.isOn = true;
        m_NoiseToggle.isOn = true;

        m_Master = FMODUnity.RuntimeManager.GetBus("bus:/");
        m_SFX = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        m_BG = FMODUnity.RuntimeManager.GetBus("bus:/BGM");

        // Find the Volume component in the scene
        m_Volume = FindFirstObjectByType<Volume>();

        if (m_Volume != null && m_Volume.profile != null)
        {
            m_VolumeProfile = m_Volume.profile;
        }

        m_GlitchOverlay = GameObject.FindGameObjectWithTag("Glitch");
    }

    private void Start()
    {
        m_MasterVolSlider.onValueChanged.AddListener(delegate { MasterVolumeLevel(); });
        m_SFXVolSlider.onValueChanged.AddListener(delegate { SFXVolumeLevel(); });
        m_BGVolSlider.onValueChanged.AddListener(delegate { BGVolumeLevel(); });

        m_VHSToggle.onValueChanged.AddListener(delegate { ToggleVHS(); });
        m_GlitchToggle.onValueChanged.AddListener(delegate { ToggleGlitch(); });
        m_NoiseToggle.onValueChanged.AddListener(delegate { ToggleNoise(); });
    }

    public void MasterVolumeLevel()
    {
        m_MasterVolume = m_MasterVolSlider.value;
        m_Master.setVolume(m_MasterVolume);
        Debug.Log("Master Vol Changed to: " + m_MasterVolume);
    }
    public void SFXVolumeLevel()
    {
        m_SFXVolume = m_SFXVolSlider.value;
        m_SFX.setVolume(m_SFXVolume);
        Debug.Log("SFX Vol Changed to: " + m_SFXVolume);
    }
    public void BGVolumeLevel()
    {
        m_BGVolume = m_BGVolSlider.value;
        m_BG.setVolume(m_BGVolume);
        Debug.Log("BG Vol Changed to: " + m_BGVolume);
    }

    public void ToggleVHS()
    {
        CathodeRayTubeVolume crtVolume;
        if (m_VolumeProfile.TryGet(out crtVolume))
        {
            crtVolume.isEnabled.value = m_VHSToggle.isOn;
        }
    }

    public void ToggleGlitch()
    {
        m_GlitchOverlay.SetActive(m_GlitchToggle.isOn);
    }

    public void ToggleNoise()
    {
        QualityOverrideVolume pixelation;
        if (m_VolumeProfile.TryGet(out pixelation))
        {
            pixelation.active = m_NoiseToggle.isOn;
        }
    }
}
