using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    #region SO Events
    [SerializeField] private GameEvent _OnPointerUp;
    [SerializeField] private GameEvent _OnPointerDown;
    [SerializeField] private GameEvent _OnPointerDrag;
    [SerializeField] private GameEvent _OnPlayerManualReload;
    #endregion

    public static Vector2 up;
    public static Vector2 direction;
    private Vector2 clickPosition;

    private bool upVectorSet = false;
    private float clickTime;
    private float reloadTimeThreshold = 0.5f;
    private float reloadPositionThreshold = 10f;

    public void OnDrag(PointerEventData eventData)
    {
        _OnPointerDrag.Invoke();
        direction = (eventData.position - clickPosition).normalized;

        /*float horizontal = SimpleInput.GetAxis("Horizontal");
        float vertical = SimpleInput.GetAxis("Vertical");
        direction = new Vector3(horizontal, vertical).normalized;

        if (direction != Vector2.zero && !upVectorSet)
        {
            upVectorSet = true;
            up = Quaternion.Euler(new Vector3(horizontal, vertical).normalized) * Vector2.up;
            Debug.Log(up);
        }*/
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _OnPointerUp.Invoke();

        if (Time.time - clickTime < reloadTimeThreshold && (eventData.position - clickPosition).sqrMagnitude < Mathf.Pow(reloadPositionThreshold, 2))
            _OnPlayerManualReload.Invoke();

        direction = Vector2.zero;
        clickPosition = Vector2.zero;
        
        //upVectorSet = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _OnPointerDown.Invoke();
        
        clickTime = Time.time;
        clickPosition = eventData.position;

        
    }
}
