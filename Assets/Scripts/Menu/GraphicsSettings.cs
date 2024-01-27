using TMPro;
using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] _resolutions;
    private int _selectedIndex = -1;

    private void Start()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        foreach (Resolution resolution in _resolutions)
        {
            float rate = resolution.refreshRateRatio.numerator / (float)resolution.refreshRateRatio.denominator;
            string text = $"{resolution.width} x {resolution.height} {rate:0.00} Hz";
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData
            {
                text = text
            });
        }
    }

    public void SelectResolution(int index)
    {
        _selectedIndex = index;
        Debug.Log($"Selected: {resolutionDropdown.options[index].text}");
    }
}
