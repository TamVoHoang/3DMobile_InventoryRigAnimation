using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[ExecuteInEditMode]
public class AiSensor : MonoBehaviour
{
    [SerializeField] private float distance = 10f;
    [SerializeField] private float angel = 30f;
    [SerializeField] private float height = 1.0f;
    [SerializeField] private Color meshColor = Color.blue;

    //detect sensor
    [SerializeField] private int scanFrequency = 30;
    [SerializeField] private LayerMask layers;
    public List<GameObject> Objects = new List<GameObject>();
    private Collider[] colliders = new Collider[50];
    Mesh mesh;

    private int count;
    float scanInterval;
    float scanTimer;

    private void Start() {
        scanInterval = 1.0f / scanFrequency;
    }

    private void Update() {
        scanTimer -= Time.deltaTime;
        if(scanTimer < 0) {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        Objects.Clear(); //xoa list objects
        for (int i = 0; i < count; i++) {
            GameObject obj = colliders[i].gameObject;
            if(IsInSight(obj)) {
                Objects.Add(obj);
            }
        }
    }
    private bool IsInSight(GameObject obj) {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest- origin;
        if(direction.y < 0 || direction.y > height) return false;

        direction.y = 0;
        float deltaAngel = Vector3.Angle(direction, transform.forward);
        if(deltaAngel > angel) return false;
        
        return true;
    }

    private Mesh CreateWedgeMesh() {
        Mesh mesh = new Mesh();

        int segments = 10;

        int numRiangels = (segments * 4) + 2 + 2;
        int numVertices = numRiangels * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angel,0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angel,0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        
        int vert = 0;

        //left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngel = - angel;
        float deltaAngel = (angel * 2) / segments;
        for (int i = 0; i < segments; i++) {
            bottomLeft = Quaternion.Euler(0, currentAngel,0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngel + deltaAngel,0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngel += deltaAngel;
        }

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();


        return mesh;
    }

    private void OnValidate() {
        mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos() {
        if(mesh) {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in Objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
            
        }
    }

}
