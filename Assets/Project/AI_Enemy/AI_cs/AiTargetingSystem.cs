using System.Collections;
using UnityEngine;
[ExecuteInEditMode]
public class AiTargetingSystem : MonoBehaviour
{
    [SerializeField] float memorySpan = 3.0f;
    [SerializeField] float distanceWeight = 1.0f;
    [SerializeField] float angleWeight = 1.0f;
    [SerializeField] float ageWeight = 1.0f;

    public bool HasTarget { get=> bestMemory != null;}
    public GameObject Target { get => bestMemory.gameObject;}
    public Vector3 TargetPosition { get => bestMemory.gameObject.transform.position;}
    public bool TargetInSight { get => bestMemory.Age < 0.5f;}
    public float TargetDistance { get => bestMemory.distance;}

    AiSensorMemory memory = new AiSensorMemory(10);
    AiSensor sensor;
    AiMemory bestMemory;

    void Start() {
        sensor = GetComponent<AiSensor>();
        
    }

    void Update()
    {
        memory.UpdateSenses(sensor);

        memory.ForgetMemories(memorySpan);
        EvaluateScores();
    }

    private void EvaluateScores() {
        bestMemory = null;
        foreach (var memory in memory.memories) {
            memory.score = CalculateScore(memory);
            if(bestMemory == null || memory.score > bestMemory.score) {
                bestMemory = memory;
            }
        }
    }
    float Normalize(float value, float maxValue) {
        return 1- (value / maxValue);
    }
    private float CalculateScore(AiMemory memory) {
        float distanceScore = Normalize(memory.distance, sensor.Distance) * distanceWeight;
        float angleScore = Normalize(memory.angle, sensor.Angle) * angleWeight;
        float ageScore = Normalize(memory.Age, memorySpan) * ageWeight;

        return distanceScore + angleScore + ageScore;
    }

    private void OnDrawGizmos() {
        // dam bao player o giua score cao hon
        float maxScore = float.MinValue;
        foreach (var memory in memory.memories) {
            maxScore = Mathf.Max(maxScore,memory.score);
        }
        //
        foreach (var memory in memory.memories) {
            Color color = Color.red;
            if(memory == bestMemory) {
                color = Color.yellow;
            }
            color.a = memory.score / maxScore;
            Gizmos.color = color;
            Gizmos.DrawSphere(memory.position, 0.2f);
        }
    }

}
