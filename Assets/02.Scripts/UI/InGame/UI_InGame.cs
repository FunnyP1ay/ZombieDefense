using UnityEngine;
using UnityEngine.InputSystem;

public class UI_InGame : MonoBehaviour
{
    public GameObject tabPanel;
    public InputActionAsset inputActions; // �ν����Ϳ��� ������ InputActionAsset

    private InputAction inputAction;

    private void OnEnable()
    {
        // "Player" Action Map���� "Tab" �׼� ��������
        var actionMap = inputActions.FindActionMap("Player", true);
        inputAction = actionMap.FindAction("Tab", true);

        inputAction.performed += TogglePanel;
        inputAction.Enable();
    }

    private void TogglePanel(InputAction.CallbackContext context)
    {
        tabPanel.SetActive(!tabPanel.activeSelf);
    }

    private void OnDisable()
    {
        inputAction.Disable();
        inputAction.performed -= TogglePanel;
    }
}