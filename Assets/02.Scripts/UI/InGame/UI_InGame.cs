using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Lean.Pool;

public class UI_InGame : MonoBehaviour
{
    public GameObject tabPanel;
    private Animator tabPanelAnimator;
    public InputActionAsset inputActions; // 인스펙터에서 참조할 InputActionAsset
    private InputAction inputAction;

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

    public LayerMask FarmBuildLayer;
    private void OnEnable()
    {
        // "Player" Action Map에서 "Tab" 액션 가져오기
        var actionMap = inputActions.FindActionMap("Player", true);
        inputAction = actionMap.FindAction("Tab", true);

        inputAction.performed += TogglePanel;
        inputAction.Enable();
    }
    private void Start()
    {
        tabPanelAnimator = tabPanel.GetComponent<Animator>();
        Tier1FarmButton.onClick.AddListener(() => CreatePrefab(Tier1Farm));
        Tier2FarmButton.onClick.AddListener(() => CreatePrefab(Tier2Farm));
        RandomFarmButton.onClick.AddListener(() => CreatePrefab(RandomFarm));
        Tier1TreeButton.onClick.AddListener(() => CreatePrefab(Tier1Tree));
        Tier2TreeButton.onClick.AddListener(() => CreatePrefab(Tier2Tree));
        RandomTreeButton.onClick.AddListener(() => CreatePrefab(RandomTree));
    }
    private void Update()
    {
        if (currentPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, FarmBuildLayer))
            {
                currentPrefab.transform.position = hit.point;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (currentPrefab.isBuildArea)
                {
                    currentPrefab.BuildObject(currentPrefab);
                    LeanPool.Despawn(currentPrefab.gameObject);
                    currentPrefab = null;
                }
                else
                {
                    LeanPool.Despawn(currentPrefab.gameObject);
                    currentPrefab = null;
                    print("건설불가");
                }
            }
        }
    }
    private void TogglePanel(InputAction.CallbackContext context)
    {
        tabPanel.SetActive(!tabPanel.activeSelf);
        tabPanelAnimator.SetTrigger("IsTab");
    }
    private void CreatePrefab(FarmAsset prefab)
    {
        if (currentPrefab == null)
        {
            GameObject spawnedObject = LeanPool.Spawn(prefab.gameObject);
            currentPrefab = spawnedObject.GetComponent<FarmAsset>();
        }
    }

    private void OnDisable()
    {
        inputAction.Disable();
        inputAction.performed -= TogglePanel;
    }
}