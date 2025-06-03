using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;


public class PlayerLighting : MonoBehaviour
{
    [SerializeField] 
    private Light2D playerLight;
    [SerializeField]
    private BatterySO batterySO;    
    [SerializeField]
    private float shootCooldown = 5f;
    [SerializeField]
    private Material unlitMaterial;
    [SerializeField]
    private Material litMaterial;
    [SerializeField]
    private SpriteRenderer targetSprite;

    public bool lightActive;
    private bool canShoot = true;
    


    public void Start()
    {
        StartCoroutine(LoseEnergy());
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

        if(Input.GetKeyDown(KeyCode.Q) && canShoot)
        {
            Shoot();
            StartCoroutine(ShotCooldown());   
        }

    }

    public void Shoot()
    {
            GameObject lightOrb =  LightOrbPool.Instance.GetPooledLightOrb();
            lightOrb.transform.position = transform.position * 2;
            lightOrb.gameObject.SetActive(true);
            lightOrb.transform.position = transform.position;

            // Richtung zur Maus
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 shootDirection = mouseWorldPos - (Vector2)transform.position;

            // Schusskraft anpassen je nach Gefühl (z. B. 15f)
            float shootForce = 15    ;

            lightOrb.GetComponent<LightOrbBehaviour>().ShootLightOrb(shootDirection, shootForce);
   
    }

    IEnumerator ShotCooldown()
    {
        canShoot = false;
        Debug.Log("cooldown");
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
                Debug.Log("Test2");
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
		}
		if (batterySO.energy <= 0)
		{
			batterySO.energy = 0;
			GameManager.Instance.gameOver = true;
		}
	}
}
