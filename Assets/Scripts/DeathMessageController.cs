using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class DeathMessageController : MonoBehaviour
{
    Animator anim;
    public TextAsset quoteSource;
    Text quoteText;
    string[] quotes;
    System.Random random;
    void Start()
    {
        anim = GetComponent<Animator>();
        quotes = quoteSource.text.Split('\n');
        random = new Random();
        quoteText = GetComponentInChildren<Text>();
    }
    void PickQuote()
    {
        string pickedQuote = quotes[random.Next(0, quotes.Length)];
        string[] separated = pickedQuote.Split('–');
        quoteText.text = separated[0] + "\n – " + separated[1];
    }

    public void FadeBlack()
    {
        PickQuote();
        anim.Play("FadeBlack");
    }
    public void FadeOut()
    {
        anim.Play("FadeOut");
    }
}
