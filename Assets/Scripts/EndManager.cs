using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{

    [SerializeField] Button RestartButton;

    IEnumerator BlinkButton()
    {
        RestartButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("StartScene");
    }

    public void ManageButtonClick()
    {
        StartCoroutine(BlinkButton());
    }

}