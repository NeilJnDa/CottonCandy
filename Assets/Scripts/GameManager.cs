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

    [Header("Title Stage")]
    [SerializeField] private CinemachineVirtualCamera titleSceneVirtualCamera;
    [SerializeField] CanvasGroup titleCanvasGroup;

    [Header("Making Stage")]
    [SerializeField] private CinemachineVirtualCamera makingVirtualCamera;
    [SerializeField] CanvasGroup makingCanvasGroup;

    [Header("Head Up Stage")]
    [SerializeField] private CinemachineVirtualCamera headUpVirtualCamera;
    [SerializeField] CanvasGroup headUpCanvasGroup;

    private void Start()
    {
        TransitToStage(GameStage.TitleScene);
    }

    private void Update()
    {
        if (currentStage == GameStage.TitleScene)
        {
            if (Input.anyKeyDown)
            {
                TransitToStage(GameStage.Making);
            }                 
        }
        if (currentStage == GameStage.Making)
        {

        }
        if (currentStage == GameStage.HeadUp)
        {

        }
    }

    public void TransitToStage(GameStage stage)
    {
        if (currentStage == stage) return;

        //  Leave current stage
        if(currentStage == GameStage.TitleScene)
        {
            titleCanvasGroup.DOFade(0f, 1f);

        }
        if (currentStage == GameStage.Making)
        {
            makingCanvasGroup.DOFade(0f, 1f);

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
        if(stage == GameStage.Making)
        {
            titleSceneVirtualCamera.m_Priority = 1;
            makingVirtualCamera.m_Priority = 10;
            headUpVirtualCamera.m_Priority = 1;

            makingCanvasGroup.DOFade(1f, 1f);

        }
        if (stage == GameStage.HeadUp)
        {
            titleSceneVirtualCamera.m_Priority = 1;
            makingVirtualCamera.m_Priority = 1;
            headUpVirtualCamera.m_Priority = 10;

            headUpCanvasGroup.DOFade(1f, 1f);

        }
    }
}