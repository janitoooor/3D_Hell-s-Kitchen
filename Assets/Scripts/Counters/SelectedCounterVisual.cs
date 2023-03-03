using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter _baseCounter;
    [SerializeField] private GameObject[] _visualGameObjectsArray;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.SelectedCounter == _baseCounter)
            Show();
        else
            Hide();
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in _visualGameObjectsArray)
            visualGameObject.SetActive(true);
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in _visualGameObjectsArray)
            visualGameObject.SetActive(false);
    }
}
