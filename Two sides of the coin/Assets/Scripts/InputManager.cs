using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera SceneCamera;
    [SerializeField] private LayerMask layer; 
    private Vector3 lastPosition;

    public event Action OnCLicked, OnExit;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnCLicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    public bool IsPointingUI() => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectMapPosition()
    {
        Vector3 mousepos = Input.mousePosition;
        Ray ray = SceneCamera.ScreenPointToRay(mousepos);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 1000f, layer))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}