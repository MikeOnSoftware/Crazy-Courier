using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Driver : MonoBehaviour
{
    [SerializeField] float       steerSpeed   = 95f;
    [SerializeField] float       moveSpeed    = 8f;
    [SerializeField] float       slowSpeed    = -4f;
    [SerializeField] float       boostSpeed   = 4f;
    [SerializeField] Text        stopWatch; 
    [SerializeField] Text        driverSays;
    [SerializeField] AudioSource speedUpSound;
    [SerializeField] AudioSource speedDownSound;
    
    const float mRegularSpeed  = 8f;
    float       mTimeRemaining = 10;
    GameObject  mTriggeredSpeedChanger;

    [Header("MOBILE")]
    [SerializeField] Button forth;
    [SerializeField] Button back;
    [SerializeField] Button up;
    [SerializeField] Button down;
    bool isForthSelected;
    bool isBackSelected;
    bool isUpSelected;
    bool isDownSelected;

    void Awake()
    {
        forth.gameObject.SetActive(false);
        back.gameObject.SetActive(false);
        up.gameObject.SetActive(false);
        down.gameObject.SetActive(false);

        if (Application.isMobilePlatform)
        {
            forth.gameObject.SetActive(true);
            back.gameObject.SetActive(true);
            up.gameObject.SetActive(true);
            down.gameObject.SetActive(true);
        }
    }

    void Update()
    {
            if (Input.GetAxis("Horizontal") > 0 || isForthSelected) transform.Translate(0,moveSpeed * Time.deltaTime,0);
       else if (Input.GetAxis("Horizontal") < 0 || isBackSelected) transform.Translate(0,-moveSpeed * Time.deltaTime,0);
            if (Input.GetAxis("Vertical") > 0 || isUpSelected) transform.Rotate(0,0,steerSpeed * Time.deltaTime);
       else if (Input.GetAxis("Vertical") < 0 || isDownSelected) transform.Rotate(0,0,-steerSpeed * Time.deltaTime);
            
       if(stopWatch.gameObject.activeSelf == true)
       {
          if (mTimeRemaining > 0)
          {
             mTimeRemaining -= Time.deltaTime;
             stopWatch.text = mTimeRemaining.ToString("0.00");
          }
          else
          {
             stopWatch.gameObject.SetActive(false);
             driverSays.text = "";
             moveSpeed = mRegularSpeed;
             mTriggeredSpeedChanger.SetActive(true);
          }
       }
    }

    public void OnPointerDownMoveControllerMobile(string direction)
    {
        switch (direction)
        {
            case "forth":
                isForthSelected = true;
                break;
            case "back":
                isBackSelected = true;
                break;
            case "up":
                isUpSelected = true;
                break;
            case "down":
                isDownSelected = true;
                break;
            default:
                break;
        }
    }
    public void OnPointerUpMoveControllerMobile(string direction)
    {
        switch (direction)
        {
            case "forth":
                isForthSelected = false;
                break;
            case "back":
                isBackSelected = false;
                break;
            case "up":
                isUpSelected = false;
                break;
            case "down":
                isDownSelected = false;
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       if(other.tag == "SpeedUp" || other.tag == "SpeedDown")
       {
          mTimeRemaining = 10;
          if(other.tag == "SpeedUp") 
          {
             speedUpSound.Play();
             driverSays.text = "Uhuu...";
             moveSpeed = mRegularSpeed + boostSpeed; 
          }
          if(other.tag == "SpeedDown") 
          {
             speedDownSound.Play();
             driverSays.text = "Ohh...";
             moveSpeed = mRegularSpeed + slowSpeed; 
          }
          //if speed thing is already taken (not null) and bumpin into another, return first one to true
          mTriggeredSpeedChanger?.SetActive(true); 
          mTriggeredSpeedChanger = other.gameObject;
          mTriggeredSpeedChanger.SetActive(false);
    
          stopWatch.gameObject.SetActive(true);
       }
       if(other.tag == "Package") 
       {
          driverSays.text = "Package picked!";
          Invoke("DeleteMessage", 2f);
       }
    }
    void DeleteMessage()
    {
       driverSays.text = "";
    }
    
    public void ReloadScene()
    {
       SceneManager.LoadScene(0);
    }
}
