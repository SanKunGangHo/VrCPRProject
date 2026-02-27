using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterlockedFinger : NetworkBehaviour
{
    //손깍지라는 뜻
    public bool leftHand, rightHand;
    
    public bool handsInterLocked = false;

    public PlayerData playerData;


    public int count = 0;

    public  bool isSingle = false;

    private IEnumerator Start()
    {
        while (true)
        {
            HandsInterlocked();
            if (!isSingle && playerData == null)
            {
                playerData = FindObjectOfType<PlayerData>();
            }
            yield return null;
        }
    }

    private void HandsInterlocked()
    {
        if (leftHand && rightHand)
        {
            handsInterLocked = true;
        }
        else
        {
            handsInterLocked = false;
        }
    }
}
