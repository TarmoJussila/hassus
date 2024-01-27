using TMPro;
using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        Resolution[] resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        foreach (Resolution resolution in resolutions)
        {
            string text = resolution.width + "x" + resolution.height + " " + resolution.refreshRate + "Hz";
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData
            {
                text = text
            });
        }
    }

    public void SelectResolution(int index)
    {
        Debug.Log($"Selected: {resolutionDropdown.options[index].text}");
    }
}
