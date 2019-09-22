using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public int id;
    private DeckManager deckManager;
    public bool selected;
    public CardData cardData;
    public GameObject paper;
    public GameObject sprite1;
    public GameObject sprite2;
    public GameObject sprite3;
    public List<Color> colors;
    public SpritesList sprites = new SpritesList();
    public List<GameObject> cardNumb;
    


    // Use this for initialization
    void Start () {
        
    }

    private void hideShapes()
    {
        foreach (GameObject item in cardNumb)
        {
            item.SetActive(false);
        }
    }

    public void changeCardData(CardData card)
    {
        hideShapes();
        cardData = card;
        GameObject cardNumContainer = cardNumb[card.number];
        cardNumContainer.SetActive(true);
        foreach (Transform child in cardNumContainer.transform)
        {
            child.GetComponent<SpriteRenderer>().sprite = sprites.list[card.cardShape].spritesList[card.fill];
            child.GetComponent<SpriteRenderer>().color = colors[card.color];
        }
        
    }
	
	public void togglePaper()
    {
        if (selected) {
            //paper.GetComponent<SpriteRenderer>().color = new Color(0.9f,0.9f,0.9f);
            this.GetComponent<Animator>().SetBool("PopDown",true);
            this.GetComponent<Animator>().SetBool("Rotate", false);
            this.GetComponent<Animator>().SetBool("Idle", false);
            this.GetComponent<Animator>().SetBool("Scale", false);
        }
        else
        {
            this.GetComponent<Animator>().SetBool("Scale", false);
            //paper.GetComponent<SpriteRenderer>().color = Color.white;
            this.GetComponent<Animator>().SetBool("PopDown", false);
            this.GetComponent<Animator>().SetBool("Rotate", true);
            this.GetComponent<Animator>().SetBool("Idle", true);
        }
       
    }

    public void setSetWin()
    {
        this.GetComponent<Animator>().SetBool("SetWin", true);
    }

    public void setSetWinChangeSelect()
    {
        this.GetComponent<Animator>().SetBool("SetWin", true);
    }

    public void setSetWinFalse()
    {
        if (DeckManager._instance!=null)
        {
            DeckManager._instance.assignNewCard(this.gameObject);
        }
        if (SetTwo._instance != null)
        {
            SetTwo._instance.assignNewCard(this.gameObject);
        }

        this.GetComponent<Animator>().SetBool("SetWin", false);
        this.GetComponent<Animator>().SetBool("Idle", true);
        this.GetComponent<Animator>().SetBool("Rotate", false);
        this.GetComponent<Animator>().SetBool("PopDown", false);
        this.GetComponent<Animator>().SetBool("Scale", false);
        selected = false;
        SoundManager._instance.playCardFlip();
    }

    public void changeColor()
    {
        this.GetComponent<Animator>().SetBool("Scale", true);
    }

}




[System.Serializable]
public class SpritesList
{

    public List<SpritesCards> list;
}


[System.Serializable]
public class SpritesCards
{
    public List<Sprite> spritesList;

}

