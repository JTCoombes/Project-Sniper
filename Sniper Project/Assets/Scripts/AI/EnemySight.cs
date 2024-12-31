using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

[ExecuteInEditMode]
public class EnemySight : MonoBehaviour
{
    public float distance = 10f;
    public float height = 1f;
    public float angle = 30;
    public Color meshColor = Color.white;

    public float ScanFrequency = 30;
    public LayerMask DetectMask;
    public LayerMask OcclusionMask;

    public List<GameObject> Objs = new List<GameObject>();  
    Collider[] colliders = new Collider[50];
    int count;
    float ScanInterval;
    float Scantimer;
    Mesh mesh;

    private void Start()
    {
        ScanInterval = 1.0f / ScanFrequency;
    }

    private void Update()
    {
        Scantimer -= Time.deltaTime;
        if(Scantimer <= 0)
        {
            Scantimer += ScanInterval;
            Scan();
        }
    }

    void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, DetectMask, QueryTriggerInteraction.Collide);

        Objs.Clear();  

        for (int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;
            if (Insight(obj))
            {
                Objs.Add(obj);
            }
        }

    }

    public bool Insight(GameObject obj)
    {
        Vector3 orgin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - orgin;

        if(direction.y <0 || direction.y > height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if(deltaAngle > angle)
        {
            return false;
        }

        //orgin.y += height / 2;
        dest.y = orgin.y;
        if(Physics.Linecast(orgin, dest, OcclusionMask))
        {
            return false;
        }
        return true;
    }

    Mesh createWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numtriangles = (segments * 4) +2+2;
        int numVertices = numtriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int [] triangles = new int[numVertices];

        Vector3 bottomCentre = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0,-angle,0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0,angle,0) * Vector3.forward * distance;

        Vector3 TopCentre = bottomCentre + Vector3.up * height;
        Vector3 TopLeft = bottomLeft + Vector3.up * height;
        Vector3 TopRight = bottomRight + Vector3.up * height;

        int vert = 0;

        //left side
        vertices[vert++] = bottomCentre;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = TopLeft;


        vertices[vert++] = TopLeft;
        vertices[vert++] = TopCentre;
        vertices[vert++] = bottomCentre;

        //right side
        vertices[vert++] = bottomCentre;
        vertices[vert++] = TopCentre;
        vertices[vert++] = TopRight;


        vertices[vert++] = TopRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCentre;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i< segments; i++)
        {
           
             bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

           
             TopLeft = bottomLeft + Vector3.up * height;
             TopRight = bottomRight + Vector3.up * height;

            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = TopRight;


            vertices[vert++] = TopRight;
            vertices[vert++] = TopLeft;
            vertices[vert++] = bottomLeft;
            //top
            vertices[vert++] = TopCentre;
            vertices[vert++] = TopLeft;
            vertices[vert++] = TopRight;

            //bottom
            vertices[vert++] = bottomCentre;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;

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
        mesh = createWedgeMesh();
        ScanInterval = 1.0f / ScanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i<count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(colliders[i].transform.position,.5f);
        }

        Gizmos.color = Color.green;
        foreach (var objects in Objs)
        {
            Gizmos.DrawSphere(objects.transform.position, .5f);
        } 


    }


    /* old Script
    public float height;
    public float distance;
    public float angle;
    public Color meshColor = Color.red;

    public List<GameObject> Objects = new List<GameObject>();
    Collider[] colliders = new Collider[50];
    public int scanfrequency = 30;
    public LayerMask detectLayer;
    public LayerMask blockLayer;

    [Space]
    int count;
    float scaninterval;
    float scantimer;

    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        scaninterval = 1.0f / scanfrequency;
    }

    // Update is called once per frame
    void Update()
    {
        scantimer -= Time.deltaTime;
        if (scantimer <= 0)
        {
            scantimer += scaninterval;
            Scan();
        }


    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, detectLayer, QueryTriggerInteraction.Collide);

        Objects.Clear();

        for (int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;

            if (IsInSight(obj))
            {
                Objects.Add(obj);
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 destination = obj.transform.position;
        Vector3 direction = destination - origin;

        if (direction.z < 0 || direction.z > height)
        {
            return false;
        }

        direction.z = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.z += height / 2;
        destination.z = origin.z;
        if (Physics.Linecast(origin, destination, blockLayer))
        {
            return false;
        }

        return true;
    }

    private void OnValidate()
    {
        mesh = createWedgeMesh();
        scaninterval = 1.0f / scanfrequency;
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; ++i)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        
        foreach (var obj in Objects)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(obj.transform.position, .2f);
        }
    }

    Mesh createWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int TriangleAmount = (segments * 4) + 2 + 2;
        int Vertamount = TriangleAmount * 3;

        Vector3[] Verts = new Vector3[Vertamount];
        int[] tris = new int[Vertamount];

        Vector3 BotCenter = Vector3.zero;
        Vector3 BotLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 BotRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 TopCenter = BotCenter + Vector3.up * height;
        Vector3 TopLeft = BotLeft + Vector3.up * height;
        Vector3 TopRight = BotRight + Vector3.up * height;

        int Vert = 0;

        // Left

        Verts[Vert++] = BotCenter;
        Verts[Vert++] = BotLeft;
        Verts[Vert++] = TopLeft;

        Verts[Vert++] = TopLeft;
        Verts[Vert++] = TopCenter;
        Verts[Vert++] = BotCenter;

        // Right
        Verts[Vert++] = BotCenter;
        Verts[Vert++] = TopCenter;
        Verts[Vert++] = TopRight;

        Verts[Vert++] = TopRight;
        Verts[Vert++] = BotRight;
        Verts[Vert++] = BotCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; i++)
        {

            BotLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            BotRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;


            TopLeft = BotLeft + Vector3.up * height;
            TopRight = BotRight + Vector3.up * height;

            // Far Side
            Verts[Vert++] = BotLeft;
            Verts[Vert++] = BotRight;
            Verts[Vert++] = TopRight;

            Verts[Vert++] = TopRight;
            Verts[Vert++] = TopLeft;
            Verts[Vert++] = BotLeft;

            // Top
            Verts[Vert++] = TopCenter;
            Verts[Vert++] = TopLeft;
            Verts[Vert++] = TopRight;


            // bottom
            Verts[Vert++] = BotCenter;
            Verts[Vert++] = BotRight;
            Verts[Vert++] = BotLeft;

            currentAngle += deltaAngle;
        }



        for (int i = 0; i < Vertamount; i++)
        {
            tris[i] = i;
        }
        mesh.vertices = Verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        return mesh;
    }
    */
}
