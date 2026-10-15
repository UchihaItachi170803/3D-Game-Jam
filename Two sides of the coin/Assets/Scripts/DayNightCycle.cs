using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Cycle Settings")]
    public float dayInMinutes = 15f;
    [Range(0f, 1f)] public float timeOfDay = 0.25f;

    [Header("Skybox Materials (FS002)")]
    public Material morningSky;    // FS002_Sunrise
    public Material daySky;        // FS002_Day
    public Material eveningSky;    // FS002_Sunset
    public Material nightSky;      // FS002_Night
    public Material rainySky;      // FS002_Rainy

    [Header("Rain System")]
    public bool isRaining = false;
    public ParticleSystem rainEffect;

    [Header("Lighting")]
    public Light sunLight;

    // Internal smoothed targets
    private Color targetLightColor;
    private float targetIntensity;
    private Material targetSky;
    private float rainAlpha = 0f;

    void Start()
    {
        if (sunLight == null) sunLight = GetComponent<Light>();
    }

    void Update()
    {
        // 1. Advance time smoothly
        timeOfDay += Time.deltaTime / (dayInMinutes * 60f);
        if (timeOfDay >= 1f) timeOfDay -= 1f;

        // 2. Rotate the sun smoothly
        float sunAngle = (timeOfDay * 360f) - 90f;
        transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // 3. Smooth transitions
        UpdateTransitions();
    }

    void UpdateTransitions()
    {
        if (timeOfDay < 0.25f) // Morning
        {
            targetSky = morningSky;
            targetLightColor = new Color(1f, 0.78f, 0.55f);
            targetIntensity = 0.85f;
        }
        else if (timeOfDay < 0.50f) // Day
        {
            targetSky = daySky;
            targetLightColor = new Color(1f, 0.98f, 0.92f);
            targetIntensity = 1.25f;
        }
        else if (timeOfDay < 0.75f) // Evening
        {
            targetSky = eveningSky;
            targetLightColor = new Color(1f, 0.45f, 0.25f);
            targetIntensity = 0.75f;
        }
        else // Night
        {
            targetSky = nightSky;
            targetLightColor = new Color(0.2f, 0.25f, 0.5f);
            targetIntensity = 0.15f;
        }

        // Handle rain override
        if (isRaining)
        {
            targetSky = rainySky;
            targetLightColor = new Color(0.45f, 0.5f, 0.6f);
            targetIntensity = 0.35f;
            rainAlpha = Mathf.MoveTowards(rainAlpha, 1f, Time.deltaTime * 0.8f);
        }
        else
        {
            rainAlpha = Mathf.MoveTowards(rainAlpha, 0f, Time.deltaTime * 0.8f);
        }

        // Apply smoothed light color and intensity
        if (sunLight != null)
        {
            sunLight.color = Color.Lerp(sunLight.color, targetLightColor, Time.deltaTime * 1.5f);
            sunLight.intensity = Mathf.Lerp(sunLight.intensity, targetIntensity, Time.deltaTime * 1.5f);
        }

        // Smooth ambient environment light
        RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, targetLightColor * 0.3f, Time.deltaTime * 1.5f);

        // Swap skybox material
        if (targetSky != null && RenderSettings.skybox != targetSky)
        {
            RenderSettings.skybox = targetSky;
        }

        // Fade rain particle emission
        if (rainEffect != null)
        {
            var emission = rainEffect.emission;
            emission.rateOverTime = 400f * rainAlpha;
            if (rainAlpha > 0.05f && !rainEffect.isPlaying) rainEffect.Play();
            else if (rainAlpha <= 0.05f && rainEffect.isPlaying) rainEffect.Stop();
        }
    }
}