using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentMSG : MonoBehaviour {

	List<string> msg = new List<string>{};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
