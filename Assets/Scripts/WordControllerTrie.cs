using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WordControllerTrie : MonoBehaviour {

	public TextAsset storyFile;
	public GameObject wordPrefab;
	private Transform canvas;
	public GameObject currentMSGObj;
	private CurrentMSG currentMSGScript;

	private Trie words;
	private TrieNode curNode;

	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("Canvas").transform;
		currentMSGScript = currentMSGObj.GetComponent<CurrentMSG>();
		LoadWords();

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonUp("Reset")) {
			ResetWords();
		}
	}

	void LoadWords() {
		string[] lines = storyFile.text.Split('\n');
		foreach(string line in lines) {
			//Purge all extra spaces from the end
			words.insert(line);
		}
		curNode = words.getRoot();
	}

	void SpawnNextWords() {
		string[] wordsToSpawn = curNode.getChildWords();
		SpawnTheseWords(wordsToSpawn);
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
}
