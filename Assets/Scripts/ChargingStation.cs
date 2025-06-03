using UnityEngine;
using System;

public class ChargingStation : MonoBehaviour
{
    [SerializeField]
    private BatterySO batterySO;
    public static event Action<bool, GameObject> OnPlayerInChargingStation;
    
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             Debug.Log("CollisionChargeStation");
             OnPlayerInChargingStation?.Invoke(true, other.gameObject);
            UIManager.Instance.ShowChargeStationUI();
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        OnPlayerInChargingStation?.Invoke(false, other.gameObject);
        UIManager.Instance.HideChargeStationUI();
    }
}
