using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class BuildSplineFromTransforms : MonoBehaviour
{
    private SplineContainer container;
    private GameObject knotsParent;

    private void OnEnable()
    {
        container = GetComponent<SplineContainer>();
    }
    
    public SplineContainer CreateSplineFromTransforms()
    {
        if (knotsParent == null)
        {
            Debug.LogWarning($"{name}: knotsParent is not assigned.");
            return null;
        }
        
        var spline = new Spline();

        foreach (Transform knot in knotsParent.GetComponentsInChildren<Transform>())
        {
            if(knot == knotsParent.transform ) continue;
            if (knot.childCount > 0) continue;
            
            spline.Add(new BezierKnot(this.transform.InverseTransformPoint(knot.position)));
        }

        container.Spline = spline;
        return container;
    }

    public void SetKnotsParent(GameObject knotsParent)
    {
        this.knotsParent = knotsParent;
    }
}
