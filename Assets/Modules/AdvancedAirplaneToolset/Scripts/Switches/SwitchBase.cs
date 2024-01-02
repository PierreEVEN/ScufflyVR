using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple switch interface. handle all the required behaviour to implement many kind of button
/// </summary>
public abstract class SwitchBase : PlaneComponent
{
    /// <summary>
    /// Description of the button
    /// </summary>
    public string Desc = "";

    /// <summary>
    /// Event called when the button is pressed
    /// </summary>
    public abstract void Press(Vector3 initialPosition, Quaternion initialRotation);

    /// <summary>
    /// Event called when the button is released
    /// </summary>
    public abstract void Release();

    public GameObject hoverUi;
    private GameObject hoverInstancedUi;

    public virtual void SetControllerTransforms(Vector3 position, Quaternion rotation)
    {
    }

    /// <summary>
    /// Event called when the button is Overred by the mouse
    /// </summary>
    public void StartOver()
    {
        // set the object layer to 3 to enable outline post process effect
        gameObject.layer = 3;
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            child.gameObject.layer = 3;
    }

    public void StartSelect()
    {
        if (hoverInstancedUi)
            return;

        if (!hoverUi)
            return;

        hoverInstancedUi = Instantiate(hoverUi);
        hoverInstancedUi.transform.parent = transform.parent;
        hoverInstancedUi.transform.position = transform.position;
        hoverInstancedUi.transform.rotation = transform.rotation;
        hoverInstancedUi.GetComponent<PlaneSwitchTooltip>().parentSwitch = this;
    }

    /// <summary>
    /// Event called when the mouse stop over this button
    /// </summary>
    public void StopOver()
    {
        // Set the object layer back to 0
        gameObject.layer = 0;
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            child.gameObject.layer = 0;
    }

    public void StopSelect()
    {
        if (hoverInstancedUi)
        {
            Destroy(hoverInstancedUi);
            hoverInstancedUi = null;
        }
    }
}