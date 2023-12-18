using UnityEditor;
using UnityEngine;

public enum RotatingSwitchStates
{
    None,
    FloodLights,
    HudLevel,
    RightScreen,
    LeftScreen,
    AntiColLights,
}

/// <summary>
/// A simple spinning button
/// </summary>
public class RotatingSwitch : SwitchBase
{
    /// <summary>
    /// The modified plane property
    /// </summary>
    public RotatingSwitchStates modifiedProperty = RotatingSwitchStates.None;

    /// <summary>
    /// Current value
    /// </summary>
    float value = 0;

    /// <summary>
    /// Is user currently holding the click to make the button spin
    /// </summary>
    bool isMoving = false;

    /// <summary>
    /// The value of the button before the user started to make it spin
    /// </summary>
    float initialValue;

    /// <summary>
    /// The max rotation
    /// </summary>
    public float range = 300;

    private float totalSlideRotation = 0;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Vector3 vectorLocalTransform(Ray vec)
    {
        // In the case where the controller is in the same axe than the button, return the controller position projected instead of the ray projected position.
        if (Mathf.Abs(Vector3.Dot(vec.direction, transform.up)) < 0.5f)
        {
            Vector3 localVector = transform.InverseTransformPoint(vec.origin);
            localVector.y = 0;
            return localVector;
        }

        return transform.InverseTransformPoint(
            (new Plane(transform.up, transform.position)).Raycast(vec, out float distance)
                ? vec.GetPoint(distance)
                : vec.origin);
    }


    public override void SetControllerTransforms(Vector3 position, Quaternion rotation)
    {
        if (!isMoving)
            return;

        Quaternion rotationDelta = rotation * Quaternion.Inverse(initialRotation);
        rotationDelta.ToAngleAxis(out float angle, out Vector3 axis);
        float offsetThroughRotation = Vector3.Dot(transform.up, axis) > 0
            ? initialValue + angle / 360 * (360 / range) * 2
            : initialValue - angle / 360 * (360 / range) * 2;

        Debug.DrawLine(transform.position,
            transform.TransformPoint(vectorLocalTransform(new Ray(position, rotation * Vector3.forward))));

        totalSlideRotation += Vector3.SignedAngle(
            vectorLocalTransform(new Ray(initialPosition, rotation * Vector3.forward)),
            vectorLocalTransform(new Ray(position, rotation * Vector3.forward)), transform.up);
        initialPosition = position;
        float offsetThroughSlide = initialValue + totalSlideRotation / 360 * Mathf.Pow(360 / range, 2);

        // Offset through controller rotation is disabled because it's not stable enough yet
        value = Mathf.Clamp(
            Mathf.Abs(offsetThroughRotation) > Mathf.Abs(offsetThroughSlide) && false
                ? offsetThroughRotation
                : offsetThroughSlide, 0, 1);

        // The update the property
        switch (modifiedProperty)
        {
            case RotatingSwitchStates.None:
                break;
            case RotatingSwitchStates.FloodLights:
                Plane.CockpitFloodLights = value;
                break;
            case RotatingSwitchStates.HudLevel:
                Plane.HudLightLevel = value;
                break;
            case RotatingSwitchStates.RightScreen:
                break;
            case RotatingSwitchStates.LeftScreen:
                break;
            case RotatingSwitchStates.AntiColLights:
                Plane.PositionLight = value;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            // make the rotation of the button match the real plane value
            switch (modifiedProperty)
            {
                case RotatingSwitchStates.None:
                    break;
                case RotatingSwitchStates.FloodLights:
                    value = Plane.CockpitFloodLights;
                    break;
                case RotatingSwitchStates.HudLevel:
                    value = Plane.HudLightLevel;
                    break;
                case RotatingSwitchStates.RightScreen:
                    break;
                case RotatingSwitchStates.LeftScreen:
                    break;
                case RotatingSwitchStates.AntiColLights:
                    value = Plane.PositionLight;
                    break;
            }
        }

        // Update mesh rotation
        for (int i = 0; i < transform.childCount; ++i)
            transform.GetChild(i).localRotation = Quaternion.Euler(0, value * range, 0);
    }

    public override void Press(Vector3 inInitialPosition, Quaternion inInitialRotation)
    {
        initialPosition = inInitialPosition;
        initialRotation = inInitialRotation;
        // We are starting draging the button
        isMoving = true;
        initialValue = value;
        totalSlideRotation = 0;
    }

    public override void Release()
    {
        // We released the button
        isMoving = false;
    }
}