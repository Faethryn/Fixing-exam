using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;



public class CardEventArgs : EventArgs
{
	public Card Card;

	public CardEventArgs(Card card)
    {
		Card = card;
    }


}
public class Card : MonoBehaviour,ICard, IBeginDragHandler, IDragHandler, IEndDragHandler {
	
	public Transform parentToReturnTo = null;
	public Transform placeholderParent = null;

	GameObject placeholder = null;

	public event EventHandler<CardEventArgs> BeginCardDrag;
	public event EventHandler<CardEventArgs> EndCardDrag;
	public event EventHandler<CardEventArgs> IsDropped;
	public event EventHandler<CardEventArgs> Clicked;
	public event EventHandler<CardEventArgs> Dragging;




	[SerializeField]
	private CardType _cardType;

    bool ICard.Played { get; }

	CardType ICard.cardType => _cardType;

    private void Start()
    {
        
    }


    public void OnBeginDrag(PointerEventData eventData) {
		Debug.Log ("OnBeginDrag");
		
		placeholder = new GameObject();
		placeholder.transform.SetParent( this.transform.parent );
		LayoutElement le = placeholder.AddComponent<LayoutElement>();
		le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
		le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
		le.flexibleWidth = 0;
		le.flexibleHeight = 0;

		placeholder.transform.SetSiblingIndex( this.transform.GetSiblingIndex() );
		
		parentToReturnTo = this.transform.parent;
		placeholderParent = parentToReturnTo;
		this.transform.SetParent( this.transform.parent.parent );
		
		GetComponent<CanvasGroup>().blocksRaycasts = false;

		OnBeginDragging(this, new CardEventArgs(this));


	}
	
	public void OnDrag(PointerEventData eventData) {
		//Debug.Log ("OnDrag");
		
		this.transform.position = eventData.position;

		if(placeholder.transform.parent != placeholderParent)
			placeholder.transform.SetParent(placeholderParent);

		int newSiblingIndex = placeholderParent.childCount;

		for(int i=0; i < placeholderParent.childCount; i++) {
			if(this.transform.position.x < placeholderParent.GetChild(i).position.x) {

				newSiblingIndex = i;

				if(placeholder.transform.GetSiblingIndex() < newSiblingIndex)
					newSiblingIndex--;

				break;
			}
		}

		placeholder.transform.SetSiblingIndex(newSiblingIndex);

	}
	
	public void OnEndDrag(PointerEventData eventData) {
		Debug.Log ("OnEndDrag");
		this.transform.SetParent( parentToReturnTo );
		this.transform.SetSiblingIndex( placeholder.transform.GetSiblingIndex() );
		GetComponent<CanvasGroup>().blocksRaycasts = true;

		Destroy(placeholder);
	}

    public void Used()
    {
        Destroy(this.gameObject);
		Destroy(placeholder);
    }


	protected virtual void OnBeginDragging(object source, CardEventArgs e)
    {
        var handler = BeginCardDrag;
		handler?.Invoke(this, e);
    }
	protected virtual void OnEndDragging(object source, CardEventArgs e)
	{
		var handler = EndCardDrag;
		handler?.Invoke(this, e);
	}

}
