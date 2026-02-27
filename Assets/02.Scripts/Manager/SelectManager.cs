using _02.Scripts;
using _02.Scripts.Manager;
using EnumTypes;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public static EnumTypes.SelectPlace SelectPlace;
    public static EnumTypes.SelectPeople SelectPeople;
    private static readonly int Property = Animator.StringToHash("Ani_0_0_TT_01(Hello)");

    [Header("Panel")]
    [SerializeField]
    private GameObject _selectPlacePanel;
    [SerializeField]
    private GameObject _selectPlayerCountPanel;
    [SerializeField]
    private GameObject _selectRoomPanel;

    [SerializeField] private GameObject _allInOnePanel;

    [Header("Button")]
    [SerializeField]
    private Button _stationButton;
    [SerializeField]
    private Button _subwayButton;
    [SerializeField]
    private Toggle _singleToggle;
    [SerializeField]
    private Toggle _multiToggle;

    [Header("FadeUI")]
    [SerializeField]
    private Canvas uiCanvas;
    
    [Header("Totta")] 
    [SerializeField] private GameObject totta;
    [SerializeField] private Transform tottaTransform;
    
    [Header("321Count")]
    [SerializeField] private CircleTimer countPanel;

    [Header("SoundManager")] [SerializeField]
    private SoundManager_ForStart _forStart;
    
    [Header("Loading")]
    public GameObject loadingPanel;

    public NetworkManager1 netRunner;

    public bool isTottaSkip = false;
    
    private void Start()
    {
        //StartCoroutine(TottaAppear());
        _stationButton.onClick.AddListener(() =>
        {
            _forStart.PlaySFX(0);
            SelectPeople = EnumTypes.SelectPeople.Single;
            SelectPlace = EnumTypes.SelectPlace.Station;
            StartCoroutine(CheckGender());
        });

        _subwayButton.onClick.AddListener(() =>
        {
            _forStart.PlaySFX(0);
            SelectPeople = EnumTypes.SelectPeople.Single;
            SelectPlace = EnumTypes.SelectPlace.Subway;
            StartCoroutine(CheckGender());
        });
    }

    private IEnumerator CheckGender()
    {
        _allInOnePanel.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        netRunner.genderUI.gameObject.SetActive(true);
    }

    public void GenderChoose(bool value)
    {
        _forStart.PlaySFX(0);
        netRunner.genderUI.gameObject.SetActive(false);
        AvatarManager.Instance.isMan = value;
        if(SelectPeople == SelectPeople.Multi) return;
        StartCoroutine(TottaAppear());
    }

    private IEnumerator waitLoadScene(string SceneName)
    {
        FadeManager.Instance.StartFadeIn();
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(SceneName);
    }

    public void TottaChaser()
    {
        StartCoroutine(TottaAppear());
    }
    public void TottaSkip()
    {
        isTottaSkip = true;
    }

    // public void SetPlayerCount()
    // {
    //     if (!_singleToggle.isOn && !_multiToggle.isOn)
    //     {
    //         // fix me: pop up 창 띄우기
    //         return;
    //     }
    //
    //     if (_singleToggle.isOn)
    //     {
    //         SelectPeople = EnumTypes.SelectPeople.Single;
    //         FadeManager.Instance.StartFadeOut();
    //         _singleToggle.isOn = false;
    //
    //         //StartCoroutine(TottaAppear());
    //     }
    //     else if (_multiToggle.isOn)
    //     {
    //         SelectPeople = EnumTypes.SelectPeople.Multi;
    //         _selectPlayerCountPanel.SetActive(false);
    //         _selectRoomPanel.SetActive(true);
    //
    //         //FadeOn Bool값 True 변경
    //        // uiCanvas.gameObject.SetActive(true);
    //         //uiCanvas.GetComponent<FadeOut>().FadeOn();
    //
    //         _multiToggle.isOn = false;
    //        // FadeManager.Instance.StartNetworkFadeOut();
    //         NetworkManager.JoinLobby();
    //     }
    // }

    public void ForcedMulti()
    {
        SelectPeople = EnumTypes.SelectPeople.Multi;
        //NetworkManager.JoinLobby();
    }

    IEnumerator TottaAppear()
    {
        if(SelectPeople == SelectPeople.Multi) yield break;
        netRunner.GetComponent<NetworkManager1>().tottaSkip.SetActive(true);
        _allInOnePanel.SetActive(false);
        Animator tottaAnim = Instantiate(totta, tottaTransform).GetComponent<Animator>();
        tottaAnim.SetBool(Property, true);
        _forStart.PlayNa(0);

        while (_forStart.naSource.isPlaying)
        {
            if (isTottaSkip) break;
            yield return null;
        }
        netRunner.GetComponent<NetworkManager1>().tottaSkip.SetActive(false);
        _forStart.naSource.Stop();
        tottaAnim.SetBool(Property, false);
        tottaAnim.gameObject.SetActive(false);
        
        countPanel.gameObject.SetActive(true);
        while (countPanel._fillTime < countPanel.limitTime)
        {
            yield return null;
        }

        loadingPanel.SetActive(true);
        StartCoroutine(ShutDownPhoton());
        FadeManager.Instance.StartFadeOut();

        //Debug.Log("SceneChange :" + SelectPlace + " " + SceneManager.GetActiveScene().name);
        if (SelectPlace == EnumTypes.SelectPlace.Station)
        {
            SceneManager.LoadScene(EnumTypes.SceneName.StationSingleScene.ToString());
        }
        else
        {
            SceneManager.LoadScene(EnumTypes.SceneName.SubwaySingleScene.ToString());
        }
    }

    IEnumerator ShutDownPhoton()
    {
        Debug.LogError("Shutting Down Photon");
        netRunner.GetComponent<NetworkRunner>().Shutdown();
        yield return null;
    }
}
