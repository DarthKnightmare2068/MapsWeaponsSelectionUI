using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Dropdown wrapper that supports an unselected state (-1) and a placeholder
public class CustomTMPDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private string placeholderText = "Choose level";
    [SerializeField] private bool showPlaceholderOnStart = true;
    
    private int internalValue = -1; // -1 means unselected
    private bool isInitialized = false;
    
    // Expose dropdown properties for easy access
    public TMP_Dropdown Dropdown => dropdown;
    
    // Custom value property that supports -1
    public int Value
    {
        get => internalValue;
        set
        {
            if (internalValue == value) return;
            
            internalValue = value;
            
            if (dropdown == null)
            {
                OnValueChanged?.Invoke(internalValue);
                return;
            }

            if (value == -1)
            {
                // Unselected state - show placeholder
                if (dropdown.captionText != null)
                {
                    dropdown.captionText.text = placeholderText;
                }
                // Ensure TMP holds a harmless index without firing events; we'll override the caption
                if (dropdown.options.Count > 0)
                {
                    dropdown.SetValueWithoutNotify(0);
                }
                // Don't set dropdown.value, keep it at 0 internally but hide checkmarks
            }
            else
            {
                // Valid selection - update dropdown normally
                if (value >= 0 && value < dropdown.options.Count)
                {
                    dropdown.value = value;
                }
            }
            
            OnValueChanged?.Invoke(internalValue);
        }
    }
    
    // Custom event that includes -1 as valid value
    public System.Action<int> OnValueChanged;
    
    private void Awake()
    {
        if (dropdown == null)
        {
            dropdown = GetComponent<TMP_Dropdown>();
        }
        
        if (dropdown == null)
        {
            Debug.LogError("CustomTMPDropdown: TMP_Dropdown component not found!");
            return;
        }
        
        // Subscribe to dropdown's value changes
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        
        if (showPlaceholderOnStart)
        {
            // Set to unselected state
            Value = -1;
        }
        else
        {
            // Use dropdown's current value
            internalValue = dropdown.value;
        }
        
        isInitialized = true;
    }
    
    private void OnDestroy()
    {
        if (dropdown != null)
        {
            dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }
    }
    
    private void LateUpdate()
    {
        if (!isInitialized || dropdown == null) return;
        
        // If in unselected state, maintain placeholder text and hide checkmarks
        if (internalValue == -1)
        {
            // Keep placeholder text visible
            if (dropdown.captionText != null && dropdown.captionText.text != placeholderText)
            {
                dropdown.captionText.text = placeholderText;
            }
            
            // Hide checkmarks when the runtime list is open
            HideAllCheckmarks();
        }
    }
    
    // Handle when user selects an option from the dropdown
    private void OnDropdownValueChanged(int value)
    {
        // User made a selection - update our internal value
        if (internalValue == -1)
        {
            // Was unselected, now user selected something
            internalValue = value;
            OnValueChanged?.Invoke(internalValue);
        }
        else if (internalValue != value)
        {
            // User changed selection
            internalValue = value;
            OnValueChanged?.Invoke(internalValue);
        }
    }
    
    // Hide all checkmarks in dropdown items
    private void HideAllCheckmarks()
    {
        if (dropdown == null) return;
        
        // If still unselected, force caption to placeholder immediately to avoid TMP overriding it on open
        if (internalValue == -1 && dropdown.captionText != null && dropdown.captionText.text != placeholderText)
        {
            dropdown.captionText.text = placeholderText;
        }
        
        // Find the instantiated runtime list named "Dropdown List"
        Transform list = dropdown.transform.Find("Dropdown List");
        if (list == null || !list.gameObject.activeInHierarchy)
        {
            // Sometimes the list is created as a sibling under the same parent/canvas
            var parent = dropdown.transform.parent;
            if (parent != null)
            {
                list = parent.Find("Dropdown List");
            }
        }
        if (list == null || !list.gameObject.activeInHierarchy) return;
        
        Transform content = list.Find("Viewport/Content");
        if (content == null) return;
        
        // In placeholder mode, allow toggles to switch off, but don't force them off here
        if (internalValue == -1)
        {
            var toggleGroup = list.GetComponentInChildren<ToggleGroup>();
            if (toggleGroup != null) toggleGroup.allowSwitchOff = true;
        }
        
        // When unselected, make sure the dropdown's current value differs from index 0
        // so that clicking the first item will fire onValueChanged.
        if (internalValue == -1 && dropdown.options.Count > 1 && dropdown.value == 0)
        {
            dropdown.SetValueWithoutNotify(1);
        }
        
        for (int i = 0; i < content.childCount; i++)
        {
            Transform item = content.GetChild(i);
            Transform checkmark = item.Find("CheckPoint") ?? item.Find("Item Checkmark") ?? item.Find("Checkmark");
            if (checkmark != null)
            {
                checkmark.gameObject.SetActive(false);
            }
            
            // Do not accept selection here; rely on onValueChanged to capture explicit user clicks
        }
    }
    
    // Reset dropdown to unselected state (-1)
    public void ResetToPlaceholder()
    {
        Value = -1;
    }
    
    // Check if dropdown has a valid selection (not -1)
    public bool HasSelection()
    {
        return internalValue >= 0;
    }
    
    // Get the selected option text, or placeholder if unselected
    public string GetSelectedText()
    {
        if (internalValue == -1)
        {
            return placeholderText;
        }
        
        if (dropdown != null && internalValue >= 0 && internalValue < dropdown.options.Count)
        {
            return dropdown.options[internalValue].text;
        }
        
        return string.Empty;
    }
}

