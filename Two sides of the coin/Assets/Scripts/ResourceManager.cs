using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Water Resource")]
    [SerializeField] private float waterCapacity;
    [SerializeField] private int population;
    [SerializeField] private int waterNeededForVillage;
    [SerializeField] private int waterNeededForCity;
    [SerializeField] private float economy;
    [SerializeField] private float rainProbability;

    private void Update()
    {
        
    }
}
