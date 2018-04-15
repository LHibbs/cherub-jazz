using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour {

	private Button myButton;
	private Text myText;
	private WordController wordControllerScript;
    private bool isQuestion = false;

    void Start()
    {
		myText = (Text) GetComponentInChildren(typeof(Text));
		wordControllerScript = GameObject.Find("WordController").GetComponent<WordController>();
    	myButton = GetComponent<Button>();
        myButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        wordControllerScript.WordClicked(myText.text);
        /*if(isQuestion) {
            wordControllerScript.QuestionWordClicked(myText.text);
        } else {
		    wordControllerScript.WordClicked(myText.text);
        }*/
    }

    public bool IsQuestion
    {
        get
        {
            return isQuestion;
        }

        set
        {
            isQuestion = value;
        }
    }
}
