using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLogics : MonoBehaviour {

    private int setCounter;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int checkForSets(List<GameObject> setCard)
    {
        List<Card> checkCardList = new List<Card>();
        for (int i = 0; i < setCard.Count; i++)
        {
            checkCardList.Add(setCard[i].GetComponent<Card>());
        }

        setCounter = 0;
        foreach (Card child in checkCardList)
        {
            CardData cardData1 = child.cardData;
            CardData cardData2;
            CardData cardData3;
            foreach (Card child2 in checkCardList)
            {
                cardData2 = child2.cardData;

                if (child == child2)
                {
                    break;
                }

                foreach (Card child3 in checkCardList)
                {
                    cardData3 = child3.cardData;
                    if (child2 == child3 || child == child3)
                    {
                        break;
                    }
                    bool fillSame = cardData1.fill == cardData2.fill && cardData1.fill == cardData3.fill;
                    bool colorSame = cardData1.color == cardData2.color && cardData1.color == cardData3.color;
                    bool shapeSame = cardData1.cardShape == cardData2.cardShape && cardData1.cardShape == cardData3.cardShape;
                    bool numberSame = cardData1.number == cardData2.number && cardData1.number == cardData3.number;

                    bool fillDiff = cardData1.fill != cardData2.fill && cardData1.fill != cardData3.fill && cardData2.fill != cardData3.fill;
                    bool colorDiff = cardData1.color != cardData2.color && cardData1.color != cardData3.color && cardData2.color != cardData3.color;
                    bool shapeDiff = cardData1.cardShape != cardData2.cardShape && cardData1.cardShape != cardData3.cardShape && cardData2.cardShape != cardData3.cardShape;
                    bool numberDiff = cardData1.number != cardData2.number && cardData1.number != cardData3.number && cardData2.number != cardData3.number;

                    if ((fillSame || fillDiff) && (colorSame || colorDiff) && (shapeSame || shapeDiff) && (numberSame || numberDiff))
                    {
                        setCounter++;
                    }
                }
            }
        }
        return setCounter;
    }

    public List<GameObject> getSet(List<GameObject> setCard,int cardNumShow)
    {
        List<Card> checkCardList = new List<Card>();
        List<GameObject> set= new List<GameObject>();
        for (int i = 0; i < setCard.Count; i++)
        {
            checkCardList.Add(setCard[i].GetComponent<Card>());
        }

        setCounter = 0;
        foreach (Card child in checkCardList)
        {
            CardData cardData1 = child.cardData;
            CardData cardData2;
            CardData cardData3;
            foreach (Card child2 in checkCardList)
            {
                cardData2 = child2.cardData;

                if (child == child2)
                {
                    break;
                }

                foreach (Card child3 in checkCardList)
                {
                    cardData3 = child3.cardData;
                    if (child2 == child3 || child == child3)
                    {
                        break;
                    }
                    bool fillSame = cardData1.fill == cardData2.fill && cardData1.fill == cardData3.fill;
                    bool colorSame = cardData1.color == cardData2.color && cardData1.color == cardData3.color;
                    bool shapeSame = cardData1.cardShape == cardData2.cardShape && cardData1.cardShape == cardData3.cardShape;
                    bool numberSame = cardData1.number == cardData2.number && cardData1.number == cardData3.number;

                    bool fillDiff = cardData1.fill != cardData2.fill && cardData1.fill != cardData3.fill && cardData2.fill != cardData3.fill;
                    bool colorDiff = cardData1.color != cardData2.color && cardData1.color != cardData3.color && cardData2.color != cardData3.color;
                    bool shapeDiff = cardData1.cardShape != cardData2.cardShape && cardData1.cardShape != cardData3.cardShape && cardData2.cardShape != cardData3.cardShape;
                    bool numberDiff = cardData1.number != cardData2.number && cardData1.number != cardData3.number && cardData2.number != cardData3.number;

                    if ((fillSame || fillDiff) && (colorSame || colorDiff) && (shapeSame || shapeDiff) && (numberSame || numberDiff))
                    {


                        switch (cardNumShow)
                        {
                            case 1:
                                set.Add(child.gameObject);

                                break;

                            case 2:
                                set.Add(child.gameObject);
                                set.Add(child2.gameObject);
                                break;

                            case 3:
                                set.Add(child.gameObject);
                                set.Add(child2.gameObject);
                                set.Add(child3.gameObject);
                                break;
                        }

                        Debug.Log("Found Set");
                        return set;
                    }
                }
            }
        }
        return null;
    }

}
