using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _base1PointText;
    [SerializeField] private TextMeshProUGUI _base2PointText;
    [SerializeField] private BaseManager _base1;
    [SerializeField] private BaseManager _base2;

    public void Subscribe()
    {
        BaseManager.onResourceCollected += PointsTextChange;
    }

    private void PointsTextChange(Fraction fraction, int points)
    {
        switch (fraction)
        {
            case Fraction.Yellow:
                _base1PointText.text = $"Yellow: {points}";
                break;
            case Fraction.Green:
                _base2PointText.text = $"Green: {points}";
                break;
            default:
                break;
        }
    }
}
