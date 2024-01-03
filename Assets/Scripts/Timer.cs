using UnityEngine;

public class Timer : MonoBehaviour {
    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 10f;

    private float timerValue;
    private bool isAnsweringQuestion = true;
    private bool loadNextQuestion = true;

    private void Awake() {
        timerValue = timeToCompleteQuestion;
    }

    private void Update() {
        UpdateTimer();
    }

    private void UpdateTimer() {
        timerValue -= Time.deltaTime;

        if (timerValue <= 0) {
            if (!isAnsweringQuestion) {
                loadNextQuestion = true;
            }

            isAnsweringQuestion = !isAnsweringQuestion;
            timerValue = isAnsweringQuestion ? timeToCompleteQuestion : timeToShowCorrectAnswer;

        }
    }

    public void CancelTimer() {
        timerValue = 0;
    }

    public float GetTimerValue() {
        return timerValue;
    }

    public float GetTimeToCompleteQuestion() {
        return timeToCompleteQuestion;
    }

    public float GetTimeToShowCorrectAnswer() {
        return timeToShowCorrectAnswer;
    }

    public bool GetIsAnsweringQuestion() {
        return isAnsweringQuestion;
    }

    public bool GetLoadNextQuestion() {
        return loadNextQuestion;
    }

    public void SetLoadNextQuestion(bool loadNextQuestion) {
        this.loadNextQuestion = loadNextQuestion;
    }
}
