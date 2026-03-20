using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class GameSequenceHandler : MonoBehaviour
{
    public UnityEvent OnGameEnable;
    
    public UnityEvent OnGameplayStart;

    [Header("Important World Positions")] 
    [SerializeField] private Transform gameStartPosition;
    
    [Header("Gameplay Prefabs")]
    [SerializeField] public GameObject PlayerPrefab;
    [SerializeField] public GameObject SplinePrefab;
    [SerializeField] public GameObject KnotsPrefab;
    
    [Header("OffsetFollowers")]
    [SerializeField] public List<OffSetFollows> FollowingObjects;
    [System.Serializable]
    public struct OffSetFollows
    {
        public OffsetFollow follower;
        public Vector3 offset;
    }

    private BuildSplineFromTransforms splineBuilder;
    private SplineContainer splineContainer;
    private PlayerSplineMovement playerSplineMovement;
    private SplineAnimate playerSplineAnimate;

    private void Awake()
    {
        OnGameEnable ??= new UnityEvent();
        OnGameplayStart ??= new UnityEvent();
    }
    
    private void Start()
    {
        OnGameEnable?.Invoke();
    }

    public void StartGame()
    {
        StartCoroutine(DelayedSequence());
    }
    
    private IEnumerator DelayedSequence()
    {
        yield return new WaitForSeconds(0.5f);
        SetupPlayerAndSpline();
        
        yield return new WaitForSeconds(0.5f);
        OnGameplayStart?.Invoke();
    }

    private void SetupPlayerAndSpline()
    {
        // Create Player :: At Vector3.zero
        GameObject player = Instantiate(PlayerPrefab);
        
        // Create Spline :: At Starting Point
        GameObject spline = Instantiate(SplinePrefab, gameStartPosition);
        
        // Create Knots :: At Starting Point
        GameObject knots = Instantiate(KnotsPrefab);
        
        // Get spline tool component and reference knots object.
        splineBuilder = spline.GetComponent<BuildSplineFromTransforms>();
        splineBuilder.SetKnotsParent(knots);
        splineContainer =  splineBuilder.CreateSplineFromTransforms();
        
        // Reference the spline to the players SplineAnimate
        playerSplineMovement = player.GetComponent<PlayerSplineMovement>();
        playerSplineAnimate = playerSplineMovement.GetPlayerSplineAnimator();
        playerSplineMovement.SetSplineContainer(splineContainer);
        
        OnGameplayStart.AddListener(playerSplineMovement.EnableMovement);
        
        // Setup Offset Follows
        foreach (OffSetFollows obj in FollowingObjects)
        {
            obj.follower.Initialize(player.transform, obj.offset);
        }
    }
}
