using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Water Resource")]
    [SerializeField] private float waterCapacity;
    [SerializeField] private int populationVillage;
    [SerializeField] private int populationCity;
    [SerializeField] private float waterNeededForVillage;
    [SerializeField] private float waterNeededForCity;
    public float economy;
    [SerializeField] private float rainProbability;
    private bool IsRained;
    [SerializeField] private float speed;
    [SerializeField] private int waterSupplyedToVillage;
    [SerializeField] private int waterSupplyedToCity;
    [SerializeField] private ObjectDataBaseSO dataBase;
    private float RainTimer = 9f;



    private void Start()
    {
        waterCapacity = 1000f;
        economy = 0;
        populationCity = 1000;
        populationVillage = 500;
        rainProbability = 10;
        Debug.Log(waterCapacity);

    }
    private void Update()
    {
        Movement();
        TimerElapsed();

    }

    private void TimerElapsed()
    {
        //Rain
        RainTimer -= Time.deltaTime;
        if (RainTimer <= 0)
        {
            RainTimer = 9f;
            economy += 50f;
            Debug.Log("Economy :" + economy);
            RainSystem();
        }
    }

    private void Movement()
    {
        var Horizontal = Input.GetAxisRaw("Horizontal");
        var Vertical = Input.GetAxisRaw("Vertical");
        transform.position +=new Vector3 (Horizontal,0, Vertical)*speed * Time.deltaTime;
       
    }

    private void RainSystem()
    {
        int RandomChanceToRain = Random.Range(1, 100);
        Debug.Log("Chance :" + RandomChanceToRain);
        if(RandomChanceToRain <= rainProbability)
        {
            //TODO : Add particle Effect
            IsRained = true;
            float waterAddedToWaterCapacity = waterCapacity * 0.02f;
            waterCapacity += waterAddedToWaterCapacity;
            Debug.Log("newWaterCapacity : " + waterCapacity);
        }
        else
        {
            Debug.Log("I am not gonna Rain");
        }
    }

    private void WaterConsumption()
    {

    }
    public void Tree()
    {
        economy -= 30;
        rainProbability += 0.2f;
        Debug.Log("Economy after Tree Purchase :" + economy);
        Debug.Log("New RainProbability :" + rainProbability);
    }

    private void RainWaterHarvestingSystem()
    {
        if(IsRained)
        {
            waterCapacity += 0.3f;
        }
    }
    
    private void GroundWaterSystem()
    {
        waterCapacity += 0.4f;
    }

    private void WasteWaterRecycleSystem()
    {
        waterCapacity += 0.8f;
    }

    private void EconomyStuff()
    {
        if(RainTimer <= 0)
        {

        }
    }

    private void WaterNeed()
    {
        waterNeededForCity = waterCapacity * 0.8f;
        waterNeededForVillage = waterCapacity * 0.6f;

    }
}
