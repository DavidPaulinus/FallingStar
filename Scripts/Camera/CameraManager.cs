using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private CinemachineImpulseSource impulse;

    private void Awake()
    {
        Instance = this;

        impulse = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake()
    {
        impulse.GenerateImpulse();
    }

    public void Shake(float x, float y)
    {
        impulse.GenerateImpulse(new Vector2(x,y));
    }
}
