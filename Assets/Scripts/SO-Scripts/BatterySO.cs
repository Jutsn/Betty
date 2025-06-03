using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Battery", menuName = "Scriptable Objects/Battery")]
public class BatterySO : ScriptableObject
{
	public float maxEnergy;
	public float energy;
	public float passiveEnergyConsumption;
}
