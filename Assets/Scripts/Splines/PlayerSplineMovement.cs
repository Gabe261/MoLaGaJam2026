using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerSplineMovement : MonoBehaviour
{
    private SplineAnimate playerSplineAnimator;
    private bool isInitialized;
    private bool movementEnabled;

    [Header("Speed")]
    [SerializeField] private float maxSpeed = 1.0f;          // max seconds-per-second added to ElapsedTime
    [SerializeField] private float decelRate = 6.0f;         // how fast speed falls back to 0 when released
    [SerializeField] private float directionChangeReset = 0.15f; // threshold for "changed direction"

    [Header("Acceleration Curve")]
    [SerializeField] private float timeToFullSpeed = 0.6f;   // curve input duration (seconds) to reach full
    [SerializeField] private AnimationCurve accelCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);            // x: normalized time (0..1), y: multiplier (0..1)

    private float moveAxis;      // target input (-1..1)
    private float heldTime;      // how long current direction has been held
    private float currentSpeed;  // signed speed applied each frame

    private void Initialize()
    {
        playerSplineAnimator = GetComponent<SplineAnimate>();
        if(playerSplineAnimator == null) Debug.LogError("PlayerSplineMovement: PlayerSplineAnimator is null");

        movementEnabled = false;
        isInitialized = true;
    }

    public SplineAnimate GetPlayerSplineAnimator()
    {
        if (!isInitialized) { Initialize(); }
        return playerSplineAnimator;
    }

    public void SetSplineContainer(SplineContainer container)
    {
        if (!isInitialized) { Initialize(); }
        playerSplineAnimator.Container = container;
    }
    
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveAxis = ctx.ReadValue<float>(); // -1, 0, 1 (or analog)
    }

    public void EnableMovement()
    {
        movementEnabled = true;
    }

    public void DisableMovement()
    {
        movementEnabled = false;
    }
    
    private void Update()
    {
        if(!movementEnabled) return;
        
        float dt = Time.deltaTime;
        
        // No input: decelerate to 0 and reset held time
        if (Mathf.Abs(moveAxis) < 0.01f)
        {
            heldTime = 0f;
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, decelRate * dt);
        }
        else
        {
            // If player flips direction, reset ramp so it doesn't instantly hit max
            if (Mathf.Sign(moveAxis) != Mathf.Sign(currentSpeed) && Mathf.Abs(currentSpeed) > directionChangeReset)
            {
                heldTime = 0f;
                currentSpeed = 0f;
            }

            heldTime += dt;

            // Normalize held time to 0..1 for the curve
            float t = Mathf.Clamp01(heldTime / Mathf.Max(0.0001f, timeToFullSpeed));

            // Curve returns 0..1 multiplier
            float accelMultiplier = Mathf.Clamp01(accelCurve.Evaluate(t));

            // Desired signed speed based on curve
            float targetSpeed = Mathf.Sign(moveAxis) * (maxSpeed * accelMultiplier);

            // Smoothly move currentSpeed toward targetSpeed (feel free to tweak)
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (maxSpeed * 10f) * dt);
        }

        // Apply to your spline time
        playerSplineAnimator.ElapsedTime += currentSpeed * dt;
    }
    
}
