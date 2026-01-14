using System.Collections;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// éwíËÇÃïbêîéûä‘Çé~ÇﬂÇÈ
    /// </summary>
    /// <param name="duration"></param>
    public void RequestHitStop(float duration)
    {
        StartCoroutine(HitStop(duration));
    }
    private IEnumerator HitStop(float duration)
    {
        float prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = prevTimeScale;
    }
}
