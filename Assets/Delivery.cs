using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Delivery : MonoBehaviour
{
    [SerializeField] float        destroyDelay    = 0.5f;
    [SerializeField] Color32      hasPackageColor = new Color32(1,1,1,1);
    [SerializeField] Color32      noPackageColor  = new Color32(1,1,1,1);
    [SerializeField] Text         customerSays;
    [SerializeField] Text         packagesCount;
    [SerializeField] GameObject[] allPackagesInScene;

    int    mPackagesLeft;
    int    mPickedPackages    = 0;
    int    mDeliveredPackages = 0;    
    string mGameState         = "Playing";

    SpriteRenderer mSpriteRenderer;

    public int DeliveredPackages
    {
        get { return mDeliveredPackages; }
        private set { mDeliveredPackages = value; } 
    }

    public string GameState 
    { 
        get { return mGameState; }
        private set { mGameState = value; }
    }

    void Start()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        allPackagesInScene = GameObject.FindGameObjectsWithTag("Package");
    }
    void Update()
    {   
        mPackagesLeft = allPackagesInScene.Length - DeliveredPackages;
        packagesCount.text = "Packages = " + mPackagesLeft.ToString();
        if(mPackagesLeft == 0) GameState = "End";
    }

    void OnTriggerEnter2D(Collider2D other)
{
        if (other.tag == "Package"
        && mPickedPackages == 0)  //can only take 1 package at a time
        {
            mPickedPackages++;
            Destroy(other.gameObject, destroyDelay);
            mSpriteRenderer.color = hasPackageColor; //change car's color
        }
        else if (other.tag == "Customer")
        {
            if (mPickedPackages > 0)
            {
                mPickedPackages--;
                customerSays.text = "Package delivered!";
                DeliveredPackages++;

                mSpriteRenderer.color = noPackageColor; //change car's color
            }
            else
            {
                customerSays.text = "Hey you forgot my package!";
            }
            Invoke("DeleteMessage", 3f);
        }
    }
    void DeleteMessage()
    {
       customerSays.text = "";
    }
}
