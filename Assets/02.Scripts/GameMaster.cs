using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : NetworkBehaviour
{
    [field: Header("Player")]
    [Networked] public NetworkObject player1 { get; set; }
    [Networked] public NetworkObject player2 { get; set; }
    
    [Header("SpawnPoint")]
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;

    [Header("UI")] 
    public List<GameObject> UIList;

    [Header("Totta")] public Animator Totta;

    private NetworkRunner runner;

    private WaitForSeconds dotOne = new WaitForSeconds(0.1f);

    #region Totta

    private IEnumerator TottaAppear(string name, float waitTime = 0)
    {
        Totta.gameObject.SetActive(true);
        if (name == "Ani_1_1_TT_01(Pointing)")
        {
            //대충 나레이션 추가
        }
        Totta.SetBool(name, true);

        while (true)
        {
            AnimatorStateInfo stateInfo = Totta.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.normalizedTime >= 1.0f)
            {
                Totta.SetBool(name, false);
                break;
            }
        }
        yield return new WaitForSeconds(waitTime);
        Totta.gameObject.SetActive(false);
    }

    #endregion
    
    IEnumerator Start()
    {
        //대합실 내부 위치하도록
        while(player1 == null || player2 == null)
        {
            if (runner == null) runner = NetworkManager1.RunnerInstance;
            if (runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR) player1 = runner.GetPlayerObject(runner.LocalPlayer);
            else player2 = runner.GetPlayerObject(runner.LocalPlayer);
            yield return dotOne;
        }
        player1.transform.position = spawnPoint1.position;
        player2.transform.position = spawnPoint2.position;
        //TODO : 다음 스텝
        StartCoroutine(SituationStation_1_1());
        yield break;
    }

    IEnumerator SituationStation_1_1()
    {
        yield return StartCoroutine(TottaAppear("Ani_1_1_TT_01(Pointing)"));
        
        yield return null;
    }
    
    
}
