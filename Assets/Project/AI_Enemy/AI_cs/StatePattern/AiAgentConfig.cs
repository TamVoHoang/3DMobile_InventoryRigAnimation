using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;

    public float dieForece = 100.0f;

    public float maxSightDistance = 5.0f;

}
