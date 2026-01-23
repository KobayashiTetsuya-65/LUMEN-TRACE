using UnityEngine;


public class SkyboxController : MonoBehaviour
{
    [Header("QÆ")]
    [SerializeField, Tooltip("“ú’†")] private Material _daySkybox;
    [SerializeField, Tooltip("–é’†")] private Material _nightSkybox;

    public void SetDay()
    {
        RenderSettings.skybox = _daySkybox;
        DynamicGI.UpdateEnvironment();
    }

    public void SetNight()
    {
        RenderSettings.skybox = _nightSkybox;
        DynamicGI.UpdateEnvironment();
    }
}
