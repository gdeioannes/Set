using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SetTwo : SetLogics
{
    public static SetTwo _instance;
    private int selectNumChange;
    private int penaltieForSet = 30;
    private float posyUp;
    private float posyDown;
    private float speedDown = 0.05f;

    // Use this for initialization
    void Start()
    {
        gameModeBestTimeKey = "setTwoBestTime";
        if (_instance == null)
        {
            _instance = this;
        }
        //Assing Number of cards in the deck this number is
        createDeck();
        finishUI.SetActive(false);
        checkDeckSets();
        StartCoroutine(moveCardsDown());
        StartCoroutine(startTime());
        int deckSets = checkDeckSets();
        changeText(deckSets);
        checkBestTime();
        Debug.Log("Set Two");
    }

    IEnumerator moveCardsDown()
    {
        while (true)
        {
            showCards();
            foreach (Transform child in cardContainer.transform) {
                Debug.Log("Move"+child.name);
                Vector3 pos = child.position;
                child.position = new Vector3(pos.x, pos.y-speedDown, pos.z) ;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }


    private void showCards()
    {
        foreach (Transform child in cardContainer.transform)
        {
            posyDown = transform.TransformPoint(child.transform.position).y + Camera.main.orthographicSize;

            if(posyDown < Camera.main.transform.position.y-2)
            {
                child.transform.parent = cardContainer.transform;
                Vector3 pos=child.gameObject.transform.position;
                child.transform.position = new Vector3(child.position.x, pos.y+22.2f, pos.z);
                //Debug.Log("PosY:"+posy);
                
            }
        }
    }

    public override void extraFunctionality()
    {
        foreach (GameObject item in selectedCards)
        {
            item.GetComponent<Card>().setSetWin();
        }   
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

    public override void extraFunctionalitytwo()
    {
        Debug.Log("Second Extra functionality");
    }
}
