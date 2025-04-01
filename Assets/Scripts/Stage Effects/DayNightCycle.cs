using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour
{
    public Light sunLight;  // Assign the Directional Light
    public Color dayColor = new Color(1f, 0.95f, 0.8f); // Warm sunlight color
    public Color nightColor = new Color(0.1f, 0.1f, 0.2f); // Dark blue night color
    public float totalGameTime = 600f; // 10 minutes in seconds
    private float halfTime; // 5 minutes mark

    void Start()
    {
        halfTime = totalGameTime / 2; // Halfway point (5 minutes)
        StartCoroutine(DayNightTransition());
    }

    IEnumerator DayNightTransition()
    {
        float elapsedTime = 0f;

        while (elapsedTime < totalGameTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / totalGameTime; // 0 to 1 over 10 mins

            if (elapsedTime < halfTime) // Daytime
            {
                sunLight.color = Color.Lerp(nightColor, dayColor, progress * 2);
                sunLight.intensity = Mathf.Lerp(0.2f, 1f, progress * 2);
            }
            else // Nighttime
            {
                sunLight.color = Color.Lerp(dayColor, nightColor, (progress - 0.5f) * 2);
                sunLight.intensity = Mathf.Lerp(1f, 0.2f, (progress - 0.5f) * 2);
            }

            yield return null;
        }
    }
}
