using UnityEngine;

[CreateAssetMenu(fileName = "Exoplanet", menuName = "Scriptable Objects/Exoplanet")]
public class Exoplanet : ScriptableObject
{
    public string planetName;
    public float mass;
    public float radius;
    public float distanceFromEarth;
    public float galacticLatitude;
    public float galacticLongitude;
}
