using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "Popup";

    [Space]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _messageText;
    [Space]
    [Header("Colors Backgrounds")]
    [Space]
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failedColor;
    [Space]
    [Header("Sprites Icons")]
    [Space]
    [SerializeField] private Sprite _failedSprite;
    [SerializeField] private Sprite _successSprite;
    [Space]
    [Header("Message Texts")]
    [Space]
    [Multiline][SerializeField] private string _failedText;
    [Multiline][SerializeField] private string _successText;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        Failed();
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        Success();
    }

    private void Failed()
    {
        ChangeResult(_failedColor, _failedSprite, _failedText);
    }

    private void Success()
    {
        ChangeResult(_successColor, _successSprite, _successText);
    }

    private void ChangeResult(Color color, Sprite sprite, string text)
    {
        gameObject.SetActive(true);
        _backgroundImage.color = color;
        _iconImage.sprite = sprite;
        _messageText.text = text;
        _animator.SetTrigger(POPUP);
    }
}
