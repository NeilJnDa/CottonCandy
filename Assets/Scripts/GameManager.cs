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
    [SerializeField]
    private Transform playerTransform;

    [Header("Title Stage")]
    [SerializeField] private CinemachineVirtualCamera titleSceneVirtualCamera;
    [SerializeField] CanvasGroup titleCanvasGroup;

    [Header("Making Stage")]
    [SerializeField] private CinemachineVirtualCamera makingVirtualCamera;
    [SerializeField] CanvasGroup makingCanvasGroup;
    [SerializeField] CandyMachine machine;
    [SerializeField] Hand hand;
    [SerializeField] Transform playerMakingTransform;

    [Header("Head Up Stage")]
    [SerializeField] CanvasGroup headUpCanvasGroup;
    [SerializeField] Transform playerHeadUpTransform;
    [SerializeField] Transform farSky;
    [SerializeField] float farSkyXYRange = 10f;
    private void Start()
    {
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
        titleSceneVirtualCamera.m_Priority = 1;
        makingVirtualCamera.m_Priority = 1;
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f);


        makingCanvasGroup.alpha = 0f;
        titleCanvasGroup.alpha = 0f;
        headUpCanvasGroup.alpha = 0f;

        machine.StopMachine();
        hand.allowInput = false;
        TransitToStage(GameStage.TitleScene);
        StopAllCoroutines();
        StartCoroutine(BlockInput(2f));


    }

    IEnumerator BlockInput(float duration)
    {
        allowInput = false;
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
        else if (currentStage == GameStage.Making)
        {
            if (Input.anyKeyDown)
            {
                TransitToStage(GameStage.HeadUp);
            }
        }
        else if(currentStage == GameStage.HeadUp)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                hand.DeleteOldCandy();
                hand.AttachNewCandy();
                TransitToStage(GameStage.Making);
            }
            else if (Input.anyKeyDown)
            {
                // Fly the candy
                StopAllCoroutines();            
                StartCoroutine(FlyCandy());
            }
        }
    }

    IEnumerator FlyCandy()
    {
        allowInput = false;
        hand.cottonCandy.transform.parent = farSky.transform;
        var newPos = farSky.transform.position + new Vector3(Random.Range(-farSkyXYRange, farSkyXYRange), 0, Random.Range(-farSkyXYRange, farSkyXYRange));
        hand.cottonCandy.transform.DOMove(newPos, 2f);
        hand.cottonCandy.transform.DOScale(farSky.transform.localScale, 2f);
        yield return new WaitForSeconds(2f);
        hand.cottonCandy.transform.DOScale(farSky.transform.localScale * 2f, 1f).SetEase(Ease.InElastic);
        yield return new WaitForSeconds(2f);

        hand.AttachNewCandy();
        TransitToStage(GameStage.Making);
        allowInput = true;
    }
    public void TransitToStage(GameStage stage)
    {
        //  Leave current stage
        if (currentStage == GameStage.TitleScene)
        {
            titleCanvasGroup.DOFade(0f, 1f);

        }
        else if(currentStage == GameStage.Making)
        {
            makingCanvasGroup.DOFade(0f, 1f);
            machine.StopMachine();
            
        }
        else if(currentStage == GameStage.HeadUp)
        {
            headUpCanvasGroup.DOFade(0f, 1f);
        }

        //  Enter new stage
        currentStage = stage;

        if (stage == GameStage.TitleScene)
        {
            titleSceneVirtualCamera.m_Priority = 10;
            makingVirtualCamera.m_Priority = 1;

            titleCanvasGroup.DOFade(1f, 1f);
        }
        else if(stage == GameStage.Making)
        {
            titleSceneVirtualCamera.m_Priority = 1;
            makingVirtualCamera.m_Priority = 10;

            makingCanvasGroup.DOFade(1f, 1f);
            machine.StartMachine();
            hand.allowInput = true;

            playerTransform.DOMove(playerMakingTransform.position, 1f);
            playerTransform.DORotateQuaternion(playerMakingTransform.rotation, 1f);

            StopAllCoroutines();
            StartCoroutine(BlockInput(2f));
        }
        else if(stage == GameStage.HeadUp)
        {
            playerTransform.DOMove(playerHeadUpTransform.position, 1f);
            playerTransform.DORotateQuaternion(playerHeadUpTransform.rotation, 1f);

            headUpCanvasGroup.DOFade(1f, 1f);
            StopAllCoroutines();
            StartCoroutine(BlockInput(1f));

        }
    }
}