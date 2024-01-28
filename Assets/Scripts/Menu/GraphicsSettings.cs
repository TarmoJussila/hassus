using TMPro;
using UnityEngine;

namespace Hassus.Menu
{
    public class GraphicsSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;

        private Resolution[] _resolutions;
        private int _selectedIndex = -1;

        public bool FullScreenEnabled { get; set; }

        private void Start()
        {
            FullScreenEnabled = Screen.fullScreen;
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

        public void OnApplyClick()
        {
            if (_selectedIndex < 0 || _resolutions == null)
            {
                Debug.Log("Skipped options menu apply, no resolution selected");
                return;
            }

            Screen.fullScreen = FullScreenEnabled;
            Resolution res = _resolutions[_selectedIndex];
            Screen.SetResolution(res.width, res.height,
                FullScreenEnabled ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed, res.refreshRateRatio);
        }
    }
}
