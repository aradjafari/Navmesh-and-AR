using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObject : MonoBehaviour
{
    [SerializeField] private GameObject AgentPlane;
    [SerializeField] private ARRaycastManager m_RaycastManager;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject target;

    [SerializeField] Text BtnText;
    [SerializeField] Image selectTargetButton;

    public NavMeshSurface surface;

    bool planesAreShowing = true;
    bool targetIsSelected = false;

    ObjectsTypes ot = ObjectsTypes.Agent;

    enum ObjectsTypes
    {
        Obsticle,
        Agent,
        Target,
        None
    }

    private void Start()
    {
        ot = ObjectsTypes.None;
    }

    public void ChangeObjectTypeForPlacing(string value)
    {
        switch (value)
        {
            case "Obsticle":
                targetIsSelected = false;
                ot = ObjectsTypes.Obsticle;
                break;
            case "Target":
                if(!targetIsSelected)
                    targetIsSelected = true;
                else
                    targetIsSelected = false;
                break;
            default:
                targetIsSelected = false;
                ot = ObjectsTypes.None;
                break;
        }
        TurnButtonColor();

    }

    private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Update()
    {
        if(Input.touchCount > 0 )
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !IsPointerOverUIObject())
            {
                if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPos = s_Hits[0].pose;

                    if (targetIsSelected)
                    {
                        AgentPlane.transform.position = hitPos.position;
                        target.transform.position = hitPos.position;
                        surface.BuildNavMesh();
                        GameObject.FindObjectOfType<Agent>().GetComponent<Agent>().SetRePosedAtStart(true);
                    }
                    else if(ot == ObjectsTypes.Obsticle) 
                    {
                        Instantiate(obstacle, hitPos.position, hitPos.rotation);
                    }
                }
            }
        }
    }

    public void TurnButtonColor()
    {  
        if (targetIsSelected)
            selectTargetButton.color = Color.green;
        else
            selectTargetButton.color = Color.white;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.GetTouch(0).position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void TogglePlanesVisibilities()
    {

        ARPlaneManager ARPM = GetComponent<ARPlaneManager>();
        if (planesAreShowing)
        {
            planesAreShowing = false;
            foreach (ARPlane plane in ARPM.trackables)
            {
                plane.gameObject.SetActive(false);
                BtnText.text = "Show AR Planes";
            }
            
        }
        else
        {
            planesAreShowing = true;
            foreach (ARPlane plane in ARPM.trackables)
            {
                plane.gameObject.SetActive(true);
                BtnText.text = "Hide AR Planes";
            }
        }
    }
}
