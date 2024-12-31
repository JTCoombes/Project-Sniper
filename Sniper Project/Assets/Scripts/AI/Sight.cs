using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float ViewAngle;

    public LayerMask TargetMask, ObjectsMask;

    public List<Transform> VisibleTargets = new List<Transform>();

    public float meshRes;
    public int EdgeResolve;
    public float EdgeDistThreshold;


    public MeshFilter ViewMesh;
    private Mesh Mesh;

    public bool Insight;

    private void Start()
    {
        StartCoroutine("FindTargets", .2f);
        Mesh = new Mesh();
        Mesh.name = "View Area";
        ViewMesh.mesh = Mesh;
    }

    IEnumerator FindTargets(float Delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(Delay);
            FindTargets();

        }
    }

    private void LateUpdate()
    {
        DrawFieldOfView(); 
    }

    void FindTargets()
    {
        VisibleTargets.Clear();
        Collider[] targetsinViewRadius = Physics.OverlapSphere(transform.position, viewRadius, TargetMask);

        for (int i = 0; i < targetsinViewRadius.Length; i++)
        {
            Transform target = targetsinViewRadius[i].transform;
            Vector3 DirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, DirToTarget) < ViewAngle / 2)
            {
                float disToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, DirToTarget, disToTarget, ObjectsMask))
                {
                    
                    VisibleTargets.Add(target);
                    Insight = true;
                }
                else
                {
                    Insight = false;
                }
            }
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(ViewAngle * meshRes);

        float stepAngleSize = ViewAngle / stepCount;

        List<Vector3> ViewPoints = new List<Vector3>();

        ViewCastInfo OldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - ViewAngle / 2 + stepAngleSize * i;
           
            ViewCastInfo NewViewCast =  ViewCast(angle);

            if(i > 0)
            {
                bool EdgeDistThresholdExceeded = Mathf.Abs(OldViewCast.Dist - NewViewCast.Dist) > EdgeDistThreshold;
                if (OldViewCast.hit != NewViewCast.hit || (OldViewCast.hit && NewViewCast.hit && EdgeDistThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(OldViewCast, NewViewCast);
                    if(edge.PointA != Vector3.zero)
                    {
                        ViewPoints.Add(edge.PointA);
                    } 
                    if(edge.PointB != Vector3.zero)
                    {
                        ViewPoints.Add(edge.PointB);
                    }
                }
            }

            ViewPoints.Add(NewViewCast.point);
            OldViewCast = NewViewCast;
        }

        int vertexCount = ViewPoints.Count +1;
        Vector3[] Vertices = new Vector3[vertexCount];
        int[] Tris = new int[(vertexCount -2) * 3];

        Vertices[0] = Vector3.zero;

        for (int i = 1; i < vertexCount - 1; i++)
        {
            Vertices[i + 1] = transform.InverseTransformPoint(ViewPoints[i]);

            if(i < vertexCount - 2)
            {
                Tris[i * 3] = 0;
                Tris[i * 3 + 1] = i + 1;
                Tris[i * 3 + 2] = i + 2;
            }
        }

        Mesh.Clear();
        Mesh.vertices = Vertices;
        Mesh.triangles = Tris;
        Mesh.RecalculateNormals();
    } 

    EdgeInfo FindEdge(ViewCastInfo MinView, ViewCastInfo MaxView)
    {
        float minAngle = MinView.angle;
        float maxAngle = MaxView.angle;
        Vector3 MinPoint = Vector3.zero;
        Vector3 MaxPoint = Vector3.zero;

        for(int i = 0; i < EdgeResolve; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);
            bool EdgeDistThresholdExceeded = Mathf.Abs(MinView.Dist - newViewCast.Dist) > EdgeDistThreshold;
            if (newViewCast.hit == MinView.hit && !EdgeDistThresholdExceeded)
            {
                minAngle = angle;
                MinPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                MaxPoint = newViewCast.point;
            }

        }

        return new EdgeInfo(MinPoint, MaxPoint);
    }

    ViewCastInfo ViewCast(float GlobalAngle)
    {
        Vector3 Dir = DirFromAngle(GlobalAngle,true);
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Dir, out hit, viewRadius, ObjectsMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, GlobalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + Dir * viewRadius, viewRadius, GlobalAngle);

        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool AngleIsGlobal)
    {
        if (!AngleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float Dist;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _Dist, float _angle)
        {
            hit = _hit;
            point = _point;
            Dist = _Dist;  
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 PointA;
        public Vector3 PointB;

        public EdgeInfo (Vector3 _PointA, Vector3 _PointB)
        {
            PointA = _PointA;
            PointB = _PointB;
        }
    }
}