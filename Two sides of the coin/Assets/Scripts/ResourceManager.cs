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
    [SerializeField] private float speed;
    [SerializeField] private ObjectDataBaseSO dataBase;
    [SerializeField] private Slider waterSlider;
    [SerializeField] private ParticleSystem rainvfx;
    [SerializeField] private GameObject Gameover;

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

    [SerializeField] private float villageSatisfaction;
    [SerializeField] private float citySatisfaction;


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
        rainvfx.Stop();
        Gameover.SetActive(false);

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
        rainvfx.Stop();
        int RandomChanceToRain = Random.Range(1, 101);
        Debug.Log("Chance :" + RandomChanceToRain);

        if (RandomChanceToRain <= rainProbability)
        {
            rainvfx.Play();
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
        Debug.Log("waterNeededForCity :" + waterNeededForCity);

        waterNeededForVillage =
            populationVillage * perpersonWaterNeedVillage;
        Debug.Log("waterNeededForVillage :" + waterNeededForVillage);


        villagePercentage = waterSlider.value;
        cityPercentage = 1f - villagePercentage;

        
        float cityWater =
            waterNeededForCity * cityPercentage;

        float villageWater =
            waterNeededForVillage * villagePercentage;

        
        float totalWaterConsumed = cityWater + villageWater;

        waterCapacity -= totalWaterConsumed;
        waterCapacity = Mathf.Max(0f, waterCapacity);

        
    }


    private void WaterLevelCheck()
    {
        villagePercentage = waterSlider.value;
        cityPercentage = 1f - villagePercentage;

        
        float cityNeed = populationCity * perpersonWaterNeedCity;
        float villageNeed = populationVillage * perpersonWaterNeedVillage;

        
        float cityProvided = waterCapacity * cityPercentage;
        float villageProvided = waterCapacity * villagePercentage;

        
        villageSatisfaction = villageNeed > 0f ? villageProvided / villageNeed : 1f;
        citySatisfaction = cityNeed > 0f ? cityProvided / cityNeed : 1f;

        // Village
        if (villageSatisfaction <= 0.2f)
        {
            waterRateInVillage = 0f;
            TenSecTimer -= Time.deltaTime;

            if (TenSecTimer <= 0f)
            {
                Debug.Log("Village Game Over");
                Gameover.SetActive(true);
                Time.timeScale = 0;

                TenSecTimer = 10f;
            }
        }
        else if (villageSatisfaction <= 0.3f)
        {
            waterRateInVillage = 0.1f;
            Debug.Log("Village 1");
        }
        else if (villageSatisfaction <= 0.5f)
        {
            waterRateInVillage = 0.2f;
            Debug.Log("Village 2");
        }
        else
        {
            waterRateInVillage = 0.2f;
            Debug.Log("Village 3");
        }

        // City
        if (citySatisfaction <= 0.2f)
        {
            waterRateInCity = 0f;
            TenSecTimer -= Time.deltaTime;

            if (TenSecTimer <= 0f)
            {
                Debug.Log("City Game Over");
                Gameover.SetActive(true);
                Time.timeScale = 0;
                TenSecTimer = 10f;
            }
        }
        else if (citySatisfaction <= 0.3f)
        {
            waterRateInCity = 0.1f;
            Debug.Log("City 1");
        }
        else if (citySatisfaction <= 0.5f)
        {
            waterRateInCity = 0.4f;
            Debug.Log("City 2");
        }
        else
        {
            waterRateInCity = 0.5f;
            Debug.Log("City 1");
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

        }
    }




}
