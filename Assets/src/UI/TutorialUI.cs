using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyUseText;
    [SerializeField] private TextMeshProUGUI keyPauseText;
    private GameInput gameInput;
    private GameManager gameManager;

    // tengo que escuchar el evento para cuando cambian las teclas asi actualizo esto
    private void Start()
    {
        gameInput = GameInput.Instance;
        gameManager = GameManager.Instance;
        gameInput.OnBindingRebind += GameInput_OnBindingRebind;
        gameManager.OnStateChanged += GameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (gameManager.IsCountdownActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = gameInput.GetBindingText(GameInput.Binding.MOVE_UP);
        keyMoveLeftText.text = gameInput.GetBindingText(GameInput.Binding.MOVE_LEFT);
        keyMoveDownText.text = gameInput.GetBindingText(GameInput.Binding.MOVE_DOWN);
        keyMoveRightText.text = gameInput.GetBindingText(GameInput.Binding.MOVE_RIGHT);
        keyInteractText.text = gameInput.GetBindingText(GameInput.Binding.INTERACT);
        keyUseText.text = gameInput.GetBindingText(GameInput.Binding.USE);
        keyPauseText.text = gameInput.GetBindingText(GameInput.Binding.PAUSE);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    // esto va a reemplazar al timer que habia hecho en el estado de game Manager "waiting to start"
    // La idea es que aparezca el tutorial y que empiece el conteo cuando apreto "interact"
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
