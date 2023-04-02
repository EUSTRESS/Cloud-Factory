using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoUIManager : MonoBehaviour
{
    public GameObject FadeRecordOfHealing;
    private Guest mGuestManager;
    private SeasonDateCalc mSeasonDateCalc;

	// Start is called before the first frame update
	void Start()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mSeasonDateCalc = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>();
        SetInactiveFadeROH();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInactiveFadeROH()
	{
        if(mSeasonDateCalc.isSatOrDisSatGuestExist == true) { FadeRecordOfHealing.SetActive(false); }
    }
}
