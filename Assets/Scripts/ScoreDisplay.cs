using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {
    private Text text;


    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
        text.text = ScoreKeeper.score.ToString();
        ScoreKeeper.Reset();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
