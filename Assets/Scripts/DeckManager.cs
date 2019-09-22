using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckManager : SetLogics
{
    public static DeckManager _instance;
    public List<GameObject> extraCards;
    private int selectNumChange;

    private bool extraCardFlag;


    // Use this for initialization
    void Start() {
        gameModeBestTimeKey = "setOneBestTime";
        if (_instance == null)
        {
            _instance = this;
        }
        //Assing Number of cards in the deck this number is
        createDeck();
        finishUI.SetActive(false);
        checkDeckSets();
        StartCoroutine(startTime());
        changeText(checkDeckSets());
        setExtraCards();
        checkBestTime();

        Debug.Log("Start DeckManager");
    }

    public override void extraFunctionalitytwo()
    {
        showExtraCard();
    }

    public override void assignNewCard(GameObject cardObj)
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

    public override void extraFunctionality() { 

        if (!extraCardFlag)
        {
            foreach (GameObject item in selectedCards)
            {
                item.GetComponent<Card>().setSetWin();
            }
        }
        else
        {
            hideExtraCard();
        }
    }
}