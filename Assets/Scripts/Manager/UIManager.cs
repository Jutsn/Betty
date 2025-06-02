using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private Slider batteryChargeSlider;
    [SerializeField]
    private BatterySO batterySO;


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
}
