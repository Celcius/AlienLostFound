using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField]
    private Vector2 offsetFromMouse = new Vector2(5,5);

    [SerializeField]
    private Camera MyCamera;

    [SerializeField]
    private Image highlight;
    private void Start() 
    {
        rectTransform =  (RectTransform) transform;
    }

    public void ShowCredits()
    {
        UpdatePos();
        highlight.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
    }

    private void UpdatePos()
    {
        return;
        rectTransform = (RectTransform) transform;
        rectTransform.position = Input.mousePosition + (Vector3)offsetFromMouse;
    }

    private void Update() 
    {
        highlight.gameObject.SetActive(this.gameObject.activeInHierarchy);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void OpenIvoURL()
    {
        Application.OpenURL("https://ivocapelo.com/");
    }

    public void OpenBastafunkURL()
    {
        Application.OpenURL("https://www.bastafunk.com/");
    }

    public void SwapCredits()
    {
        highlight.gameObject.SetActive(!this.gameObject.activeInHierarchy);
        this.gameObject.SetActive(!this.gameObject.activeInHierarchy);
    }
}
