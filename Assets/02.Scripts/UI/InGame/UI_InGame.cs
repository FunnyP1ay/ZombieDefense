using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Lean.Pool;
using UnityEngine.SceneManagement;

public class UI_InGame : MonoBehaviour
{
    public GameObject tabPanel;
    public GameObject escPanel;
    private Animator tabPanelAnimator;

    public InputActionAsset inputActions; // 인스펙터에서 참조할 InputActionAsset
    private InputAction tabAction;
    private InputAction escAction;

    [Header("농장 버튼들")]
    public Button Tier1FarmButton;
    public Button Tier2FarmButton;
    public Button RandomFarmButton;
    [Header("나무 버튼들")]
    public Button Tier1TreeButton;
    public Button Tier2TreeButton;
    public Button RandomTreeButton;

    [Header("프리팹들")]
    public FarmAsset Tier1Farm;
    public FarmAsset Tier2Farm;
    public FarmAsset RandomFarm;
    public FarmAsset Tier1Tree;
    public FarmAsset Tier2Tree;
    public FarmAsset RandomTree;
    public FarmAsset currentPrefab;
    [Header("게임 패배 패널")]
    public GameObject UI_GameLose;
    public LayerMask FarmBuildLayer;
    private void Awake()
    {
        GameManager.Instance.UI_InGame = this;
    }
    private void OnEnable()
    {
        // "Player" Action Map에서 "Tab" 액션 가져오기
        var actionMap = inputActions.FindActionMap("Player", true);

        tabAction = actionMap.FindAction("Tab", true);
        tabAction.performed += ToggleTabPanel;
        tabAction.Enable();

        escAction = actionMap.FindAction("Esc", true);
        escAction.performed += ToggleEscPanel;
        escAction.Enable();
    }
    private void Start()
    {
        tabPanelAnimator = tabPanel.GetComponent<Animator>();
        Tier1FarmButton.onClick.AddListener(() => CreateFarmPrefab(Tier1Farm));
        Tier2FarmButton.onClick.AddListener(() => CreateFarmPrefab(Tier2Farm));
        RandomFarmButton.onClick.AddListener(() => CreateFarmPrefab(RandomFarm));
       // Tier1TreeButton.onClick.AddListener(() => CreateFarmPrefab(Tier1Tree));
       // Tier2TreeButton.onClick.AddListener(() => CreateFarmPrefab(Tier2Tree));
       // RandomTreeButton.onClick.AddListener(() => CreateFarmPrefab(RandomTree));

    }
    private void Update()
    {
        if (currentPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, FarmBuildLayer))
            {
                currentPrefab.transform.position = hit.point ;
            }
            // 설치 가능 여부 확인
            if (currentPrefab.CheckBuildArea() && Input.GetMouseButtonDown(0))
            {
                currentPrefab.BuildObject(currentPrefab);
                LeanPool.Despawn(currentPrefab.gameObject);
                currentPrefab = null;
            }
            else if (!currentPrefab.isBuildArea && Input.GetMouseButtonDown(0))
            {
                LeanPool.Despawn(currentPrefab.gameObject);
                currentPrefab = null;
                print("건설불가");
            }

        }
    }
    private void ToggleTabPanel(InputAction.CallbackContext context)
    {
        tabPanel.SetActive(!tabPanel.activeSelf);
        tabPanelAnimator.SetTrigger("IsTab");
    }
    private void ToggleEscPanel(InputAction.CallbackContext context)
    {
        escPanel.SetActive(!escPanel.activeSelf);
    }
    private void CreateFarmPrefab(FarmAsset prefab)
    {
        if (currentPrefab == null&& prefab.BuildPrice<GameManager.Instance.playerCityData.CityMoney)
        {
            GameObject spawnedObject = LeanPool.Spawn(prefab.gameObject);
            currentPrefab = spawnedObject.GetComponent<FarmAsset>();
        }
    }
    public void GameLose()
    {
        UI_GameLose.SetActive(true);
    }
    public void MainMenuLoad()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDisable()
    {
        tabAction.performed -= ToggleTabPanel;
        tabAction.Disable();

        escAction.performed -= ToggleEscPanel;
        escAction.Disable();
    }
}