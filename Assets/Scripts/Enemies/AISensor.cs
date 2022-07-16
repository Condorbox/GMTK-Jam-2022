using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    [SerializeField] private float distance = 10;
    [SerializeField] private float angle = 30;
    [SerializeField] private float heigh = 1.0f;
    [SerializeField] private Color debugMeshColor = Color.red;
    [SerializeField] private int scanFrequency = 30;
    [SerializeField] private LayerMask detectionLayers;
    [SerializeField] private LayerMask occlusionLayers;
    private List<GameObject> objectsDetected = new List<GameObject>();
    private Mesh debugMesh;
    private Collider[] detectedColliders = new Collider[50];
    private int count;
    private float scanInterval;
    private float scanTimer;
    public event EventHandler<Transform> OnPlayerDetected;

    private void Awake()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    private void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer <= 0)
        {
            scanTimer = scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, detectedColliders, detectionLayers, QueryTriggerInteraction.Collide);

        objectsDetected.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = detectedColliders[i].gameObject;
            if (IsInSight(obj))
            {
                objectsDetected.Add(obj);
                if (obj.CompareTag("Player"))
                {
                    OnPlayerDetected?.Invoke(this, obj.transform);
                }
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;

        if (direction.y < 0 || direction.y > heigh)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += heigh / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }
        return true;
    }

    private Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numOfTriangles = (segments * 4) + 2 + 2;
        int numVertices = numOfTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * heigh;
        Vector3 topRight = bottomRight + Vector3.up * heigh;
        Vector3 topLeft = bottomLeft + Vector3.up * heigh;

        int vertex = 0;

        //Left side
        vertices[vertex++] = bottomCenter;
        vertices[vertex++] = bottomLeft;
        vertices[vertex++] = topLeft;

        vertices[vertex++] = topLeft;
        vertices[vertex++] = topCenter;
        vertices[vertex++] = bottomCenter;

        //Right side
        vertices[vertex++] = bottomCenter;
        vertices[vertex++] = topCenter;
        vertices[vertex++] = topRight;

        vertices[vertex++] = topRight;
        vertices[vertex++] = bottomRight;
        vertices[vertex++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;

        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * heigh;
            topLeft = bottomLeft + Vector3.up * heigh;

            currentAngle += deltaAngle;

            //Far side
            vertices[vertex++] = bottomLeft;
            vertices[vertex++] = bottomRight;
            vertices[vertex++] = topRight;

            vertices[vertex++] = topRight;
            vertices[vertex++] = topLeft;
            vertices[vertex++] = bottomLeft;

            //top side
            vertices[vertex++] = topCenter;
            vertices[vertex++] = topLeft;
            vertices[vertex++] = topRight;

            //bottom side
            vertices[vertex++] = bottomCenter;
            vertices[vertex++] = bottomRight;
            vertices[vertex++] = bottomLeft;
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

    private void OnValidate()
    {
        debugMesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (debugMesh)
        {
            Gizmos.color = debugMeshColor;
            Gizmos.DrawMesh(debugMesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(detectedColliders[i].transform.position, .2f);
        }

        Gizmos.color = Color.green;
        foreach (GameObject obj in objectsDetected)
        {
            Gizmos.DrawSphere(obj.transform.position, .2f);
        }
    }

    public List<GameObject> GetObjectsDetected()
    {
        return objectsDetected;
    }
}
