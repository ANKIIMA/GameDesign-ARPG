using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnvironmentManager : MonoBehaviour
{
    [Header("Base Wind")]
    [Tooltip("Base wind animate the trunks")]
    [Range(0f, 5f)]
    public float baseWindPower = 3f;
    [Tooltip("Base wind animate the trunks")]
    public float baseWindSpeed = 1f;


    [Header("Wind Bursts")]
    [Tooltip("Bursts are managed by a moving World-Space noise that multiply the base wind speed and power")]
    [Range(0f, 10f)]
    public float burstsPower = 0.5f;
    [Tooltip("Bursts are managed by a moving World-Space noise that multiply the base wind speed and power")]
    public float burstsSpeed = 5f;
    [Tooltip("Bursts are managed by a moving World-Space noise that multiply the base wind speed and power")]
    public float burstsScale = 10f;

    [Header("Micro Wind")]
    [Tooltip("Micro wind animate the leaves")]
    [Range(0f, 1f)]
    public float microPower = 0.1f;
    [Tooltip("Micro wind animate the leaves")]
    public float microSpeed = 1f;
    [Tooltip("Micro wind animate the leaves")]
    public float microFrequency = 3f;

    [Header("Grass")]
    public float renderDistance = 30f;

    [Header("Debug")]
    public bool showWind;

    void Update()
    {
        Shader.SetGlobalFloat("WindPower", baseWindPower);
        Shader.SetGlobalFloat("WindSpeed", baseWindSpeed);
        Shader.SetGlobalFloat("WindBurstsPower", burstsPower);
        Shader.SetGlobalFloat("WindBurstsSpeed", burstsSpeed);
        Shader.SetGlobalFloat("WindBurstsScale", burstsScale);
        Shader.SetGlobalFloat("MicroPower", microPower);
        Shader.SetGlobalFloat("MicroSpeed", microSpeed);
        Shader.SetGlobalFloat("MicroFrequency", microFrequency);
        Shader.SetGlobalFloat("GrassRenderDist", renderDistance);

        if (showWind == true)
        {
            Shader.EnableKeyword("_WINDDEBUGVIEW_ON");
        }
        else if (showWind == false)
        {
            Shader.DisableKeyword("_WINDDEBUGVIEW_ON");
        }
    }
}