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

    float waterRateInCity = 0.5f; //Per Person
    float waterRateInVillage = 0.2f; //Per Person

    int perPersonWaterConsumptionCity = 8;
    int perPersonWaterConsumptionVillage = 6;
    private float RainTimer = 20f;
    int harvestCounter = 0;
    bool IsHarvest;



    private void Start()
    {
        waterCapacity = 1000f;
        economy = 0;
        populationCity = 100;
        populationVillage = 100;
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
            RainTimer = 20f;
            economy = (perPersonWaterConsumptionCity*populationCity) + (perPersonWaterConsumptionVillage*populationVillage);
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
        IsRained = false;
        int RandomChanceToRain = Random.Range(1, 101);
        Debug.Log("Chance :" + RandomChanceToRain);

        if (RandomChanceToRain <= rainProbability)
        {
            IsRained = true;
            float AddedWater;

            if (harvestCounter > 0) 
            {
                AddedWater = harvestCounter * 40f;
            }
            else
            {
                AddedWater = 20f;
            }

            waterCapacity += AddedWater;
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
    }

    public void RainWaterHarvestingSystem()
    {
        economy -= 100;
        IsHarvest = true;
        harvestCounter++;
        
    }
    
    public void GroundWaterSystem()
    {
        economy -= 150;
        waterCapacity += 60f;
    }

    public void WasteWaterRecycleSystem()
    {
        economy -= 250;
        waterCapacity += 80f;
    }

    private void EconomyStuff()
    {
        if(RainTimer <= 0)
        {

        }
    }

    public void WaterNeed()
    {
        //Per person spending 0.5 for water in city 
        // per person spending 0.2 for water in city
        waterNeededForCity = waterCapacity * 0.8f;
        waterNeededForVillage = waterCapacity * 0.6f;
        if(waterNeededForCity < waterNeededForCity * 0.2f)
        {

        }
        if(waterNeededForVillage < waterNeededForVillage * 0.2f)
        {

        }
        

    }
}
