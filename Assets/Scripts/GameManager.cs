using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // UI Elements
    [SerializeField] Image PlayerImage;
    [SerializeField] Image PlayerLostImage;
    [SerializeField] Image PlayerSpeech;
    [SerializeField] Text PlayerSpeechText;
    [SerializeField] Image WizardImage;
    [SerializeField] Image WizardLostImage;
    [SerializeField] Image WizardSpeech;
    [SerializeField] Text WizardSpeechText;
    [SerializeField] Button BeginButton;

    [SerializeField] GameObject InputHolder;
    [SerializeField] TextMeshProUGUI InputText;
    [SerializeField] TextMeshProUGUI InputInfo;
    [SerializeField] TextMeshProUGUI InputTitle;
    [SerializeField] TMP_InputField InputTextDisplay;

    // Game Elements
    [SerializeField] int max;
    [SerializeField] int min;
    int wizardGuessNumber;
    int wizardSacredNumber;
    int playerGuessNumber;
    int playerSacredNumber;
    int guessCount = 0;
    int validatedNumber;

    // States
    enum State
    {
        INPUT_SACRED_NUMBER,
        INTRO,
        GAME
    }
    State currentState;

    // Core
    private void Start()
    {
        max = max + 1;
        min = min - 1;
        wizardGuessNumber = UnityEngine.Random.Range(min, max);
        currentState = State.INPUT_SACRED_NUMBER;
    }

    private void SetWizardSacredNumber()
    {
        wizardSacredNumber = UnityEngine.Random.Range(min, max);
    }

    private void StartIntro()
    {
        StartCoroutine(ShowWizard(.5f));
        StartCoroutine(ShowWizardDialogue("I Am Matheeko The Magnificent.\nThe Greatest Number Wizard Of All time!", 1f));
        StartCoroutine(ShowPlayer(2.5f));
        StartCoroutine(ShowPlayerDialogue("I, Areethma The Extraordaire,\nchallenge you to a number duel!", 3f));
        StartCoroutine(ShowBeginButton(4f));

    }

    public void StartGame()
    {
        currentState = State.GAME;
        HideWizardDialogue();
        HidePlayerDialogue();
        BeginButton.gameObject.SetActive(false);
        SetWizardSacredNumber();
        MainGame();
    }

    public void MainGame()
    {
        StartCoroutine(ClearDialogues(0f));
        WizardGuess();
        CheckWizardGuess();
        StartCoroutine(PlayerGuess(2f));
    }

    private IEnumerator PlayerGuess(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HidePlayerDialogue();
        HideWizardDialogue();
        SetInputHolderForGuess();
        ShowInputHolder();
    }

    private void ManageState()
    {
        if (currentState == State.INPUT_SACRED_NUMBER)
        {
            HideInputHolder();
            StartIntro();
        }
        else if (currentState == State.GAME)
        {
            HideInputHolder();
            StartCoroutine(ShowPlayerDialogue("Is your sacred number " + playerGuessNumber, .5f));
            CheckPlayerGuess();
        }
    }

    private void CheckPlayerGuess()
    {
        if (playerGuessNumber == wizardSacredNumber)
        {
            StartCoroutine(ShowWizardDialogue("FORGIVE ME NUMERISA", 1f));
            StartCoroutine(WizardDies(2f));
        }
        else if(playerGuessNumber == 1)
        {
            StartCoroutine(ShowWizardDialogue("FORGIVE ME NUMERISA", 1f));
            StartCoroutine(WizardDies(2f));
        }
        else
        {
            PlayerWrong();
        }
    }

    private void WizardGuess()
    {
        guessCount++;
        if (guessCount == 1)
        {
            StartCoroutine(ShowWizardDialogue("Let's start with " + wizardGuessNumber, .5f));
        }
        else
        {
            if (guessCount % 2 == 0)
            {
                wizardGuessNumber = (max + min) / 2;
            }
            else
            {
                wizardGuessNumber = UnityEngine.Random.Range(min, max);
            }

            StartCoroutine(ShowWizardDialogue("Is it " + wizardGuessNumber, .5f));
        }

        HidePlayerDialogue();
    }

    private void CheckWizardGuess()
    {
        if (wizardGuessNumber == playerSacredNumber)
        {
            StartCoroutine(ShowPlayerDialogue("NOOOO", 1f));
            StartCoroutine(PlayerDies(2f));
        }
        else if(playerSacredNumber == 1)
        {
            StartCoroutine(ShowPlayerDialogue("NOOOO", 1f));
            StartCoroutine(PlayerDies(2f));
        }
        else
        {
            WizardWrong();
        }
    }

    private void WizardWrong()
    {
        if(wizardGuessNumber > playerSacredNumber)
        {
            max = wizardGuessNumber;
            HidePlayerDialogue();
            HideWizardDialogue();
            StartCoroutine(ShowPlayerDialogue("Lower", 1f));
        }
        else
        {
            min = wizardGuessNumber;
            HidePlayerDialogue();
            HideWizardDialogue();
            StartCoroutine(ShowPlayerDialogue("Higher", 1f));
        }
    }

    private void PlayerWrong()
    {
        if (playerGuessNumber > wizardSacredNumber)
        {
            HidePlayerDialogue();
            HideWizardDialogue();
            StartCoroutine(ShowWizardDialogue("Lower", 1f));
        }
        else
        {
            HidePlayerDialogue();
            HideWizardDialogue();
            StartCoroutine(ShowWizardDialogue("Higher", 1f));
        }
    }

    // Mechanic Handlers
    private void ResetRound()
    {
        HidePlayerDialogue();
        HideWizardDialogue();
        playerGuessNumber = 0;
        wizardGuessNumber = 0;
    }

    private IEnumerator LoadWizardWinScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("WizardWinScene");
    }

    private IEnumerator LoadPlayerWinScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("PlayerWinScene");
    }


    // Button Handlers
    public void OnClickBegin()
    {
        StartGame();
    }

    public void OnClickSubmit()
    {
        if(ValidateInput(InputText.text))
        {
            AcceptInput();
            ManageState();
        }
        else
        {
            RejectInput();
        }
    }

    // Input Handlers
    private bool ValidateInput(string input)
    {
        int tempNumber = 0;
        validatedNumber = 0;

        if (int.TryParse(input, out tempNumber))
        {
            if(tempNumber >= 1 && tempNumber <= 1000)
            {
                validatedNumber = tempNumber;
                return true;
            }
        }

        return false;
    }

    private void RejectInput()
    {
        InputInfo.color = Color.red;
        InputTextDisplay.text = "....";
    }

    private void AcceptInput()
    {
        if(currentState == State.INPUT_SACRED_NUMBER)
        {
            playerSacredNumber = validatedNumber;
            Debug.Log("Player Sacred Number set to " + playerSacredNumber);
        }
        else if (currentState == State.GAME)
        {
            playerGuessNumber = validatedNumber;
            Debug.Log("Player guess set to " + playerGuessNumber);
        }
        else
        {
            Debug.Log("Input valid but can't determine state");
        }
    }

    // UI Handlers
    private void ShowWizardDialogue(string dialogue)
    {
        WizardSpeechText.text = dialogue;
        WizardSpeech.gameObject.SetActive(true);
    }

    private IEnumerator ShowWizardDialogue(string dialogue, float seconds)
    {
        WizardSpeechText.text = dialogue;
        yield return new WaitForSeconds(seconds);
        WizardSpeech.gameObject.SetActive(true);
    }

    private void ShowPlayerDialogue(string dialogue)
    {
        PlayerSpeechText.text = dialogue;
        PlayerSpeech.gameObject.SetActive(true);
    }

    private IEnumerator ShowPlayerDialogue(string dialogue, float seconds)
    {
        PlayerSpeechText.text = dialogue;
        yield return new WaitForSeconds(seconds);
        PlayerSpeech.gameObject.SetActive(true);
    }

    private void HideWizardDialogue()
    {
        WizardSpeech.gameObject.SetActive(false);
    }

    private void HidePlayerDialogue()
    {
        PlayerSpeech.gameObject.SetActive(false);
    }

    private IEnumerator PlayerDies(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayerImage.gameObject.SetActive(false);
        PlayerLostImage.gameObject.SetActive(true);
        HideWizardDialogue();
        StartCoroutine(ShowWizardDialogue("Another one bites the dust.", 1f));
        StartCoroutine(LoadWizardWinScene(3f));
    }

    private IEnumerator WizardDies(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        WizardImage.gameObject.SetActive(false);
        WizardLostImage.gameObject.SetActive(true);
        HideWizardDialogue();
        StartCoroutine(ShowPlayerDialogue("Your power is mine.", 1f));
        StartCoroutine(LoadPlayerWinScene(3f));
    }

    private IEnumerator ShowWizard(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        WizardImage.gameObject.SetActive(true);
    }

    private IEnumerator ShowPlayer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayerImage.gameObject.SetActive(true);
    }

    private IEnumerator ShowBeginButton(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        BeginButton.gameObject.SetActive(true);
    }

    private void HideInputHolder()
    {
        InputHolder.SetActive(false);
    }

    private void ShowInputHolder()
    {
        InputHolder.SetActive(true);
    }

    private void SetInputHolderForGuess()
    {
        InputTitle.text = "Enter Guess";
        InputTextDisplay.text = "....";
        InputInfo.color = Color.white;
    }

    private IEnumerator ClearDialogues(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HidePlayerDialogue();
        HideWizardDialogue();
    }

}