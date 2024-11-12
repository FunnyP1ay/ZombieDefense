using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Lean.Pool;

public class UI_InGame : MonoBehaviour
{
    public GameObject tabPanel;
    private Animator tabPanelAnimator;
    public InputActionAsset inputActions; // �ν����Ϳ��� ������ InputActionAsset
    private InputAction inputAction;

    [Header("���� ��ư��")]
    public Button Tier1FarmButton;
    public Button Tier2FarmButton;
    public Button RandomFarmButton;
    [Header("���� ��ư��")]
    public Button Tier1TreeButton;
    public Button Tier2TreeButton;
    public Button RandomTreeButton;

    [Header("�����յ�")]
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
        // "Player" Action Map���� "Tab" �׼� ��������
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
                    print("�Ǽ��Ұ�");
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