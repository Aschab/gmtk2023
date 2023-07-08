using System.Diagnostics.Tracing;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    private FloatVariable volume;
    private BoolVariable muted;

    [SerializeField] Slider volumeSlider;
    [SerializeField] Button mutedToggle;
    [SerializeField] Image mutedImage;
    [SerializeField] Sprite isMutedSprite;
    [SerializeField] Sprite isNotMutedSprite;

    void Start()
    {
        UpdateMutedToggle();
        UpdateSlider();

        mutedToggle.onClick.AddListener(() => muted.Value = !muted.Value);
        volumeSlider.onValueChanged.AddListener((float newVolume) => volume.Value = newVolume);
    }

    void OnEnable()
    {
        volume = Resources.Load<FloatVariable>("Variables/Settings/volume");
        muted = Resources.Load<BoolVariable>("Variables/Settings/muted");

        volume.AddListener(UpdateSlider);
        muted.AddListener(UpdateMutedToggle);
    }

    void OnDisable()
    {
        volume.RemoveListener(UpdateSlider);
        muted.RemoveListener(UpdateMutedToggle);
    }

    private void UpdateSlider()
    {
        volumeSlider.value = volume.Value;
        if (volume.Value <= 0) UpdateMutedToggle();
    }
    private void UpdateMutedToggle()
    {
        if (volume.Value <= 0 || muted.Value)
        {
            mutedImage.sprite = isMutedSprite;
        }
        else
        {
            mutedImage.sprite = isNotMutedSprite;
        }
        volumeSlider.interactable = !muted.Value;
    }


}
