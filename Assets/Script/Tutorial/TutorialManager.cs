using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] TutorialList;
    public GameObject Container;
    private GameObject currentTutorial = null;
    private int currentStep = 0;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    public GameObject target;

    private LineRenderer lineRenderer;

    public void nextStep()
    {
        if (currentStep >= TutorialList.Length)
        {
            Destroy(gameObject);
            return;
        }

        List<GameObject> children = new List<GameObject>();
        Container.GetChildGameObjects(children);
        foreach (GameObject child in children)
            Destroy(child);

        if (currentTutorial)
            Destroy(currentTutorial);

        currentTutorial = Instantiate(TutorialList[currentStep]);
        currentTutorial.transform.parent = Container.transform;
        currentTutorial.GetComponent<TutorialElement>().manager = this;
        currentTutorial.transform.localScale = Vector3.one;
        currentTutorial.transform.position = transform.position;
        currentTutorial.transform.rotation = transform.rotation;
        currentStep++;
    }
    
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        nextStep();
        ComputeTargetPoint(out targetPosition, out targetRotation);
    }

    Bounds bounds = new Bounds(Vector3.zero, new Vector3(0.975f, 0.975f, 0.01f));

    bool isBlockedByTarget()
    {
        if (!target)
            return false;

        Vector3 start = transform.InverseTransformPoint(PlayerController.LocalPlayerController.GetHmd().transform.position);
        Vector3 direction = transform.InverseTransformDirection(target.transform.position - PlayerController.LocalPlayerController.GetHmd().transform.position);

        return bounds.IntersectRay(new Ray(start, direction));
    }
    public Vector3 FindClosestPointOnLine(Vector3 origin, Vector3 direction, Vector3 point)
    {
        direction.Normalize();
        Vector3 lhs = point - origin;

        float dotP = Vector3.Dot(lhs, direction);
        return origin + direction * dotP;
    }

    void ComputeTargetPoint(out Vector3 pos, out Quaternion rot)
    {
        if (!PlayerController.LocalPlayerController)
        {
            pos = transform.position;
            rot = transform.rotation;
            return;
        }

        GameObject HMD = PlayerController.LocalPlayerController.GetHmd();
        pos = HMD.transform.position + HMD.transform.forward * 0.35f - HMD.transform.up * 0.15f;

        if (isBlockedByTarget())
        {
            Vector3 rayPoint = FindClosestPointOnLine(HMD.transform.position,
                target.transform.position - HMD.transform.position, pos);
            Vector3 dir = Vector3.Normalize(pos - rayPoint);
            pos = rayPoint + dir * 0.3f;

        }
        rot = Quaternion.LookRotation(pos - HMD.transform.position, HMD.transform.up);
    }

    void Update()
    {
        ComputeTargetPoint(out var newPos, out var newRot);

        if (Quaternion.Angle(transform.rotation, newRot) > 45 || Vector3.Distance(transform.position, targetPosition) > 0.5f || isBlockedByTarget())
        {
            targetPosition = newPos;
            targetRotation = newRot;
        }


        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);

        if (target && lineRenderer)
        {
            Vector3 closestPoint = transform.TransformPoint(bounds.ClosestPoint(transform.InverseTransformPoint(target.transform.position)));
            Vector3[] positions = new Vector3[2];
            positions[0] = closestPoint;
            positions[1] = target.transform.position;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
            lineRenderer.SetPositions(positions);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
}
