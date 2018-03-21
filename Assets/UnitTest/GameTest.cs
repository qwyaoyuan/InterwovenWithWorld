using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
            iPlayerState.HP = 1;
        }
	}
}
