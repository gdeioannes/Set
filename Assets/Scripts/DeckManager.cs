using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour {

    public static DeckManager _instance;
    public GameObject cardContainer;
    public GameObject finishUI;
    public List<CardData> cardDeck = new List<CardData>();
    public List<int> cardPick = new List<int>();
    private List<GameObject> selectedCards = new List<GameObject>();
    public List<GameObject> extraCards;
    private int selectNumChange;

    public Text tex1;
    public Text tex2;
    public Text tex3;
    public Text tex4;
    public Text finishText;
    public Text bestTimeText;

    private int bestTime = 0;
    private bool extraCardFlag=false;
    private int numSets;
    private int sg;
    private int penaltieForSet = 30;

    private SetLogics setLogics = new SetLogics();

    // Use this for initialization
    void Start() {
        //Assing Number of cards in the deck this number is
        createDeck();
        if (_instance == null)
        {
            _instance = this;
        }
        finishUI.SetActive(false);
        checkDeckSets();
        StartCoroutine(startTime());
        changeText(checkDeckSets());
        setExtraCards();

        if (PlayerPrefs.HasKey("bestTime"))
        {
            bestTime = PlayerPrefs.GetInt("bestTime");
        }
        else
        {
            bestTime = -1;
        }

        changeBestTime();
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

    IEnumerator startTime()
    {
        while (true) {
            sg++;
            changeTime();


            yield return new WaitForSeconds(1);
        }
    }

    private void changeTime() {

        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(sg);
        string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        tex4.text = "" + timeSpan;
    }

    private void changeBestTime()
    {

        if (bestTime > 0) { 
            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(bestTime);
        string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        
        bestTimeText.text =""+ timeSpan;
        }
        else
        {
            bestTimeText.text = "No best Time yet";
        }
    }

    private void createDeck()
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
                cardObj.GetComponent<Card>().id= cardDeckNumber;
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
        return setLogics.checkForSets(getPlayDeck());
    }

    public void changeText(int numbertOfSetsInPlay)
    {
        tex1.text = "N S D:" + numbertOfSetsInPlay;
        tex2.text = "N Cards:" + cardPick.Count;
        tex3.text = "N Sets:" + numSets;
    }

    public void checkGameState(int numbertOfSetsInPlay)
    {
        

        if (cardPick.Count == 0 && numbertOfSetsInPlay == 0)
        {

            if (sg < bestTime || bestTime<0)
            {
                finishText.text="BEST TIME ACHIEVE!";
                PlayerPrefs.SetInt("bestTime",sg);
                PlayerPrefs.Save();
            }
            else
            {
                finishText.text="No more Cards! \n Play another?";
            }


            finishUI.SetActive(true);
        }
        else
        {
            if (numbertOfSetsInPlay == 0)
            {
                showExtraCard();
                Debug.Log("No Sets!");
            }
        }
    }

    private void selectCards(RaycastHit2D hit)
    {   
        if (hit.collider.gameObject.GetComponent<Card>() is Card)
        {

            GameObject hitCard = hit.collider.gameObject;

            if (!hitCard.GetComponent<Card>().selected)
            {
                if (selectedCards.Count < 3) {
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

        if (selectedCards.Count == 3) {

            //Check Selected Sets
            if (setLogics.checkForSets(selectedCards) == 1) {
                numSets++;

                //Animate and add new cards
                if (!extraCardFlag) { 
                    foreach (GameObject item in selectedCards)
                    {
                        item.GetComponent<Card>().setSetWin();
                    }
                }
                else
                {
                    hideExtraCard();
                }
                SoundManager._instance.playSet();
                selectedCards.Clear();
            }else
            {
                SoundManager._instance.playNoSet();
            }
        }
    }

    public void assignNewCard(GameObject cardObj)
    {
        selectNumChange++;
        changeCard(cardObj);

        if (selectNumChange == 3)
        {
            selectNumChange = 0;
            int deckSets = checkDeckSets();
            changeText(deckSets);
            checkGameState(deckSets);
        }
    }

    private void setExtraCards()
    {
        foreach (GameObject item in extraCards)
        {
            item.SetActive(false);
            item.GetComponent<Animator>().SetBool("Hide", true);
            
        }
    }

    public void showExtraCard()
    {
        foreach (GameObject item in extraCards)
        {
            item.SetActive(true);
            changeCard(item);
        }
        extraCardFlag = true;
        cardContainer.GetComponent<Animator>().SetBool("MoveLeft", true);
        changeText(checkDeckSets());
    }

    public void hideExtraCard()
    {
        
        List<GameObject> unselectedExtra = new List<GameObject>();
         foreach (GameObject extraCard in extraCards)
        {
            if (!extraCard.GetComponent<Card>().selected) {
                unselectedExtra.Add(extraCard);
            }
        }

        List<GameObject> cardSelectNotExtra = new List<GameObject>();
        foreach (GameObject selectCard in selectedCards)
        {
            bool same = false;
            foreach (GameObject extraCard in extraCards)
            {
                if(selectCard.GetComponent<Card>().id == extraCard.GetComponent<Card>().id)
                {
                    same = true;
                }
            }
            if (!same)
            {
                cardSelectNotExtra.Add(selectCard);
            }
        }

        Debug.Log("Extra Count:" + unselectedExtra.Count);
        Debug.Log("Select count:" + cardSelectNotExtra.Count);

        for (int i = 0; i < cardSelectNotExtra.Count; i++)
        {
            cardSelectNotExtra[i].GetComponent<Card>().changeCardData(unselectedExtra[i].GetComponent<Card>().cardData);
            cardSelectNotExtra[i].GetComponent<Card>().selected = false;
            cardSelectNotExtra[i].GetComponent<Card>().togglePaper();
        }

        foreach (GameObject item in extraCards)
        {
            item.GetComponent<Card>().selected = false;
            item.GetComponent<Card>().togglePaper();
            item.SetActive(false);
        }
        cardContainer.GetComponent<Animator>().SetBool("MoveLeft", false);
        extraCardFlag = false;
        checkGameState(checkDeckSets());
    }

    public void showSet(int setNum)
    {
        sg += setNum * penaltieForSet;
        changeTime();
        List<GameObject> set = setLogics.getSet(getPlayDeck(), setNum);

        foreach (GameObject item in set)
        {
            item.GetComponent<Card>().changeColor();
        }
    }

    public void resetStage()
    {
        SceneManager.LoadScene(0); //Load scene called Game
    }
}