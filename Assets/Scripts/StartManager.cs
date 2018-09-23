using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour {

    [SerializeField] Button StartButton;

    IEnumerator BlinkButton()
    {
        StartButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameScene");
    }

    public void ManageButtonClick()
    {
        StartCoroutine(BlinkButton());
    }

}
