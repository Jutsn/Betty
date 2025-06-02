using UnityEngine;
using UnityEngine.Rendering.Universal;


public class PlayerLighting : MonoBehaviour
{
    [SerializeField] 
    private Light2D playerLight;
    private bool lightActive;


    public void Update()
    {
        GetInput();
    }
    public void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.F) )
            if(lightActive)
            {
                playerLight.enabled = false;
                lightActive = false;
            }
            else{
            playerLight.enabled = true;
            lightActive = true;
            }
    }
}
