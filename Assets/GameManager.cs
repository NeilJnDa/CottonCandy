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
    [SerializeField] TMP_Text titleTipText;

    [Header("Making Stage")]
    [SerializeField] private CinemachineVirtualCamera makingVirtualCamera;
    [SerializeField] TMP_Text makingTipText;

    [Header("Head Up Stage")]
    [SerializeField] private CinemachineVirtualCamera headUpVirtualCamera;
    [SerializeField] TMP_Text headUpTipText;


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
            titleTipText.DOFade(0f, 1f);

        }
        if (currentStage == GameStage.Making)
        {
            makingTipText.DOFade(0f, 1f);

        }
        if (currentStage == GameStage.HeadUp)
        {
            headUpTipText.DOFade(0f, 1f);
        }

        //  Enter new stage
        currentStage = stage;
        if (stage == GameStage.TitleScene)
        {
            titleSceneVirtualCamera.m_Priority = 10;
            makingVirtualCamera.m_Priority = 1;
            headUpVirtualCamera.m_Priority = 1;
        }
        if(stage == GameStage.Making)
        {
            titleSceneVirtualCamera.m_Priority = 1;
            makingVirtualCamera.m_Priority = 10;
            headUpVirtualCamera.m_Priority = 1;
        }
        if(stage == GameStage.HeadUp)
        {
            titleSceneVirtualCamera.m_Priority = 1;
            makingVirtualCamera.m_Priority = 1;
            headUpVirtualCamera.m_Priority = 10;
        }
    }
}