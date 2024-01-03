using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour {
    [Header("Questions")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private List<QuestionSO> questions = new List<QuestionSO>();
    private QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] private GameObject[] answerButtons;
    private bool hasAnsweredEarly;
    private bool hasTimerRanOut = false;

    [Header("Button Sprites")]
    [SerializeField] private Sprite defaultAnswerSprite;
    [SerializeField] private Sprite correctAnswerSprite;
    private Color defaultColorBackground = new Color32(91, 120, 154, 255);
    private Color correctAnswerColorBackground = new Color32(178, 149, 117, 255);
    private Color incorrectAnswerColorBackground = new Color32(178, 117, 125, 255);

    [Header("SFX")]
    [SerializeField] AudioClip wrongSFX;
    [SerializeField] AudioClip correctSFX;
    private AudioSource audio;

    [Header("Timer")]
    [SerializeField] private Timer timer;
    [SerializeField] private GameObject timerImage;


    private void Awake() {
        audio = GetComponent<AudioSource>();
    }

    private void Update() {
        UpdateImageTimerFillAmount();

        if (timer.GetLoadNextQuestion()) {
            hasAnsweredEarly = false;
            hasTimerRanOut = false;
            GetNextQuestion();
            timer.SetLoadNextQuestion(false);
        } else if (!hasAnsweredEarly && !timer.GetIsAnsweringQuestion() && !hasTimerRanOut) {
            DisplayAnswer();
            SetButtonStates(false);
            hasTimerRanOut = true;
        }
    }

    private void DisplayQuestionAndAnswers() {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++) {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    public void OnAnswerSelected(int index) {
        hasAnsweredEarly = true;
        ShowCorrectAnswer(index);
        SetButtonStates(false);
    }

    private void ShowCorrectAnswer(int index) {
        Image buttonClickedImage = answerButtons[index].GetComponent<Image>();
        int correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
        Image correctButtonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();

        if (index == currentQuestion.GetCorrectAnswerIndex()) {
            audio.PlayOneShot(correctSFX, 0.15f);
            questionText.text = "Correct! Your wisdom rivals even mine. Almost.";
        } else {
            audio.PlayOneShot(wrongSFX, 1f);
            questionText.text = "Incorrect! Even my cat would have guessed better. No offense, Whiskers.";
            buttonClickedImage.color = incorrectAnswerColorBackground;
        }

        correctButtonImage.sprite = correctAnswerSprite;
        correctButtonImage.color = correctAnswerColorBackground;

        timer.CancelTimer();
    }

    private void DisplayAnswer() {
        int correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
        Image correctButtonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();

        audio.PlayOneShot(wrongSFX, 1f);
        questionText.text = "Tick-tock, tick-tock. The sands settle, and alas, time outran your answer. Fear not, for each question is a lesson, and wisdom often takes its time to unfold.";

        correctButtonImage.sprite = correctAnswerSprite;
        correctButtonImage.color = correctAnswerColorBackground;
    }

    private void SetButtonStates(bool isInteractable) {
        foreach (GameObject answerButton in answerButtons) {
            answerButton.GetComponent<Button>().interactable = isInteractable;
        }
    }

    private void GetNextQuestion() {
        if (questions.Count > 0) {
            SetButtonStates(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestionAndAnswers();
        }
    }

    private void GetRandomQuestion() {
        int randomIndex = Random.Range(0, questions.Count);
        currentQuestion = questions[randomIndex];

        if (questions.Contains(currentQuestion)) {
            questions.Remove(currentQuestion);
        }
    }

    private void SetDefaultButtonSprites() {
        foreach (GameObject answerButton in answerButtons) {
            Image buttonImage = answerButton.GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
            buttonImage.color = defaultColorBackground;
        }
    }

    private void UpdateImageTimerFillAmount() {
        float fillAmount;

        if (timer.GetIsAnsweringQuestion()) {
            fillAmount = timer.GetTimerValue() / timer.GetTimeToCompleteQuestion();
        } else {
            fillAmount = timer.GetTimerValue() / timer.GetTimeToShowCorrectAnswer();
        }

        timerImage.GetComponent<Image>().fillAmount = fillAmount;
    }
}
