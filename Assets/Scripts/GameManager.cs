using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MyBox;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    private static GameManager _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public enum GameStage
    {
        TitleScene,
        Making,
        HeadUp
    }
    [SerializeField]
    [ReadOnly] public GameStage currentStage;
    [SerializeField]
    [ReadOnly] private bool allowInput;
    private float timer;

    [Header("Title Stage")]
    [SerializeField] private CinemachineVirtualCamera titleSceneVirtualCamera;
    [SerializeField] CanvasGroup titleCanvasGroup;

    [Header("Making Stage")]
    [SerializeField] private CinemachineVirtualCamera makingVirtualCamera;
    [SerializeField] CanvasGroup makingCanvasGroup;
    [SerializeField] CandyMachine machine;
    [SerializeField] Hand hand;

    [Header("Head Up Stage")]
    [SerializeField] private CinemachineVirtualCamera headUpVirtualCamera;
    [SerializeField] CanvasGroup headUpCanvasGroup;

    private void Start()
    {
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
        titleSceneVirtualCamera.m_Priority = 1;
        makingVirtualCamera.m_Priority = 1;
        headUpVirtualCamera.m_Priority = 1;
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f);


        makingCanvasGroup.alpha = 0f;
        titleCanvasGroup.alpha = 0f;
        headUpCanvasGroup.alpha = 0f;

        machine.StopMachine();
        hand.allowInput = false;
        TransitToStage(GameStage.TitleScene);

        StartCoroutine(BlockInput(2f));


    }

    IEnumerator BlockInput(float duration)
    {
        yield return new WaitForSeconds(duration);
        allowInput = true;
    }
    private void Update()
    {
        if (allowInput == false)
            return;


        if (currentStage == GameStage.TitleScene)
        {
            if (Input.anyKeyDown)
            {
                TransitToStage(GameStage.Making);
            }
        }
        if (currentStage == GameStage.Making)
        {
            if (Input.anyKeyDown)
            {
                TransitToStage(GameStage.HeadUp);
            }
        }
        if (currentStage == GameStage.HeadUp)
        {
            if (Input.anyKeyDown)
            {
                TransitToStage(GameStage.Making);
            }
        }
    }

    public void TransitToStage(GameStage stage)
    {
        //  Leave current stage
        if (currentStage == GameStage.TitleScene)
        {
            titleCanvasGroup.DOFade(0f, 1f);

        }
        if (currentStage == GameStage.Making)
        {
            makingCanvasGroup.DOFade(0f, 1f);
            machine.StopMachine();
            
        }
        if (currentStage == GameStage.HeadUp)
        {
            headUpCanvasGroup.DOFade(0f, 1f);
        }

        //  Enter new stage
        currentStage = stage;
        if (stage == GameStage.TitleScene)
        {
            titleSceneVirtualCamera.m_Priority = 10;
            makingVirtualCamera.m_Priority = 1;
            headUpVirtualCamera.m_Priority = 1;

            titleCanvasGroup.DOFade(1f, 1f);
        }
        if (stage == GameStage.Making)
        {
            titleSceneVirtualCamera.m_Priority = 1;
            makingVirtualCamera.m_Priority = 10;
            headUpVirtualCamera.m_Priority = 1;

            makingCanvasGroup.DOFade(1f, 1f);
            machine.StartMachine();
            hand.allowInput = true;
            StartCoroutine(BlockInput(3f));
        }
        if (stage == GameStage.HeadUp)
        {
            titleSceneVirtualCamera.m_Priority = 1;
            makingVirtualCamera.m_Priority = 1;
            headUpVirtualCamera.m_Priority = 10;

            headUpCanvasGroup.DOFade(1f, 1f);
            StartCoroutine(BlockInput(1f));


        }
    }
}