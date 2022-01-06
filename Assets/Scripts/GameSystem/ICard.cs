using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard 
{
   bool Played { get; }

    CardType cardType { get; }

    public void Used();
}
