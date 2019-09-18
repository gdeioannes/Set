using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public int cardShape;
    public int fill;
    public int color;
    public int number;

    public CardData(int cardShape, int fill, int color, int number)
    {
        this.cardShape = cardShape;
        this.fill = fill;
        this.color = color;
        this.number = number;
    }
}