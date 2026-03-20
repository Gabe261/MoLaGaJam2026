using UnityEngine;

/// <summary>
/// Camera offset follow.
/// </summary>
public class OffsetFollow : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private Vector3 offset;
    
    private bool isInitialized;
    
    public void Initialize(Transform target, Vector3 offset)
    {
        targetObject = target;
        this.offset = offset;
        isInitialized = true;
    }

    private void LateUpdate()
    {
        if (!isInitialized)
        {
            return;
        }
        transform.position = targetObject.position + offset;
    }
}
