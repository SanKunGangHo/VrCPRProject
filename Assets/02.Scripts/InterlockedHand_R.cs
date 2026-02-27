using UnityEngine;

public class InterlockedHand_R: MonoBehaviour
{
    public InterlockedFinger headScript;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hands")) headScript.rightHand = true;

        if (other.CompareTag("Patient") && headScript.handsInterLocked)
        {
            headScript.count++;
        }
        
        if (!headScript.isSingle)
        {
            headScript.playerData.ChangeScore(headScript.count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Hands")) headScript.rightHand = false;
    }
}
