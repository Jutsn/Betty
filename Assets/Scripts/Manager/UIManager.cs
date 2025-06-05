using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private Slider batteryChargeSlider;
    [SerializeField]
    private BatterySO batterySO;

    [SerializeField]
    private GameObject chargeStationPanel;
    [SerializeField]
    private GameObject consolePanel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateBatteryChargeUI();
    }

    // Update is called once per frame
    public void UpdateBatteryChargeUI()
    {
        batteryChargeSlider.maxValue = batterySO.maxEnergy;
        batteryChargeSlider.value = batterySO.energy;
    }

    public void ShowChargeStationUI()
    {
        chargeStationPanel.SetActive(true);
    }

    public void HideChargeStationUI()
    {
        chargeStationPanel.SetActive(false);
    }

    public void ShowConsoleUI()
    {
		consolePanel.SetActive(true);
    }

    public void HideConsoleUI()
    {
		consolePanel.SetActive(false);
    }
}
