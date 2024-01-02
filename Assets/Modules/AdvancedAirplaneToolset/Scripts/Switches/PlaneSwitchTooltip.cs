using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaneSwitchTooltip : MonoBehaviour
{
    public SwitchBase parentSwitch;
    void Start()
    {
        transform.position += transform.up * 0.02f;
        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.text = parentSwitch.Desc;
        }
    }
}
