using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GetValuesFromDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private string placeholderText = "Choose level"; // Shown until user picks an option
    [SerializeField] private bool showPlaceholderOnStart = true; // Show placeholder in caption only (not in list)

    private bool isPlaceholderActive = false; // Track if we're showing placeholder

    private void Awake()
    {
        // Auto-wire the TMP_Dropdown if not assigned
        if (dropdown == null)
        {
            dropdown = GetComponent<TMP_Dropdown>();
        }
    }

    private void Start()
    {
        if (dropdown == null) return;

        if (showPlaceholderOnStart)
        {
            // Show placeholder in caption only, without adding it to the options list
            SetPlaceholderCaption();
        }

        // Subscribe to value changes to handle when user selects an option
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (dropdown != null)
        {
            dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }
    }

    private void LateUpdate()
    {
        // Keep placeholder text visible until user makes a selection
        if (isPlaceholderActive && dropdown != null && dropdown.captionText != null)
        {
            // Continuously override caption text to keep placeholder visible
            // This prevents dropdown from updating caption automatically
            if (dropdown.captionText.text != placeholderText)
            {
                dropdown.captionText.text = placeholderText;
            }
        }
    }

    // Handle dropdown value changes
    private void OnDropdownValueChanged(int value)
    {
        // If placeholder was active, disable it and let dropdown show normal selection
        if (isPlaceholderActive)
        {
            isPlaceholderActive = false;
            // Dropdown will automatically update caption to show selected option
        }
    }

    // Set placeholder text in caption without adding it to the options list
    private void SetPlaceholderCaption()
    {
        if (dropdown == null || dropdown.captionText == null) return;

        // Set caption text to placeholder
        dropdown.captionText.text = placeholderText;
        isPlaceholderActive = true;

        // Set value to 0 but prevent caption from updating
        // LateUpdate will keep overriding the caption text
        if (dropdown.options.Count > 0)
        {
            dropdown.SetValueWithoutNotify(0);
        }
    }

    public void GetValues()
    {
        // Get the selected value when user picks an option
        if (dropdown == null) return;
        
        if (dropdown.value < 0 || dropdown.options.Count == 0)
        {
            // No valid selection
            return;
        }

        int selectedIndex = dropdown.value; // Real selection (0 or higher)
        Debug.Log("Dropdown selected index: " + selectedIndex);
        Debug.Log("Selected option text: " + dropdown.options[selectedIndex].text);
    }
    
    // Note: Colors/hover visuals are controlled by the Dropdown Template's Item Selectable/Image.
}
