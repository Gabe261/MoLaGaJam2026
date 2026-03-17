using UnityEngine;

/// <summary>
/// Camera offset follow.
/// </summary>
public class OffsetFollow : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private Vector3 manuelOffset;
    
    private void Start()
    {
        if (manuelOffset == Vector3.zero)
        {
            Debug.LogWarning($"Warning: {name} OffsetFollow needs to be set.");
        }
    }

    private void LateUpdate()
    {
        transform.position = targetObject.position + manuelOffset;
    }
}
