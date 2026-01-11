using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GoUI : MonoBehaviour
{
    [Title("References")]
    private CameraManager _cameraManager;

    [Title("Go! UI")]
    [SerializeField] private TextMeshProUGUI _goText;
    [SerializeField] private Image _arrow;

    void Start()
    {
        // Setting references up
        _cameraManager = FindAnyObjectByType<CameraManager>();

        _goText = GetComponentInChildren<TextMeshProUGUI>();
        _arrow = GetComponentInChildren<Image>();

        // Visual
        ToggleUI(false);
    }

    public void ToggleUI(bool value)
    {
        _goText.gameObject.SetActive(value);
        _arrow.gameObject.SetActive(value);
    }

    public void SetDirection()
    {
        if(_cameraManager.NextDirection == "up")
        {
            _arrow.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if(_cameraManager.NextDirection == "right")
        {
            _arrow.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (_cameraManager.NextDirection == "left")
        {
            _arrow.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (_cameraManager.NextDirection == "down")
        {
            _arrow.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }

        ToggleUI(true);
    }
}