using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordControllerArray : MonoBehaviour {

	public TextAsset storyFile;
	public TextAsset questionsFile;
	public TextAsset answersFile;
	private int currentQuestionNumber = 0;
	private int cycles = 0;
	public int maxWords = 5;
	public int maxAnswers = 10;
	public GameObject word;
	private Transform canvas;
	public GameObject currentMSGObj;
	private CurrentMSG currentMSGScript;

	private List<List<string>> words = new List<List<string>>();
	private List<List<string>> questions = new List<List<string>>();
	private List<List<string>> answerOne = new List<List<string>>();
	private List<List<string>> answerTwo = new List<List<string>>();
	private List<List<string>> answerThree = new List<List<string>>();
	private List<List<List<string>>> allAnswers = new List<List<List<string>>>();
	private int currentAnswerNumber = -1;

	void Start () {
		canvas = GameObject.Find("Canvas").transform;
		currentMSGScript = currentMSGObj.GetComponent<CurrentMSG>();
		LoadWords();
		LoadQuestions();
		LoadAnswers();
		SpawnNextWords();
		//SpawnNextQuestion();
	}

	void Update() {
		Debug.Log(currentQuestionNumber);
		if(Input.GetButtonUp("Reset")) {
			if(cycles == 5)
			{
				ResetWords(true);
				currentQuestionNumber++;
				currentAnswerNumber++;
			} else {
				ResetWords(false);
			}
			cycles++;
		}

		if(Input.GetButtonUp("Fire1")) {
			currentAnswerNumber++;
		}
	}

	void LoadWords() {
		string[] lines = storyFile.text.Split('\n');
		for (int i = 0; i < lines.Length; i++) {
			string[] thisLine = lines[i].Split(',');
			List<string> sendLine = new List<string>();
			foreach (string s in thisLine) {
				if(s == "") {
					break;
				}
				sendLine.Add(s);
			}
			words.Add(sendLine);
		}

	}

	void LoadQuestions() {
		string[] lines = questionsFile.text.Split('\n');
		for (int i = 0; i < lines.Length; i++) {
			string[] thisLine = lines[i].Split(',');
			List<string> sendLine = new List<string>();
			foreach (string s in thisLine) {
				if(s == "") {
					break;
				}
				sendLine.Add(s);
			}
			questions.Add(sendLine);
		}
	}

	void LoadAnswers() {
		allAnswers.Add(answerOne);
		allAnswers.Add(answerTwo);
		allAnswers.Add(answerThree);
		int questionNumber = 0;
		string[] lines = answersFile.text.Split('\n');
		for (int i = 0; i < lines.Length; i++) {
			string[] thisLine = lines[i].Split(',');
			List<string> sendLine = new List<string>();
			bool breakLine = false;
			foreach (string s in thisLine) {
				if(s == "ENDANSWER") {
					breakLine = true;
					questionNumber++;
					break;
				}
				if(s == "") {
					break;
				}
				sendLine.Add(s);
			}
			if(!breakLine) {
				allAnswers[questionNumber].Add(sendLine);
			}
		}
	}

	void SpawnNextWords() {
		List<string> compatibleWords = new List<string>();
		//if this line is to short, skip it
		for(int i = 0; i < words.Count; i++) {
			if(words[i].Count <= currentMSGScript.Msg.Count) {
				continue;
			}
			//Not too short? lets assume it is corect
			bool addThisWord = true;
			//Check if every word already in current Message matches every coorosponding word in the line 
			for(int j = 0; j < currentMSGScript.Msg.Count; j++) {
				if(words[i][j] != currentMSGScript.Msg[j]) {
					addThisWord = false;
					break;
				}
			}
			//If we get here:
			// 1. The length of this line is > currentMSG
			// 2. Every word in the current msg is equal to the coorosponding part of the line
			if(addThisWord) {
				compatibleWords.Add(words[i][currentMSGScript.Msg.Count]);
			}
		}
		//compatibleWords.Add ( AddAnswerWords() )
		//If compatibleWords is empty, exit early
		if(compatibleWords.Count == 0) {
			return;
		}
		//Take a sample of the words which work
		//Probability of being chosen = (number needed)/(number left)
		List<string> wordsToSpawn = new List<string>();
		for (int i = compatibleWords.Count - 1; i >= 0; i--) {
			//If wordsToSpawn has enough entries, then we're done here
			if (wordsToSpawn.Count >= maxWords) {
				break;
			}
			//If this word is already in wordsToSpawn, then skip it and go to the next word
			if(wordsToSpawn.Contains(compatibleWords[i])) {
				continue;
			}
			float rand = Random.Range(0f, 1f);
			//Decide if this word should go in the wordsToSpawn, randomly, 
			// with the probibilty of a word being chosen increasing over time
			// Every word has an equal chance og being chosen
			if(rand <= (maxWords * 1.0f - wordsToSpawn.Count) / (i+1.0f)) {
				wordsToSpawn.Add(compatibleWords[i]);
			}
		}

		//Spawn words
		foreach (string w in wordsToSpawn) {
			Transform newWord = Instantiate(word, canvas).transform;
			RectTransform rt = newWord.GetComponent<RectTransform>();
			rt.position = RandomPosition();
			Text newWordText = newWord.Find("Text").GetComponent<Text>();
			newWordText.text = w;
		}
	}

	void SpawnNextAnswer() {
		//Take in currentAnswerNumber
		//If -1 ---> don't spawn answers
		if(currentAnswerNumber < 0) {
			return;
		}
		//If 0,1,2 ----> spawn answer 1,2,3
		List<List<string>> currentAnswer;
		currentAnswer = allAnswers[currentAnswerNumber];

		List<string> compatibleWords = new List<string>();
		//if this line is to short, skip it
		for(int i = 0; i < currentAnswer.Count; i++) {
			if(currentAnswer[i].Count <= currentMSGScript.Msg.Count) {
				continue;
			}
			//Not too short? lets assume it is corect
			bool addThisWord = true;
			//Check if every word already in current Message matches every coorosponding word in the line 
			for(int j = 0; j < currentMSGScript.Msg.Count; j++) {
				if(currentAnswer[i][j] != currentMSGScript.Msg[j]) {
					addThisWord = false;
					break;
				}
			}
			//If we get here:
			// 1. The length of this line is > currentMSG
			// 2. Every word in the current msg is equal to the coorosponding part of the line
			if(addThisWord) {
				compatibleWords.Add(currentAnswer[i][currentMSGScript.Msg.Count]);
				//Debug.Log(currentAnswer[i][currentMSGScript.Msg.Count]);
			}
		}
		//compatibleWords.Add ( AddAnswerWords() )
		//If compatibleWords is empty, exit early
		if(compatibleWords.Count == 0) {
			return;
		}
		//Take a sample of the words which work
		//Probability of being chosen = (number needed)/(number left)
		List<string> wordsToSpawn = new List<string>();
		for (int i = compatibleWords.Count - 1; i >= 0; i--) {
			//If wordsToSpawn has enough entries, then we're done here
			if (wordsToSpawn.Count >= maxAnswers) {
				break;
			}
			//If this word is already in wordsToSpawn, then skip it and go to the next word
			if(wordsToSpawn.Contains(compatibleWords[i])) {
				continue;
			}
			float rand = Random.Range(0f, 1f);
			//Decide if this word should go in the wordsToSpawn, randomly, 
			// with the probibilty of a word being chosen increasing over time
			// Every word has an equal chance og being chosen
			if(rand <= (maxWords * 1.0f - wordsToSpawn.Count) / (i+1.0f)) {
				wordsToSpawn.Add(compatibleWords[i]);
			}
		}

		//Spawn words
		foreach (string w in wordsToSpawn) {
			Transform newWord = Instantiate(word, canvas).transform;
			RectTransform rt = newWord.GetComponent<RectTransform>();
			newWord.GetComponent<Image>().color = Color.red;
			rt.position = RandomPosition();
			Text newWordText = newWord.Find("Text").GetComponent<Text>();
			newWordText.text = w;
			//Debug.Log(w);
		}
	}

	void SpawnNextQuestion() {
		foreach (string thisWord in questions[currentQuestionNumber]) {
			if(currentMSGScript.Msg.Contains(thisWord)) {
				continue;
			} else {
				Transform newWord = Instantiate(word, canvas).transform;
				RectTransform rt = newWord.GetComponent<RectTransform>();
				rt.position = RandomPosition();
				Text newWordText = newWord.Find("Text").GetComponent<Text>();
				newWordText.text = thisWord;
				newWord.GetComponent<Word>().IsQuestion = true;
				break;
			}
		}
	}

	public void WordClicked(string wordString) {
		currentMSGScript.AddWord(wordString);
		DeleteWords();
		SpawnNextWords();
		SpawnNextAnswer();
	}

	public void QuestionWordClicked(string wordString) {
		currentMSGScript.AddWord(wordString);
		DeleteWords();
		SpawnNextQuestion();
		SpawnNextAnswer();
	}

	Vector3 RandomPosition() {
		float xPos = Random.Range(-7f, 7f);
		float yPos = Random.Range(-2f, 2f);
		return new Vector3(xPos, yPos);
	}

	void DeleteWords() {
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Word")) {
			Destroy(g);
		}
	}

	void ResetWords(bool questionNow) {
		currentMSGScript.EmptyMsg();
		DeleteWords();
		if(questionNow) {
			SpawnNextQuestion();
		} else {
			SpawnNextWords();
			SpawnNextAnswer();
		}
	}

}
