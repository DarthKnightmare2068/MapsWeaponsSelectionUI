using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GetValuesFromDropdown : MonoBehaviour
{
    [SerializeField] private CustomTMPDropdown customDropdown;
    [SerializeField] private TMP_Dropdown dropdown; // Keep for backward compatibility, but prefer CustomTMPDropdown
    [SerializeField] private TextMeshProUGUI infoText; // Single text component to display map info
    
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
            // Clear info text when unselected
            if (infoText != null)
            {
                infoText.text = "";
            }
            return;
        }
        
        // Get the selected option text
        string selectedText = GetSelectedOptionText();
        if (string.IsNullOrEmpty(selectedText))
        {
            return;
        }
        
        // Update info text based on selection
        UpdateInfoText(selectedText);
    }
    
    // Get the selected option text from dropdown
    private string GetSelectedOptionText()
    {
        if (customDropdown != null)
        {
            return customDropdown.GetSelectedText();
        }
        else if (dropdown != null && dropdown.options.Count > 0 && dropdown.value >= 0 && dropdown.value < dropdown.options.Count)
        {
            return dropdown.options[dropdown.value].text;
        }
        return string.Empty;
    }
    
    // Update the info text based on the selected option
    private void UpdateInfoText(string selectedOptionText)
    {
        if (infoText == null) return;
        
        string combinedText = "";
        
        // Use localization key for comparison instead of hardcoded text
        if (customDropdown != null)
        {
            string localizationKey = customDropdown.GetSelectedLocalizationKey();
            if (localizationKey == "mapLevel.Normal")
            {
                // Normal level: Small map, 3 players, Unlimited kit
                string mapSize = GetLocalizedString("MapInfo", "mapInfo.smallMap") + " (20'-30')";
                string minPlayers = GetLocalizedString("MapInfo", "mapInfo.minTeamPlayer") + ": 3";
                // Note: unlimitedKit key doesn't exist in table - using hardcoded text or add key to table
                string kitType = "Unlimited Kit"; // TODO: Add mapInfo.unlimitedKit key to MapInfo table
                combinedText = $"{mapSize}\n\n{minPlayers}\n\n{kitType}";
            }
            else if (localizationKey == "mapLevel.Superhard")
            {
                // Superhard level: Big map, 5 players, Limited kit
                string mapSize = GetLocalizedString("MapInfo", "mapInfo.bigMap") + " (30'-40')";
                string minPlayers = GetLocalizedString("MapInfo", "mapInfo.minTeamPlayer") + ": 5";
                string kitType = GetLocalizedString("MapInfo", "mapInfo.limitedMemberSize");
                combinedText = $"{mapSize}\n\n{minPlayers}\n\n{kitType}";
            }
            else
            {
                // Default fallback
                string mapSize = GetLocalizedString("MapInfo", "mapInfo.bigMap");
                string minPlayers = GetLocalizedString("MapInfo", "mapInfo.minTeamPlayer") + ":";
                combinedText = $"{mapSize}\n\n{minPlayers}\n\n{GetLocalizedString("MapInfo", "mapInfo.limitedMemberSize")}";
            }
        }
        else
        {
            // Fallback for regular dropdown - check localized strings
            try
            {
                string normalText = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("Map Level", "mapLevel.Normal");
                string superhardText = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString("Map Level", "mapLevel.Superhard");
                
                if (selectedOptionText.Contains(normalText) || selectedOptionText.Contains("City Disaster"))
                {
                    string mapSize = GetLocalizedString("MapInfo", "mapInfo.smallMap") + " (20'-30')";
                    string minPlayers = GetLocalizedString("MapInfo", "mapInfo.minTeamPlayer") + ": 3";
                    string kitType = "Unlimited Kit"; // TODO: Add mapInfo.unlimitedKit key to MapInfo table
                    combinedText = $"{mapSize}\n\n{minPlayers}\n\n{kitType}";
                }
                else if (selectedOptionText.Contains(superhardText) || selectedOptionText.Contains("Universe Disaster"))
                {
                    string mapSize = GetLocalizedString("MapInfo", "mapInfo.bigMap") + " (30'-40')";
                    string minPlayers = GetLocalizedString("MapInfo", "mapInfo.minTeamPlayer") + ": 5";
                    string kitType = GetLocalizedString("MapInfo", "mapInfo.limitedMemberSize");
                    combinedText = $"{mapSize}\n\n{minPlayers}\n\n{kitType}";
                }
                else
                {
                    string mapSize = GetLocalizedString("MapInfo", "mapInfo.bigMap");
                    string minPlayers = GetLocalizedString("MapInfo", "mapInfo.minTeamPlayer") + ":";
                    combinedText = $"{mapSize}\n\n{minPlayers}\n\n{GetLocalizedString("MapInfo", "mapInfo.limitedMemberSize")}";
                }
            }
            catch
            {
                // Fallback to original hardcoded check
                if (selectedOptionText.Contains("City Disaster (Normal)"))
                {
                    string mapSize = GetLocalizedString("MapInfo", "mapInfo.smallMap") + " (20'-30')";
                    string minPlayers = GetLocalizedString("MapInfo", "mapInfo.minTeamPlayer") + ": 3";
                    string kitType = "Unlimited Kit"; // TODO: Add mapInfo.unlimitedKit key to MapInfo table
                    combinedText = $"{mapSize}\n\n{minPlayers}\n\n{kitType}";
                }
                else if (selectedOptionText.Contains("Universe Disaster (Superhard)"))
                {
                    string mapSize = GetLocalizedString("MapInfo", "mapInfo.bigMap") + " (30'-40')";
                    string minPlayers = GetLocalizedString("MapInfo", "mapInfo.minTeamPlayer") + ": 5";
                    string kitType = GetLocalizedString("MapInfo", "mapInfo.limitedMemberSize");
                    combinedText = $"{mapSize}\n\n{minPlayers}\n\n{kitType}";
                }
                else
                {
                    string mapSize = GetLocalizedString("MapInfo", "mapInfo.bigMap");
                    string minPlayers = GetLocalizedString("MapInfo", "mapInfo.minTeamPlayer") + ":";
                    combinedText = $"{mapSize}\n\n{minPlayers}\n\n{GetLocalizedString("MapInfo", "mapInfo.limitedMemberSize")}";
                }
            }
        }
        
        infoText.text = combinedText;
    }
    
    // Helper method to get localized string synchronously
    private string GetLocalizedString(string tableName, string entryKey)
    {
        try
        {
            return UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedString(tableName, entryKey);
        }
        catch
        {
            Debug.LogWarning($"GetValuesFromDropdown: Could not find key '{entryKey}' in table '{tableName}'");
            return entryKey; // Return key as fallback
        }
    }

    // Get the selected value when user picks an option
    public void GetValues()
    {
        if (customDropdown != null)
        {
            if (!customDropdown.HasSelection())
            {
                return;
            }
            
            // Get selected values (logs removed for production)
            int selectedIndex = customDropdown.Value;
            string selectedText = customDropdown.GetSelectedText();
        }
        else if (dropdown != null)
        {
            if (dropdown.options.Count == 0) return;

            // Get selected values (logs removed for production)
            int selectedIndex = dropdown.value;
            string selectedText = dropdown.options[selectedIndex].text;
        }
    }
}
