
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private Slider waterSlider;

    [Header("Resource Text")]
    [SerializeField] private TMP_Text waterText;
    [SerializeField] private TMP_Text economyText;

    [Header("Population Text")]
    [SerializeField] private TMP_Text cityPopulationText;
    [SerializeField] private TMP_Text villagePopulationText;

    [Header("Water Distribution")]
    [SerializeField] private TMP_Text cityWaterText;
    [SerializeField] private TMP_Text villageWaterText;

    [Header("Day and Weather")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text rainText;

    private void Update()
    {
        if (resourceManager == null)
            return;

        UpdateResources();
        UpdatePopulation();
        UpdateWaterDistribution();
        UpdateDayAndWeather();
    }

    private void UpdateResources()
    {
        waterText.text =
            $"Water: {resourceManager.WaterCapacity:F0} L";

        economyText.text =
            $"Economy: {resourceManager.Economy:F0}";
    }

    private void UpdatePopulation()
    {
        cityPopulationText.text =
            $"City: {resourceManager.CityPopulation}";

        villagePopulationText.text =
            $"Village: {resourceManager.VillagePopulation}";
    }

    private void UpdateWaterDistribution()
    {
        if (waterSlider == null)
            return;

        float villagePercent = waterSlider.value * 100f;
        float cityPercent = (1f - waterSlider.value) * 100f;

        cityWaterText.text =
            $"City Allocation: {cityPercent:F0}%";

        villageWaterText.text =
            $"Village Allocation: {villagePercent:F0}%";
    }

    private void UpdateDayAndWeather()
    {
        dayText.text = $"Day {resourceManager.CurrentDay}";

        timerText.text =
            $"Next day: {resourceManager.TimeUntilNextDay:F0}s";

        rainText.text =
            $"Rain Chance: {resourceManager.RainProbability:F0}%";
    }
}
