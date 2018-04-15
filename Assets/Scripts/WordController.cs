using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WordController : MonoBehaviour {

	public TextAsset storyFile;
	public GameObject wordPrefab;
	private Transform canvas;
	public GameObject currentMSGObj;
	private CurrentMSG currentMSGScript;

	private int MAX_WORDS = 8;
	private Trie words;
	private TrieNode curNode;

	// Use this for initialization
	void Start () {
		words = new Trie();
		curNode = words.getRoot();
		canvas = GameObject.Find("Canvas").transform;
		currentMSGScript = currentMSGObj.GetComponent<CurrentMSG>();
		LoadWords();
		SpawnNextWords();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonUp("Reset")) {
			ResetWords();
		}
	}

	//TODO: Purge extra spaces
	//TODO: Remove M: and A:
	//TODO Remove [APRIL] lines
	void LoadWords() {
		string[] lines = storyFile.text.Split('\n');
		foreach(string line in lines) {
			//Purge all extra spaces from the end
			words.insert(line);
			Debug.Log(line);
		}
	}

	void SpawnNextWords() {
		string[] wordsToSpawn = curNode.getChildWords();
		if(wordsToSpawn.Length <= MAX_WORDS) {
			SpawnTheseWords(wordsToSpawn);
		} else {
			string[] subWordsToSpawn = new string[MAX_WORDS];
			for(int i = 0; i < MAX_WORDS; i++) {
				subWordsToSpawn[i] = wordsToSpawn[i];
			}
			SpawnTheseWords(subWordsToSpawn);
		}
	}

	private void SpawnTheseWords(string[] wordsToSpawn) {
		foreach (string w in wordsToSpawn) {
			Transform newWord = Instantiate(wordPrefab, canvas).transform;
			RectTransform rt = newWord.GetComponent<RectTransform>();
			rt.position = RandomPosition();
			Text newWordText = newWord.Find("Text").GetComponent<Text>();
			newWordText.text = w;
		}
	}

	Vector3 RandomPosition() {
		float xPos = Random.Range(-7f, 7f);
		float yPos = Random.Range(-2f, 2f);
		return new Vector3(xPos, yPos);
	}

	public void WordClicked(string wordString) {
		currentMSGScript.AddWord(wordString);
		DeleteWords();
		curNode = curNode.getChildren()[wordString];
		SpawnNextWords();
	}

	private void DeleteWords() {
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Word")) {
			Destroy(g);
		}
	}

	private void ResetWords() {
		currentMSGScript.EmptyMsg();
		DeleteWords();
		curNode = words.getRoot();
	}
}
