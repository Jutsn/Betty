using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;


public class PlayerLighting : MonoBehaviour
{
    [SerializeField] 
    private Light2D playerLight;
    [SerializeField]
    private BatterySO batterySO;    
    private ChargingStation currentChargingStation;
    [SerializeField]
    private float shootCooldown = 5f;
    [SerializeField]
    private float shootCost = 10f;
    [SerializeField]
    private Material unlitMaterial;
    [SerializeField]
    private Material litMaterial;
    [SerializeField]
    private SpriteRenderer targetSprite;

    [SerializeField]
    private bool isInChargingStation;   
    public bool lightActive;
    private bool canShoot = true;
    


    public void Start()
    {
        StartCoroutine(LoseEnergy());
    }

    private void OnEnable() 
    {
        ChargingStation.OnPlayerInChargingStation += HandleChargingStation;  
    }

    private void OnDisable()
    {
        ChargingStation.OnPlayerInChargingStation -= HandleChargingStation;
    }

    public void Update()
    {
        GetInput();
    }
    public void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.F) )
            if(lightActive)
            {
                targetSprite.material = litMaterial;
                playerLight.enabled = false;
                lightActive = false;
            }
            else{
                targetSprite.material = unlitMaterial;
                playerLight.enabled = true;
                lightActive = true;
            }

        if(Input.GetKeyDown(KeyCode.Q) && canShoot && batterySO.energy > shootCost)
        {
            Shoot();
            StartCoroutine(ShotCooldown());   
        }

        if (isInChargingStation && Input.GetKeyDown(KeyCode.E))
        {
            batterySO.energy = batterySO.maxEnergy;
            UIManager.Instance.UpdateBatteryChargeUI();
            Debug.Log("Battery recharged!");
            
            currentChargingStation.TrySetCheckpoint(gameObject);
        }
    }

    private void HandleChargingStation(bool entered, GameObject player,ChargingStation station)
    {
        Debug.Log("EventInvoked");
        if (player == this.gameObject)
        {
            isInChargingStation = entered; 
            currentChargingStation = station;   
        }
    }


    

    public void Shoot()
    {
            batterySO.energy -= shootCost;
            UIManager.Instance.UpdateBatteryChargeUI();
            GameObject lightOrb =  LightOrbPool.Instance.GetPooledLightOrb();
            if (lightOrb != null)
            {
            lightOrb.gameObject.SetActive(true);
            lightOrb.transform.position = transform.position;

            // Richtung zur Maus
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 shootDirection = mouseWorldPos - (Vector2)transform.position;

            // Schusskraft anpassen je nach Gefühl (z. B. 15f)
            float shootForce = 15f;

            lightOrb.GetComponent<LightOrbBehaviour>().ShootLightOrb(shootDirection, shootForce);
            }

   
    }

    IEnumerator ShotCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    IEnumerator LoseEnergy()
    {
        while(!GameManager.Instance.gameOver)
        {
            Debug.Log("Test1");
            while (lightActive && batterySO.energy > 0)
            {
                batterySO.energy -= batterySO.passiveEnergyConsumption;
                UIManager.Instance.UpdateBatteryChargeUI();
                yield return new WaitForSeconds(1f);
            }
            if (batterySO.energy <= 0)
            {
				batterySO.energy = 0;
                GameManager.Instance.gameOver = true;

			}
            yield return new WaitForSeconds(1f);
        }
            
        yield return null;
    }

	public void GetEnergyDamage(float damage)
	{
		if (batterySO.energy > 0)
		{
			batterySO.energy -= damage;
			UIManager.Instance.UpdateBatteryChargeUI();
		}
		if (batterySO.energy <= 0)
		{
			batterySO.energy = 0;
			GameManager.Instance.gameOver = true;
			UIManager.Instance.UpdateBatteryChargeUI();
		}
	}
}
