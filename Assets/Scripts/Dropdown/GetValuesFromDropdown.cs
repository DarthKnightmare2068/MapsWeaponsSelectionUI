using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GetValuesFromDropdown : MonoBehaviour
{
    [SerializeField] private CustomTMPDropdown customDropdown;
    [SerializeField] private TMP_Dropdown dropdown; // Keep for backward compatibility, but prefer CustomTMPDropdown
    
    private void Awake()
    {
        // Try to get CustomTMPDropdown first
        if (customDropdown == null)
        {
            customDropdown = GetComponent<CustomTMPDropdown>();
        }
        
        // Fallback to TMP_Dropdown for backward compatibility
        if (customDropdown == null && dropdown == null)
        {
            dropdown = GetComponent<TMP_Dropdown>();
        }
    }

    private void Start()
    {
        // Subscribe to custom dropdown value changes
        if (customDropdown != null)
        {
            customDropdown.OnValueChanged += OnDropdownValueChanged;
        }
        // Fallback to regular dropdown
        else if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (customDropdown != null)
        {
            customDropdown.OnValueChanged -= OnDropdownValueChanged;
        }
        else if (dropdown != null)
        {
            dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }
    }

    // Handle dropdown value changes
    private void OnDropdownValueChanged(int value)
    {
        // Value can be -1 for unselected state (CustomTMPDropdown) or 0+ for regular dropdown
        if (value == -1)
        {
            Debug.Log("Dropdown: No selection (placeholder active)");
            return;
        }
        
        Debug.Log("Dropdown selected index: " + value);
        
        if (customDropdown != null && customDropdown.Dropdown != null)
        {
            if (value >= 0 && value < customDropdown.Dropdown.options.Count)
            {
                Debug.Log("Selected option text: " + customDropdown.Dropdown.options[value].text);
            }
        }
        else if (dropdown != null)
        {
            if (value >= 0 && value < dropdown.options.Count)
            {
                Debug.Log("Selected option text: " + dropdown.options[value].text);
            }
        }
    }

    // Get the selected value when user picks an option
    public void GetValues()
    {
        if (customDropdown != null)
        {
            if (!customDropdown.HasSelection())
            {
                Debug.Log("Dropdown: No selection made (placeholder active)");
                return;
            }
            
            int selectedIndex = customDropdown.Value;
            Debug.Log("Dropdown selected index: " + selectedIndex);
            Debug.Log("Selected option text: " + customDropdown.GetSelectedText());
        }
        else if (dropdown != null)
        {
            if (dropdown.options.Count == 0) return;

            int selectedIndex = dropdown.value;
            Debug.Log("Dropdown selected index: " + selectedIndex);
            Debug.Log("Selected option text: " + dropdown.options[selectedIndex].text);
        }
    }
}
