using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class Hint : MonoBehaviour
{
    public int randNum;
    public GameObject hintDisp;
    public bool GeneraHint = false;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(HintTracker()); 
    }

    // Update is called once per frame
    void Update()
    {
        if (GeneraHint == false)
        {
            GeneraHint = true;
            StartCoroutine(HintTracker());
        }
    }

    IEnumerator HintTracker()
    {

        randNum = Random.Range(1,3);
        TMP_Text hintText = hintDisp.GetComponent<TMP_Text>();
        hintText.fontSize = 25;
        
        if (randNum == 1)
        {
            hintText.text = "Having difficulty fighting boss ?<color=#000000>,</color> try using stronger weapon";
        }

        if (randNum == 2)
        {
            hintText.text = "Health too low ?<color=#000000>,</color> try using item";
        }
        hintDisp.GetComponent<Animator>().Play("HinText");
        yield return new WaitForSeconds(9);
        GeneraHint = false;
    }
}
