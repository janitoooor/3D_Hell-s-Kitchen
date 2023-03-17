using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuccessRecipesUI : MonoBehaviour
{
    private const string SUCCES_TRIGER = "Success";

    [SerializeField] private TextMeshProUGUI _textRecipesAmount;
    [SerializeField] private Animator _iconRecipesAnimator;
    [SerializeField] private Animator _textRecipesAnimator;

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        _textRecipesAmount.text = DeliveryManager.Instance.SuccessfulRecipesAmount.ToString();
        _iconRecipesAnimator.SetTrigger(SUCCES_TRIGER);
        _textRecipesAnimator.SetTrigger(SUCCES_TRIGER);
    }
}