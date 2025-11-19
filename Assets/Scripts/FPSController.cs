using UnityEngine;

public class FPSController : MonoBehaviour
{
    public int targetFrameRate = 60;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
}
