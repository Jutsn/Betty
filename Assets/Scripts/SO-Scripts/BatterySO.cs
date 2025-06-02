using UnityEngine;

[CreateAssetMenu(fileName = "Battery", menuName = "Scriptable Objects/Battery")]
public class BatterySO : ScriptableObject
{
	public int maxEnergy;
	public int energy;
}
