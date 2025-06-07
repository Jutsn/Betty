using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField]
    private GameObject PressFPanel;
    [SerializeField]
    private GameObject PressQPanel;
    public TMPro.TextMeshProUGUI batteryPercentageText;
    public float batteryPercentage;

	public GameObject endPanel;
	public GameObject endPanel2;

	bool showWarning;

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


    private void Update()
    {
        batteryPercentage = (batterySO.energy / batterySO.maxEnergy) * 100;
        Debug.Log(batterySO.energy);
    }

    // Update is called once per frame
    public void UpdateBatteryChargeUI()
    {
        batteryChargeSlider.maxValue = batterySO.maxEnergy;
        batteryChargeSlider.value = batterySO.energy;
        batteryPercentage = (batterySO.energy / batterySO.maxEnergy) * 100;
        ChangeBatteryColor();
        batteryPercentageText.text = $"{(batterySO.energy / batterySO.maxEnergy * 100).ToString("F0")}%";
    }

    private void ChangeBatteryColor()
    {
        if (!showWarning)
        {
            if (batteryPercentage > 50f)
            {
                batteryChargeSlider.fillRect.GetComponent<Image>().color = Color.green;
            }
            else if (batteryPercentage > 20f)
            {
                batteryChargeSlider.fillRect.GetComponent<Image>().color = Color.yellow;
            }
            else if (batteryPercentage <= 20f)
            {
                batteryChargeSlider.fillRect.GetComponent<Image>().color = Color.red;
            }
        }
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

    public void ShowPressFPanelUI()
    {
        PressFPanel.SetActive(true);
    }

    public void HidePressFPanelUI()
    {
        PressFPanel.SetActive(false);
    }
    public void ShowPressQPanelUI()
    {
        PressQPanel.SetActive(true);
    }

    public void HidePressQPanelUI()
    {
        PressQPanel.SetActive(false);
    }

    public void ChangeBatteryColorToRed()
    {
        showWarning = true;
        batteryChargeSlider.fillRect.GetComponent<Image>().color = Color.red;
        StartCoroutine(ResetBatteryColorAfterWarning());
    }
    IEnumerator ResetBatteryColorAfterWarning()
    {
        yield return new WaitForSeconds(0.5f);
        showWarning = false;
        ChangeBatteryColor();
    }

    public void ShowEndPanels()
    {
		StartCoroutine(ActivateEndPanel());
	}

	IEnumerator ActivateEndPanel()
	{
		endPanel.SetActive(true);
		yield return new WaitForSeconds(2.5f);
		endPanel.SetActive(false);
		yield return new WaitForSeconds(0.5f);
		endPanel2.SetActive(true);
		yield return new WaitForSeconds(2.5f);
		SceneManager.LoadScene("MainMenu");
	}
}
