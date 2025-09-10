using UnityEditor;
using UnityEngine;


[CustomEditor(typeof (Sight))]
public class SightEditor : Editor
{
    
    private void OnSceneGUI()
    {
        Sight sight = (Sight)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(sight.transform.position, Vector3.up, Vector3.forward, 360, sight.viewRadius);
        Vector3 viewAngleA = sight.DirFromAngle(-sight.ViewAngle / 2, false);
        Vector3 viewAngleB = sight.DirFromAngle(sight.ViewAngle / 2, false);

        Handles.DrawLine(sight.transform.position, sight.transform.position + viewAngleA * sight.viewRadius);
        Handles.DrawLine(sight.transform.position, sight.transform.position + viewAngleB * sight.viewRadius);

        foreach (Transform visbleTarget in sight.VisibleTargets)
        {
            Handles.DrawLine(sight.transform.position, visbleTarget.position);
        }
    
    }
    
}
