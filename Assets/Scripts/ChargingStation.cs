using UnityEngine;
using System;

public class ChargingStation : MonoBehaviour
{
    [SerializeField] private BatterySO batterySO;
    [SerializeField] private Transform spawnPoint;
    public static event Action<bool, GameObject, ChargingStation> OnPlayerInChargingStation;


    private void Awake() {
        spawnPoint = GetComponentInChildren<Transform>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("CollisionChargeStation");
            OnPlayerInChargingStation?.Invoke(true, other.gameObject, this);
            UIManager.Instance.ShowChargeStationUI();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        OnPlayerInChargingStation?.Invoke(false, other.gameObject, this);
        UIManager.Instance.HideChargeStationUI();
    }
    
        public void TrySetCheckpoint(GameObject player)
    {
        player.GetComponent<PlayerRespawn>().SetCheckpoint(spawnPoint);
    }
}
