using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LongClickButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler, IDragHandler,
    IBeginDragHandler,
    IEndDragHandler
{
    // Start is called before the first frame update
    private bool pointerDown = false;
    private float pointerDownTimer = 0,requireTimeHold = .5f;
    public UnityEvent<bool> onLongClick,onClick;
    private float dragX;
    private float dragY;
    [SerializeField]
    protected ScrollRect draggableRoot;
    public float maxDragThresholdX = 30;
    public float maxDragThresholdY = 50;
    [SerializeField]
    private bool isDragged;
    private bool currentDragFinished;
    public float maxClickThresholdX = 10;
    public float maxClickThresholdY = 10;
    [SerializeField]
    public Action<PointerEventData> rootOnBeginDrag, rootOnEndDrag;
    void Start()
    {
        draggableRoot = GetComponentInParent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer > requireTimeHold)
            {
                if (onLongClick != null&&!isDragged)
                {
                    onLongClick.Invoke(true);
                }
                Reset();
            }
            
        }
    }
    public void OnPointerDown(PointerEventData evenData)
    {
        pointerDown = true;
    }
    public void OnPointerUp(PointerEventData evenData)
    {
        if (pointerDown)
        {
            if (pointerDownTimer < requireTimeHold && !isDragged)
            {
                if (onClick != null)
                {
                    onClick.Invoke(true);
                }

            }
        }
            
        Reset();
    }
   
    void Reset()
    {
        pointerDownTimer = 0;
        pointerDown = false;
        isDragged = false;

    }


    public void OnDrag(PointerEventData eventData)
    {
        dragX = Mathf.Abs(eventData.pressPosition.x - eventData.position.x);
        dragY = Mathf.Abs(eventData.pressPosition.y - eventData.position.y);
        if (dragX < maxClickThresholdX && dragY < maxClickThresholdY)
        {
            isDragged = false;
        }
        else
        {
            isDragged = true;
        }
        if (!currentDragFinished && (dragX > maxDragThresholdX || dragY > maxDragThresholdY))
        {
            currentDragFinished = true;
            OnPointerUp(eventData);
        }
        if (draggableRoot != null && currentDragFinished)
        {
            draggableRoot.OnDrag(eventData);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (draggableRoot != null)
        {
            ((IBeginDragHandler)draggableRoot).OnBeginDrag(eventData);
        }
        rootOnBeginDrag?.Invoke(eventData);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggableRoot != null)
        {
            ((IEndDragHandler)draggableRoot).OnEndDrag(eventData);
        }
        if (rootOnEndDrag != null)
        {
            rootOnEndDrag(eventData);
        }
    }

}
