using DAE.GameSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PositionEventArgs : EventArgs
{
    public Tile Position { get; }

    public PositionEventArgs(Tile position)
    {
        Position = position;
    }
}
public class Tile : MonoBehaviour ,IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int Q;
    public int R;
    public int S;
    [SerializeField]
    private UnityEvent OnActivate;

    [SerializeField]
    private UnityEvent OnDeactivate;

    public event EventHandler<PositionEventArgs> DroppedOn;
    public event EventHandler<PositionEventArgs>Exited;
    public event EventHandler<PositionEventArgs> Entered;

    [SerializeField]
    private GameLoop _loop;

    private void Start()
    {
        
    }
    private PositionModel _model;

    public PositionModel Model
    {
        set
        {
            if (_model != null)
            {
                _model.Activated -= PositionActivated;
                _model.Deactivated -= PositionDeactivated;
            }

            _model = value;

            if (_model != null)
            {
                _model.Activated += PositionActivated;
                _model.Deactivated += PositionDeactivated;
            }

        }
        get
        {
            return _model;
        }
    }

    public void PositionDeactivated(object sender, EventArgs e)
        => OnDeactivate.Invoke();

    public void PositionActivated(object sender, EventArgs e)
        => OnActivate.Invoke();


    protected virtual void OnEntering(object source,PositionEventArgs e)
    {
        var handler = Entered;
        handler?.Invoke(this, e);
    }

    protected virtual void OnExiting(object source, PositionEventArgs e)
    {
        var handler = Exited;
        handler?.Invoke(this, e);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
        if (eventData.pointerDrag == null)
            return;

        //Card d = eventData.pointerDrag.GetComponent<Card>();
        //if (d != null)
        //{
        //    d.placeholderParent = this.transform;
        //}
        OnEntering(this, new PositionEventArgs(this));

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit");
        if (eventData.pointerDrag == null)
            return;

        //Card d = eventData.pointerDrag.GetComponent<Card>();
        //if (d != null && d.placeholderParent == this.transform)
        //{
        //    d.placeholderParent = d.parentToReturnTo;
        //}
        OnExiting(this, new PositionEventArgs(this));

    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        Card d = eventData.pointerDrag.GetComponent<Card>();
        if (d != null)
        {
            var handler = DroppedOn;
            handler?.Invoke(this, new PositionEventArgs( this));
        }

    }

}
