using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FollowCamera : MonoBehaviour
{

    [SerializeField] GameObject thingToFollow;
    [SerializeField] Text       timer;
    [SerializeField] float      seconds, minutes;
    [SerializeField] Button     restartButton;

    Delivery deliveryScript;

private void Start()
{
    deliveryScript = thingToFollow.GetComponent<Delivery>();
    restartButton.gameObject.SetActive(false);
}

    void LateUpdate() 
    {
        transform.position = thingToFollow.transform.position + new Vector3(0,0,-20);

        if (deliveryScript.GameState != "End")
        {
            minutes = (int)(Time.timeSinceLevelLoad / 60f);
            seconds = (int)(Time.timeSinceLevelLoad % 60f);
            timer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else
        {
            restartButton.gameObject.SetActive(true);
        }
    }
}
