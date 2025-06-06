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
    private Material unlitMaterial;
    [SerializeField]
    private Material litMaterial;
    [SerializeField]
    private SpriteRenderer targetSprite;

    [SerializeField]
    private bool isInChargingStation;   
    public bool lightActive = false;
    private bool canShoot = true;

    GameObject passiveLight;
    

    
    public void Start()
    {
        passiveLight = GameObject.Find("Light 2D Passiv");
		FlashLightOff();
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
                FlashLightOff();
			}
            else
            {
                FlashLightOn();
			}

        if(Input.GetKeyDown(KeyCode.Q) && canShoot && batterySO.energy > batterySO.shootEnergyConsumption)
        {
            Shoot();
            StartCoroutine(ShotCooldown());   
        }

        if (isInChargingStation && Input.GetKeyDown(KeyCode.E) &&!GameManager.Instance.gameOver)
        {
            Recharge();
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

    private void Recharge()
    {
        currentChargingStation.UpgradeMaxEnergy();
        currentChargingStation.anim.SetBool("isActivated", true);
		batterySO.energy = batterySO.maxEnergy;
        Light2D light = currentChargingStation.GetComponentInChildren<Light2D>();
        StartCoroutine(SmoothExpandLight(light, 5f, 1.5f)); // Dauer: 1.5 Sekunden
        UIManager.Instance.UpdateBatteryChargeUI();
            
        currentChargingStation.TrySetCheckpoint(gameObject);
    }

    private IEnumerator SmoothExpandLight(Light2D light, float targetRadius, float duration)
{
    float startRadius = light.pointLightOuterRadius;
    float time = 0f;

    while (time < duration)
    {
        time += Time.deltaTime;
        float t = time / duration;
        light.pointLightOuterRadius = Mathf.Lerp(startRadius, targetRadius, t);
        yield return null;
    }

    light.pointLightOuterRadius = targetRadius; // Sicherstellen, dass es exakt ankommt
}

    private void FlashLightOff()
    {
        targetSprite.material = litMaterial;
        playerLight.enabled = false;
        passiveLight.gameObject.SetActive(true);
        lightActive = false;
    }
    void FlashLightOn()
    {
		targetSprite.material = unlitMaterial;
		playerLight.enabled = true;
		passiveLight.gameObject.SetActive(false);
		lightActive = true;
	}
    

    public void Shoot()
    {
            batterySO.energy -= batterySO.shootEnergyConsumption;
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
            float shootForce = 5f;

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
        while (true)
        {
			while (!GameManager.Instance.gameOver)
			{
				if (lightActive && batterySO.energy > 0)
				{
					batterySO.energy -= batterySO.lightOnEnergyConsumption;
					UIManager.Instance.UpdateBatteryChargeUI();
				}
				else if (!lightActive && batterySO.energy > 0)
				{
					batterySO.energy -= batterySO.passiveEnergyConsumption;
					UIManager.Instance.UpdateBatteryChargeUI();
				}

				if (batterySO.energy <= 0)
				{
					batterySO.energy = 0;
					UIManager.Instance.UpdateBatteryChargeUI();
					FlashLightOff();
					GameManager.Instance.GameOver();
				}

				yield return new WaitForSeconds(1f);
			}

            yield return new WaitForSeconds(1f);
		}   
        
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
			UIManager.Instance.UpdateBatteryChargeUI();
			FlashLightOff();
			GameManager.Instance.GameOver();
		}
	}
}
