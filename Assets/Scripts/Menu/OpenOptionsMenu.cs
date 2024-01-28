using TMPro;
using UnityEngine;

namespace Hassus.Menu
{
    public class OpenOptionsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject optionsMenu;

        private TextMeshProUGUI _text;
    
        private void Start()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Toggle()
        {
            bool menuVisible = !optionsMenu.activeSelf;
            optionsMenu.SetActive(menuVisible);
            _text.text = menuVisible ? "Close" : "Options";
        }
    }
}
