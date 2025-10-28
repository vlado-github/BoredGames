using Assets.Scripts.GamePlay;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RequiredWinsHandler : MonoBehaviour
{
    private Slider slider;

    [SerializeField] public TextMeshProUGUI valueLabel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject sliderObject = GameObject.Find("Slider");
        if (sliderObject != null)
        {
            slider = sliderObject.GetComponent<Slider>();
            if (slider != null)
            {
                slider.onValueChanged.AddListener(OnValueChange);
            }
        }

    }

    private void OnValueChange(float value)
    {
        if (value > 0)
        {
            int intValue = (int)value;
            GameConfiguration.Instance.RequiredNumberOfConsecutiveWins = intValue;
            valueLabel.text = intValue.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
