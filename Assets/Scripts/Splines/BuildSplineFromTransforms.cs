using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class BuildSplineFromTransforms : MonoBehaviour
{
    private SplineContainer container;
    [SerializeField] private GameObject knotsParent;

    private void Awake()
    {
        container = GetComponent<SplineContainer>();
        if (knotsParent == null) { Debug.LogWarning($"{name}: knotsParent is not assigned."); }
    }
    
    public void CreateSplineFromTransforms()
    {
        if (knotsParent == null)
        {
            Debug.LogWarning($"{name}: knotsParent is not assigned.");
            return;
        }
        
        var spline = new Spline();

        foreach (Transform knot in knotsParent.GetComponentsInChildren<Transform>())
        {
            if(knot == knotsParent.transform ) continue;
            if (knot.childCount > 0) continue;
            
            spline.Add(new BezierKnot(this.transform.InverseTransformPoint(knot.position)));
        }

        container.Spline = spline;
    }
}
