using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialElement : MonoBehaviour
{
    public TutorialManager manager;
    protected void Next()
    {
        if (manager)
            manager.nextStep();
    }

    protected void SetTarget(GameObject target)
    {
        manager.target = target;
    }
}
