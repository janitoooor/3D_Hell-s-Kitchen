using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PorgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject _hasProgressGameObject;
    [SerializeField] private Image _barImage;

    private IHasProgress _hasProgress;

    private void Awake()
    {
        if (!_hasProgressGameObject.TryGetComponent(out _hasProgress))
            Debug.LogError($"Game Object {_hasProgressGameObject} doesn't have a component that implements IHasProgress");
    }

    private void Start()
    {
        _hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        _barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        _barImage.fillAmount = e.ProgressNormalized;

        if (e.ProgressNormalized == 0f || e.ProgressNormalized == 1f)
            Hide();
        else
            Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
