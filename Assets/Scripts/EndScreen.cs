using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI finalScoreText;
    private ScoreKeeper scoreKeeper;

    private void Awake() {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void ShowFinalScore() {
        finalScoreText.text = scoreKeeper.CalculateScore() + "%";
    }

}
