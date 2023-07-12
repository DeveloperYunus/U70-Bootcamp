using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MouseCamLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    public GameObject MainMenuCanvas, QuitGameMenu, DoorQuitGameMenu;
    bool bookOpened = false, book = false, door = false;
    public AudioSource MainMenuMusic;
    public Slider MusicSlider, SoundSlider;
    public AudioListener AudioListener;
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined; // Fare imleci ekranda sınırlı bir alanda hareket eder
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // Fare imleci görünür hale getirilir
    }

    void Update()
    {
        if (!bookOpened)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);

        }
        if (Input.GetMouseButtonDown(0))
        {
            if (book)
            {
                MainMenuCanvas.SetActive(true);
                MainMenuCanvas.transform.DOScale(new Vector3(1, 1, 1), 1);
                bookOpened = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (door)
            {

                DoorQuitGameMenu.SetActive(true);
                DoorQuitGameMenu.transform.DOScale(new Vector3(1, 1, 1), 1);
                bookOpened = true;
                Cursor.lockState = CursorLockMode.None;

            }

        }


    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Book")
            {
                book = true;
            }
            else
            {
                book = false;
            }

            if (hit.collider.gameObject.tag == "Door")
            {
                door = true;
            }
            else
            {
                door = false;
            }

        }
    }

    public void CloseMenu()
    {
        MainMenuCanvas.transform.DOScale(new Vector3(0, 0, 0), 1);
        bookOpened = false;
    }
    public void OpenQuitMenu()
    {
        QuitGameMenu.transform.DOScale(new Vector3(1, 1, 1), 1);

    }

    public void CloseQuitMenu()
    {
        QuitGameMenu.transform.DOScale(new Vector3(0, 0, 0), 1).OnComplete(() => QuitGameMenu.SetActive(false));

    }

    public void DoorCloseQuitMenu()
    {

        DoorQuitGameMenu.transform.DOScale(new Vector3(0, 0, 0), 1).OnComplete(() => QuitGameMenu.SetActive(false));
        bookOpened = false;
    }



    public void MusicVolumeChange()
    {
        MainMenuMusic.volume = MusicSlider.value;
    }
    public void SoundVolumeChange()
    {
        AudioListener.volume = SoundSlider.value;
    }
}