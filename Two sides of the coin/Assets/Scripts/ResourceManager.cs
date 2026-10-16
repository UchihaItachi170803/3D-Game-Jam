using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private ObjectDataBaseSO dataBase;
    [SerializeField] private Slider waterSlider;

    float waterRateInCity = 0.5f; //Per Person
    float waterRateInVillage = 0.2f; //Per Person

    float perpersonWaterNeedCity = 8;
    float perpersonWaterNeedVillage = 6;

    private float RainTimer = 20f;
    int harvestCounter = 0;
    float TenSecTimer = 10f;

    float villagePercentage;
    float cityPercentage;
    private float dailyWaterBonus;



    public float WaterCapacity => waterCapacity;
    public float Economy => economy;

    public int CityPopulation => populationCity;
    public int VillagePopulation => populationVillage;

    public float RainProbability => rainProbability;

    public int CurrentDay { get; private set; } = 1;

    public float TimeUntilNextDay => Mathf.Max(0f, RainTimer);




    private void Start()
    {
        waterCapacity = 1000f;
        economy = 0;
        populationCity = 100;
        populationVillage = 100;
        Debug.Log(waterCapacity);

    }
    private void Update()
    {
        Movement();
        TimerElapsed();
        WaterLevelCheck();
    }

    private void Movement()
    {
        var Horizontal = Input.GetAxisRaw("Horizontal");
        var Vertical = Input.GetAxisRaw("Vertical");
        transform.position += new Vector3(Vertical, 0, -Horizontal) * speed * Time.deltaTime;

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
                AddedWater = harvestCounter * 60f;
            }
            else
            {
                AddedWater = 20f;
            }

            dailyWaterBonus += AddedWater;
        }
        else
        {
            Debug.Log("I am not gonna Rain");
        }
    }

    public void Tree()
    {
        economy -= 30;
        rainProbability += 0.2f;
    }

    public void RainWaterHarvestingSystem()
    {
        economy -= 100;
        harvestCounter++;

    }

    public void GroundWaterSystem()
    {
        economy -= 150;
        dailyWaterBonus += 60f;
    }

    public void WasteWaterRecycleSystem()
    {
        economy -= 250;
        dailyWaterBonus += 80f;
    }

    public void SliderInfo()
    {
        waterNeededForCity = 800f;
        waterNeededForVillage = 600f;

        villagePercentage = waterSlider.value;
        cityPercentage = 1f - villagePercentage;

        float cityWater = waterNeededForCity * cityPercentage;
        float villageWater = waterNeededForVillage * villagePercentage;

        Debug.Log("City received: " + cityWater);
        Debug.Log("Village received: " + villageWater);
    }



    private void WaterSupply()
    {
        
        waterNeededForCity =
            populationCity * perpersonWaterNeedCity;

        waterNeededForVillage =
            populationVillage * perpersonWaterNeedVillage;

       
        villagePercentage = waterSlider.value;
        cityPercentage = 1f - villagePercentage;

        
        float cityWater =
            waterNeededForCity * cityPercentage;

        float villageWater =
            waterNeededForVillage * villagePercentage;

        
        float totalWaterConsumed = cityWater + villageWater;

        waterCapacity -= totalWaterConsumed;
        waterCapacity = Mathf.Max(0f, waterCapacity);

        Debug.Log("City Water Demand: " + waterNeededForCity);
        Debug.Log("Village Water Demand: " + waterNeededForVillage);
        Debug.Log("City Water Supplied: " + cityWater);
        Debug.Log("Village Water Supplied: " + villageWater);
        Debug.Log("Remaining Water: " + waterCapacity);
    }


    private void WaterLevelCheck()
    {
        villagePercentage = waterSlider.value;
        cityPercentage = 1f - villagePercentage;
        // Village
        if (villagePercentage <= 0.2f)
        {
            waterRateInVillage = 0f;
            TenSecTimer -= Time.deltaTime;

            if (TenSecTimer <= 0f)
            {
                Debug.Log("Village Game Over");
                Time.timeScale = 0;
                TenSecTimer = 10f;
            }
        }
        else if (villagePercentage <= 0.3f)
        {
            waterRateInVillage = 0.1f;
        }
        else if (villagePercentage <= 0.5f)
        {
            waterRateInVillage = 0.2f;
        }
        else
        {
            waterRateInVillage = 0.2f;
        }

        // City
        if (cityPercentage <= 0.2f)
        {
            waterRateInCity = 0f;
            TenSecTimer -= Time.deltaTime;

            if (TenSecTimer <= 0f)
            {
                Debug.Log("City Game Over");
                Time.timeScale = 0;
                TenSecTimer = 10f;
            }
        }
        else if (cityPercentage <= 0.3f)
        {
            waterRateInCity = 0.1f;
        }
        else if (cityPercentage <= 0.5f)
        {
            waterRateInCity = 0.4f;
        }
        else
        {
            waterRateInCity = 0.5f;
        }
    }


    private void TimerElapsed()
    {
        RainTimer -= Time.deltaTime;

        if (RainTimer <= 0f)
        {
            RainTimer = 20f;

            economy += (waterRateInCity * populationCity)
                     + (waterRateInVillage * populationVillage);

            WaterSupply();
            RainSystem();
            populationVillage += 2;
            populationCity += 5;
            waterCapacity = 1000f + dailyWaterBonus;
            CurrentDay++;

            Debug.Log("Economy: " + economy);
            Debug.Log("Water Remaining: " + waterCapacity);
            Debug.Log("Village Population: " + populationVillage);
            Debug.Log("City Population: " + populationCity);
            Debug.Log("Day: " + CurrentDay);
        }
    }




}
