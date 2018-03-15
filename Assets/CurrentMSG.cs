using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentMSG : MonoBehaviour {

	List<string> msg = new List<string>{};
	string msgString = "";
	Text msgText;

	void Start() {
		msgText = GetComponent<Text>();
	}

	public void AddWord(string newWord) {
		msg.Add(newWord);
		UpdateMsgString();
	}

	void UpdateMsgString() {
		msgString = "";
		foreach (string word in msg) {
			msgString = msgString + word + " ";
		}
		msgText.text = msgString;
	}

	public void EmptyMsg() {
		msg.Clear();
		UpdateMsgString();
		}

    public List<string> Msg
    {
        get
        {
            return msg;
        }

        set
        {
            msg = value;
        }
    }
}
