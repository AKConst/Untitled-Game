using UnityEngine;

public class StringTimer : MonoBehaviour
{
    [SerializeField] private float endStringTime; //how long will the timer last
    private float endStringTimeLive; //value that will be used for the actual timer

    void Update()
    {
        endStringTimeLive -= Time.deltaTime; //counting down

        //we reset the combo value if the timer hits 0
        if(endStringTimeLive <= 0.0f)
        {
            resetPSC();
        }
    }

    //function to reset the PSC (player string count) when the timer runs out
    private void resetPSC()
    {
        GetComponentInParent<PlayerAttack>().SetPSC(0);
    }

    //function that's called to reset the timer (called from the PlayerAttack script)
    public void resetTimer()
    {
        endStringTimeLive = endStringTime;
    }
}
