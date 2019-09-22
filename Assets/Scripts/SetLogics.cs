using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SetLogics : MonoBehaviour {

    public GameObject cardContainer;
    private int setCounter;
    protected List<GameObject> selectedCards = new List<GameObject>();
    public List<int> cardPick = new List<int>();
    public List<CardData> cardDeck = new List<CardData>();
    protected int numSets;
    public Text finishText;
    public GameObject finishUI;
    public Text bestTimeText;
    protected int sg;
    private int penaltieForSet = 30;

    public Text numbertOfSetsInPlayText;
    public Text cardCountText;
    public Text numberOfSetsText;
    public Text timeText;

    public string gameModeBestTimeKey = "";

    private int bestTime;

    private bool deleteAll = false;

    void Awake()
    {
        Time.timeScale = 1;
        if (deleteAll) {
            Debug.Log("Delete ALL");
        PlayerPrefs.DeleteAll();
        }
    }

    protected void checkBestTime()
    {
        if (PlayerPrefs.HasKey(gameModeBestTimeKey))
        {
            bestTime = PlayerPrefs.GetInt(gameModeBestTimeKey);
        }
        else
        {
            bestTime = -1;
        }

        changeBestTime();
    }

    public void changeText(int numbertOfSetsInPlay)
    {
        numbertOfSetsInPlayText.text = "N S D:" + numbertOfSetsInPlay;
        cardCountText.text = "N Cards:" + cardPick.Count;
        numberOfSetsText.text = "N Sets:" + numSets;
    }

    public IEnumerator startTime()
    {
        while (true)
        {
            sg++;
            changeTime();
            yield return new WaitForSeconds(1);
        }
    }

    private void changeBestTime()
    {

        if (bestTime > 0)
        {
            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(bestTime);
            string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            bestTimeText.text = "" + timeSpan;
        }
        else
        {
            bestTimeText.text = "No best Time yet";
        }
    }

    void Update()
    {
        //If the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
                selectCards(hit);
            }
        }
    }


    public void showSet(int setNum)
    {
        List<GameObject> set = getSet(getPlayDeck(), setNum);

        foreach (GameObject item in set)
        {
            item.GetComponent<Card>().changeColor();
        }
        calculateScore(setNum);
    }

    public abstract void assignNewCard(GameObject cardObj);

    protected void createDeck()
    {
        int colorNumMax = 3;
        int shapeNumMax = 3;
        int cardFillMax = 3;
        int numMax = 3;
        int numberOfCards = colorNumMax * shapeNumMax * cardFillMax * numMax;
        //related to the number of possible combination of each simbol
        for (int i = 0; i < numberOfCards; i++)
        {
            cardPick.Add(i);
        }

        for (int colorNum = 0; colorNum < colorNumMax; colorNum++)
        {
            for (int shapeNum = 0; shapeNum < shapeNumMax; shapeNum++)
            {
                for (int cardFill = 0; cardFill < cardFillMax; cardFill++)
                {
                    for (int num = 0; num < numMax; num++)
                    {
                        cardDeck.Add(new CardData(shapeNum, cardFill, colorNum, num));
                    }
                }
            }
        }

        List<GameObject> existingDeck = new List<GameObject>();
        foreach (Transform child in cardContainer.transform)
        {
            existingDeck.Add(child.gameObject);
        }

        addCards(existingDeck);
    }

    private void addCards(List<GameObject> cards)
    {
        //Add Cards acording to deck in a random way
        for (int i = 0; i < cards.Count; i++)
        {
            changeCard(cards[i]);
        }
    }

    public List<GameObject> getPlayDeck()
    {
        List<GameObject> listOfCards = new List<GameObject>();
        foreach (Transform child in cardContainer.transform)
        {
            if (child.gameObject.activeSelf)
            {
                listOfCards.Add(child.gameObject);
            }
        }

        return listOfCards;
    }

    public int checkDeckSets()
    {
        return checkForSets(getPlayDeck());
    }

    public void changeCard(GameObject cardObj)
    {
        if (cardPick.Count >= 1)
        {
            //Pick a Card
            if (cardObj.activeSelf)
            {
                int cardDeckNumber = cardPick[Random.Range(0, cardPick.Count)];
                //Remove the card number from the deck
                cardPick.Remove(cardDeckNumber);

                cardObj.GetComponent<Card>().selected = false;
                cardObj.GetComponent<Card>().togglePaper();
                cardObj.GetComponent<Card>().id = cardDeckNumber;
                cardObj.GetComponent<Card>().changeCardData(cardDeck[cardDeckNumber]);
            }
        }
        else
        {
            //If there is no more cards in the deck hide the cards
            cardObj.GetComponent<Card>().selected = false;
            cardObj.GetComponent<Card>().togglePaper();
            cardObj.SetActive(false);
        }
    }

    private void selectCards(RaycastHit2D hit)
    {
        if (hit.collider.gameObject.GetComponent<Card>() is Card)
        {

            GameObject hitCard = hit.collider.gameObject;

            if (!hitCard.GetComponent<Card>().selected)
            {
                if (selectedCards.Count < 3)
                {
                    hitCard.GetComponent<Card>().selected = true;
                    hitCard.GetComponent<Card>().togglePaper();
                    selectedCards.Add(hitCard);

                }
                else
                {
                    selectedCards[0].GetComponent<Card>().selected = false;
                    selectedCards[0].GetComponent<Card>().togglePaper();
                    selectedCards.Remove(selectedCards[0]);
                    hit.collider.gameObject.GetComponent<Card>().selected = true;
                    hitCard.GetComponent<Card>().togglePaper();
                    selectedCards.Add(hit.collider.gameObject);
                }
                SoundManager._instance.playPickCard();
            }
            else
            {
                hitCard.GetComponent<Card>().selected = false;
                hitCard.GetComponent<Card>().togglePaper();
                selectedCards.Remove(hitCard);
                SoundManager._instance.playLeaveCard();
            }
        }
        if (selectedCards.Count == 3)
        {
			Debug.Log("Selected 3");
            //Check Selected Sets
            if (checkForSets(selectedCards) == 1)
            {
                numSets++;

                //Animate and add new cards
                extraFunctionality();
                SoundManager._instance.playSet();
                selectedCards.Clear();
            }
            else
            {
                SoundManager._instance.playNoSet();
            }
        }
    }

    public abstract void extraFunctionality();

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
            if (!child.gameObject.activeSelf)
            {
                break;
            }
            CardData cardData1 = child.cardData;
            CardData cardData2;
            CardData cardData3;
            foreach (Card child2 in checkCardList)
            {
                if (!child2.gameObject.activeSelf)
                {
                    break;
                }

                cardData2 = child2.cardData;

                if (child == child2)
                {
                    break;
                }

                foreach (Card child3 in checkCardList)
                {
                    if (!child3.gameObject.activeSelf)
                    {
                        break;
                    }
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

    public void checkGameState(int numbertOfSetsInPlay)
    {


        if (cardPick.Count == 0 && numbertOfSetsInPlay == 0)
        {

            if (sg < bestTime || bestTime < 0)
            {
                finishText.text = "BEST TIME ACHIEVE!";
                PlayerPrefs.SetInt(gameModeBestTimeKey, sg);
                PlayerPrefs.Save();
            }
            else
            {
                finishText.text = "No more Cards! \n Play another?";
            }


            finishUI.SetActive(true);
        }
        else
        {
            if (numbertOfSetsInPlay == 0)
            {
                extraFunctionalitytwo();
                Debug.Log("No Sets!");
            }
        }
    }

    protected void changeTime()
    {

        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(sg);
        string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        this.timeText.text = "" + timeSpan;
    }

    //Override to add time penalties
    public void calculateScore(int setNum)
    {
        sg += setNum * penaltieForSet;
        changeTime();
    }

    public abstract void extraFunctionalitytwo();

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
            if (!child.gameObject.activeSelf)
            {
                break;
            }

            CardData cardData1 = child.cardData;
            CardData cardData2;
            CardData cardData3;
            foreach (Card child2 in checkCardList)
            {
                if (!child2.gameObject.activeSelf)
                {
                    break;
                }

                cardData2 = child2.cardData;

                if (child == child2)
                {
                    break;
                }

                foreach (Card child3 in checkCardList)
                {
                    if (!child3.gameObject.activeSelf)
                    {
                        break;
                    }
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
