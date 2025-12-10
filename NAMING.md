# Codebase Naming Summary

This document lists all classes, functions, and variables found in the codebase.

---

## ChooseLevelWarning.cs

### Class: `ChooseLevelWarning`
**Inherits:** `MonoBehaviour`

#### Variables:
- `dropdown` (private CustomTMPDropdown)
- `canvasGroup` (private CanvasGroup)
- `fadeCoroutine` (private Coroutine)
- `currentMapCard` (private MapCardUI)
- `warningText` (SerializeField TextMeshProUGUI)
- `displayDuration` (SerializeField float)
- `fadeDuration` (SerializeField float)
- `ENGLISH_TEXT` (private const string)
- `VIETNAMESE_TEXT` (private const string)

#### Functions:
- `Awake()` (void)
- `OnDestroy()` (void)
- `OnDropdownValueChanged(int value)` (private void)
- `CheckLevelSelection(MapCardUI mapCard = null)` (public bool)
- `ShowWarning()` (private void)
- `UpdateLocalizedText()` (private void)
- `FadeOutAfterDelay()` (private IEnumerator)
- `HideWarning()` (public void)

---

## Dropdown/CustomTMPDropdown.cs

### Class: `CustomTMPDropdown`
**Inherits:** `MonoBehaviour`

#### Variables:
- `dropdown` (SerializeField TMP_Dropdown)
- `placeholderText` (SerializeField string)
- `showPlaceholderOnStart` (SerializeField bool)
- `useLocalization` (SerializeField bool)
- `placeholderLocalizationKey` (SerializeField string)
- `localizationTableName` (SerializeField string)
- `internalValue` (private int)
- `isInitialized` (private bool)
- `localizedPlaceholder` (private LocalizedString)
- `currentPlaceholderText` (private string)
- `Dropdown` (public TMP_Dropdown property)
- `Value` (public int property)
- `OnValueChanged` (public System.Action<int>)

#### Functions:
- `Awake()` (void)
- `OnPlaceholderLocalized(string translatedText)` (private void)
- `LocalizeDropdownOptions()` (private void)
- `LateUpdate()` (void)
- `OnDropdownValueChanged(int value)` (private void)
- `HideAllCheckmarks()` (private void)
- `ResetToPlaceholder()` (public void)
- `HasSelection()` (public bool)
- `GetSelectedText()` (public string)
- `GetSelectedLocalizationKey()` (public string)
- `OnDestroy()` (void)

---

## Dropdown/GetValuesFromDropdown.cs

### Class: `GetValuesFromDropdown`
**Inherits:** `MonoBehaviour`

#### Variables:
- `customDropdown` (SerializeField CustomTMPDropdown)
- `dropdown` (SerializeField TMP_Dropdown)
- `infoText` (SerializeField TextMeshProUGUI)

#### Functions:
- `Awake()` (void)
- `Start()` (void)
- `OnDestroy()` (void)
- `OnDropdownValueChanged(int value)` (private void)
- `GetSelectedOptionText()` (private string)
- `UpdateInfoText(string selectedOptionText)` (private void)
- `GetLocalizedString(string tableName, string entryKey)` (private string)
- `GetValues()` (public void)

---

## GameStartService.cs

### Class: `GameStartService`
**Type:** Static class

#### Functions:
- `TryStartGame(MapData mapData, CustomTMPDropdown levelDropdown, ChooseLevelWarning warning, SceneLoader sceneLoader, MapCardUI mapCard = null)` (public static bool)

---

## LocalizationMethods/LocaleSetter.cs

### Class: `LocaleSetter`
**Inherits:** `MonoBehaviour`

#### Variables:
- `PlayerPrefsKey` (private const string)
- `isInitialized` (private bool)

#### Functions:
- `Start()` (void)
- `InitializeAndLoadSavedLocale()` (private IEnumerator)
- `SetLocale(string localeCode)` (public void)
- `SetLocaleAfterInit(string localeCode)` (private IEnumerator)
- `SetEnglishUS()` (public void)
- `SetVietnameseVN()` (public void)

---

## LocalizationMethods/LocalizationManager.cs

### Class: `LocalizationManager`
**Type:** Static class

#### Variables:
- `TABLE_TITTLE_LABELS` (public const string) - Legacy misspelled constant (kept for compatibility)
- `TABLE_TITLE_LABELS` (public const string) - Preferred correctly spelled alias
- `TABLE_MAP_LABELS` (public const string)
- `TABLE_MAP_LEVEL` (public const string)
- `TABLE_MAP_INFO` (public const string)
- `TABLE_WEAPON_INFO` (public const string)

#### Functions:
- `GetLocalizedString(string tableName, string entryKey)` (public static LocalizedString)
- `GetTitleLabel(string entryKey)` (public static LocalizedString) - Preferred method
- `GetTittleLabel(string entryKey)` (public static LocalizedString) - Legacy misspelled alias
- `GetMapLabel(string entryKey)` (public static LocalizedString)
- `GetLocalizedStringSync(string tableName, string entryKey)` (public static string)
- `GetTitleLabelSync(string entryKey)` (public static string) - Preferred method
- `GetTittleLabelSync(string entryKey)` (public static string) - Legacy misspelled alias
- `GetMapLabelSync(string entryKey)` (public static string)
- `GetWeaponInfoSync(string entryKey)` (public static string)
- `SetLocale(string localeCode)` (public static void)
- `GetCurrentLocaleCode()` (public static string)
- `IsLocaleAvailable(string localeCode)` (public static bool)
- `GetAvailableLocaleCodes()` (public static string[])

---

## Maps/MapCardManager.cs

### Class: `MapCardManager`
**Inherits:** `SpawnCardLogic<MapData, MapCardUI>`

#### Variables:
- `mapCardPrefab` (SerializeField GameObject)
- `sceneLoader` (SerializeField SceneLoader)
- `mapsData` (SerializeField List<MapData>)
- `lockedCardAlpha` (public float)
- `numberOfMaps` (SerializeField int)

#### Functions:
- `Start()` (void)
- `GenerateMapCards()` (public void)
- `InitializeCard(MapCardUI card, MapData data, int index)` (protected override void)
- `AddMap(MapData mapData)` (public void)
- `ClearCards()` (public void)
- `RefreshCards()` (public void)
- `OnValidate()` (void)

---

## Maps/MapCardUI.cs

### Class: `MapCardUI`
**Inherits:** `MonoBehaviour`

#### Variables:
- `mapNameText` (SerializeField TextMeshProUGUI)
- `mapButton` (SerializeField Button)
- `mapImage` (SerializeField Image)
- `localizationTableName` (SerializeField string)
- `lockImageDisplay` (SerializeField Image)
- `mapData` (private MapData)
- `sceneLoader` (private SceneLoader)
- `localizedString` (private LocalizedString)
- `canvasGroup` (private CanvasGroup)
- `lockedCardAlpha` (private float)
- `levelDropdown` (private CustomTMPDropdown)
- `warning` (private ChooseLevelWarning)

#### Functions:
- `Awake()` (void)
- `OnDestroy()` (void)
- `Initialize(MapData data, SceneLoader loader, float cardAlpha = 0.3f)` (public void)
- `OnMapCardClicked()` (private void)
- `OnPlayButtonClicked()` (public void)
- `TryStartGame()` (private void)
- `UpdateText(string translatedText)` (private void)
- `UpdateCardLockedState()` (private void)
- `UpdateLockIcon()` (private void)
- `UpdateCard(MapData data)` (public void)

---

## Maps/MapData.cs

### Class: `MapData`
**Type:** Serializable class

#### Variables:
- `mapName` (SerializeField string)
- `localizationKey` (SerializeField string)
- `sceneName` (SerializeField string)
- `mapImage` (SerializeField Sprite)
- `isLocked` (SerializeField bool)
- `MapName` (public string property)
- `LocalizationKey` (public string property)
- `SceneName` (public string property)
- `MapImage` (public Sprite property)
- `IsLocked` (public bool property)

#### Functions:
- `MapData(string mapName, string sceneName, Sprite mapImage = null, bool isLocked = false)` (constructor)
- `MapData(string mapName, string localizationKey, string sceneName, Sprite mapImage = null, bool isLocked = false)` (constructor)

---

## Maps/MapSelectionManager.cs

### Class: `MapSelectionManager`
**Type:** Static class

#### Variables:
- `selectedMapSceneName` (private static string)

#### Functions:
- `SetSelectedMap(string sceneName)` (public static void)
- `GetSelectedMapSceneName()` (public static string)
- `HasSelectedMap()` (public static bool)
- `ClearSelectedMap()` (public static void)

---

## MenuButton/MenuButtons.cs

### Class: `MenuButtons`
**Inherits:** `MonoBehaviour`

#### Variables:
- `menuPanel` (SerializeField GameObject)

#### Functions:
- `Start()` (void)
- `SwitchPanel(GameObject fromPanel, GameObject toPanel)` (public void)
- `ShowPanel(GameObject toPanel)` (public void)

---

## PlayButton/PlayButtonHandler.cs

### Class: `PlayButtonHandler`
**Inherits:** `MonoBehaviour`

#### Variables:
- `warning` (SerializeField ChooseLevelWarning)
- `sceneLoader` (SerializeField SceneLoader)
- `sceneName` (SerializeField string)
- `mapCardUI` (SerializeField MapCardUI)

#### Functions:
- `Awake()` (void)
- `OnPlayButtonClicked()` (public void)
- `OnPlayButtonClicked(string sceneToLoad)` (public void)

---

## PlayButton/PlayButtonLevelCheck.cs

### Class: `PlayButtonLevelCheck`
**Inherits:** `MonoBehaviour`

#### Variables:
- `button` (private Button)
- `mapCardUI` (private MapCardUI)

#### Functions:
- `Awake()` (void)
- `OnButtonClicked()` (private void)

---

## SceneLoading/Loader.cs

### Class: `Loader`
**Type:** Static class

#### Variables:
- `onLoaderCallback` (private static Action)

#### Functions:
- `LoaderCallback()` (public static void)

---

## SceneLoading/LoaderCallback.cs

### Class: `LoaderCallback`
**Inherits:** `MonoBehaviour`

#### Variables:
- `isFirstUpdate` (private bool)

#### Functions:
- `Update()` (void)

---

## SceneLoading/SceneLoader.cs

### Class: `SceneLoader`
**Inherits:** `MonoBehaviour`

#### Functions:
- `LoadMenu()` (public void)
- `LoadMapMenu()` (public void)
- `LoadMap1()` (public void)
- `LoadMap2()` (public void)
- `LoadMap3()` (public void)
- `LoadSceneByName(string sceneName)` (public void)
- `LoadSceneWithLevelCheck(string sceneName)` (public void)
- `LoadMap1WithLevelCheck()` (public void)
- `LoadMap2WithLevelCheck()` (public void)
- `LoadMap3WithLevelCheck()` (public void)

---

## ScrollThumbFixed.cs

### Class: `ScrollThumbFixed`
**Inherits:** `MonoBehaviour`

#### Variables:
- `scrollbar` (SerializeField Scrollbar)
- `handleRect` (SerializeField RectTransform)
- `initialSize` (private float)
- `scrollbarRect` (private RectTransform)

#### Functions:
- `Awake()` (void)
- `OnEnable()` (void)
- `OnRectTransformDimensionsChange()` (void)
- `InitNextFrame()` (private IEnumerator)
- `ApplyThumbSize()` (private void)
- `SetThumbSize(float normalizedSize)` (public void)
- `ApplyFromNormalized(float normalizedSize)` (private void)

---

## SpawnCardLogic.cs

### Class: `SpawnCardLogic<TData, TCard>`
**Inherits:** `MonoBehaviour`
**Type:** Abstract generic class

#### Variables:
- `contentPath` (SerializeField string)
- `cardContainer` (protected Transform)
- `instantiatedCards` (protected readonly List<TCard>)

#### Functions:
- `SetupCardContainer()` (protected void)
- `GenerateCards(IList<TData> dataList, GameObject cardPrefab)` (protected void)
- `SpawnSingleCard(GameObject cardPrefab, TData data, int index)` (protected TCard)
- `ClearSpawnedCards()` (protected void)
- `IsDataValid(TData data)` (protected virtual bool)
- `InitializeCard(TCard card, TData data, int index)` (protected abstract void)

---

## Weapon/WeaponCardData.cs

### Class: `WeaponCardData`
**Inherits:** `MonoBehaviour`

#### Variables:
- `weaponNameText` (SerializeField TextMeshProUGUI)
- `attributesNamesText` (SerializeField TextMeshProUGUI)
- `attributesValuesText` (SerializeField TextMeshProUGUI)
- `bmbButton` (SerializeField Button)
- `ewarButton` (SerializeField Button)
- `bmbButtonText` (SerializeField TextMeshProUGUI)
- `ewarButtonText` (SerializeField TextMeshProUGUI)
- `upgradeText` (SerializeField TextMeshProUGUI)
- `currentWeapon` (private WeaponData)
- `weaponUpgrade` (private WeaponUpgrade)

#### Functions:
- `Awake()` (void)
- `Start()` (void)
- `FindWeaponUpgrade()` (private void)
- `OnDestroy()` (void)
- `SetWeapon(WeaponData data)` (public void)
- `GetAttributeNames()` (private List<string>)
- `GetAttributeValues(WeaponData data)` (private List<string>)
- `GetAttributeValue(WeaponData data, WeaponData.WeaponAttribute attribute)` (private float)
- `BuildAttributeText(List<string> attributes)` (private string)
- `SetEmptyData()` (private void)
- `OnBmbButtonClicked()` (private void)
- `OnEwarButtonClicked()` (private void)
- `RefreshDisplay()` (public void)
- `UpdateCurrencyCosts()` (private void)

---

## Weapon/WeaponCardDisplay.cs

### Class: `WeaponCardDisplay`
**Inherits:** `MonoBehaviour`

#### Variables:
- `weaponImage` (SerializeField Image)
- `useButton` (SerializeField Button)
- `rentOutButton` (SerializeField Button)
- `useButtonText` (SerializeField TextMeshProUGUI)
- `rentOutButtonText` (SerializeField TextMeshProUGUI)
- `currentWeapon` (private WeaponData)
- `currentSelectedCard` (private WeaponCardSelection)

#### Functions:
- `Awake()` (void)
- `OnDestroy()` (void)
- `SetWeapon(WeaponData data, WeaponCardSelection selectedCard = null)` (public void)
- `OnUseButtonClicked()` (private void)
- `OnRentOutButtonClicked()` (private void)

---

## Weapon/WeaponCardManager.cs

### Class: `WeaponCardManager`
**Inherits:** `SpawnCardLogic<WeaponData, WeaponCardSelection>`

#### Variables:
- `weaponCardSelectionPrefab` (SerializeField GameObject)
- `weaponsData` (SerializeField List<WeaponData>)
- `weaponCardDisplayPrefab` (SerializeField GameObject)
- `weaponCardDataPrefab` (SerializeField GameObject)
- `numberOfWeapons` (SerializeField int)
- `weaponMaxLv` (SerializeField int)
- `canvas` (private Canvas)
- `currentSelection` (private WeaponCardSelection)
- `instantiatedDisplay` (private WeaponCardDisplay)
- `instantiatedData` (private WeaponCardData)

#### Functions:
- `Start()` (void)
- `SpawnDisplayAndData()` (private void)
- `GenerateWeaponCards()` (public void)
- `InitializeCard(WeaponCardSelection card, WeaponData data, int index)` (protected override void)
- `AddWeapon(WeaponData weaponData)` (public void)
- `SetMaxLevelForAllWeapons()` (private void)
- `ClearCards()` (public void)
- `OnDestroy()` (void)
- `RefreshCards()` (public void)
- `OnValidate()` (void)
- `OnWeaponCardSelected(WeaponCardSelection card)` (public void)

---

## Weapon/WeaponCardSelection.cs

### Class: `WeaponCardSelection`
**Inherits:** `MonoBehaviour`

#### Variables:
- `weaponImage` (SerializeField Image)
- `useStatusText` (SerializeField TextMeshProUGUI)
- `rentOutStatusText` (SerializeField TextMeshProUGUI)
- `normalFrame` (SerializeField GameObject)
- `glowingFrame` (SerializeField GameObject)
- `weaponNameText` (private TextMeshProUGUI)
- `weaponData` (private WeaponData)
- `manager` (private WeaponCardManager)
- `isSelected` (private bool)
- `WeaponData` (public WeaponData property)

#### Functions:
- `Awake()` (void)
- `OnDestroy()` (void)
- `Initialize(WeaponData data, WeaponCardManager owner)` (public void)
- `OnCardClicked()` (private void)
- `SetSelected(bool selected)` (public void)
- `UpdateFrameDisplay(bool selected)` (private void)
- `SetStatus(WeaponStatus status)` (public void)

---

## Weapon/WeaponData.cs

### Class: `WeaponData`
**Type:** Serializable class

#### Variables:
- `WeaponAttribute` (public enum)
- `AttributeLocalizationKeys` (private static readonly Dictionary<WeaponAttribute, string>)
- `AttributeNames` (public static Dictionary<WeaponAttribute, string> property)
- `AttributeFormats` (public static readonly Dictionary<WeaponAttribute, string>)
- `AttributeOrder` (public static readonly WeaponAttribute[])
- `DefaultWeaponMaxLv` (public static int property)
- `weaponName` (SerializeField string)
- `weaponImage` (SerializeField Sprite)
- `damage` (SerializeField float)
- `dispersion` (SerializeField float)
- `rateOfFire` (SerializeField float)
- `reloadSpeed` (SerializeField float)
- `ammunition` (SerializeField float)
- `upgradeCount` (SerializeField int)
- `weaponMaxLv` (SerializeField int)
- `WeaponName` (public string property)
- `WeaponImage` (public Sprite property)
- `Damage` (public float property)
- `Dispersion` (public float property)
- `RateOfFire` (public float property)
- `ReloadSpeed` (public float property)
- `Ammunition` (public float property)
- `UpgradeCount` (public int property)
- `WeaponMaxLv` (public int property)
- `CanUpgrade` (public bool property)

#### Functions:
- `SetMaxLevel(int maxLevel)` (public void)
- `WeaponData(string weaponName, float damage, float dispersion, float rateOfFire, float reloadSpeed, float ammunition, Sprite weaponImage = null)` (constructor)
- `WeaponData(string weaponName, string unusedLocalizationKey, float damage, float dispersion, float rateOfFire, float reloadSpeed, float ammunition, Sprite weaponImage = null)` (constructor)
- `Upgrade()` (public bool)

#### Enums:
- `WeaponAttribute` (Damage, Dispersion, RateOfFire, ReloadSpeed, Ammunition)

---

## Weapon/WeaponMenuButtons.cs

### Class: `WeaponMenuButtons`
**Inherits:** `MonoBehaviour`

#### Variables:
- `backButton` (SerializeField GameObject)
- `doneButton` (SerializeField GameObject)
- `sceneLoader` (SerializeField SceneLoader)
- `backButtonText` (private TextMeshProUGUI)
- `localizedBackButton` (private LocalizedString)

#### Functions:
- `Awake()` (void)
- `Start()` (void)
- `OnBackButtonClicked()` (private void)
- `OnDoneButtonClicked()` (private void)
- `InitializeBackButtonText()` (private void)
- `UpdateBackButtonText(string translatedText)` (private void)
- `OnDestroy()` (void)

---

## Weapon/WeaponUpgrade.cs

### Class: `WeaponUpgrade`
**Inherits:** `MonoBehaviour`

#### Variables:
- `baseBMB` (SerializeField int)
- `baseEWAR` (SerializeField int)
- `weaponCardData` (private WeaponCardData)

#### Functions:
- `Awake()` (void)
- `UpgradeWithCurrency1(WeaponData weaponData)` (public bool)
- `UpgradeWithCurrency2(WeaponData weaponData)` (public bool)
- `ValidateUpgrade(WeaponData weaponData)` (private bool)
- `GetCurrency1Cost()` (public int)
- `GetCurrency2Cost()` (public int)
- `ResetCurrencyCosts(WeaponData weaponData)` (public void)

---

## Additional Enums

### Enum: `WeaponStatus`
**Location:** Weapon/WeaponCardSelection.cs

#### Values:
- `None`
- `Use`
- `RentOut`

---

## Summary Statistics

- **Total Classes:** 25
- **Total Static Classes:** 4 (GameStartService, LocalizationManager, MapSelectionManager, Loader)
- **Total Abstract Classes:** 1 (SpawnCardLogic<TData, TCard>)
- **Total Enums:** 2 (WeaponAttribute, WeaponStatus)
- **Total Serializable Classes:** 2 (MapData, WeaponData)

---

## Known Issues

No outstanding naming issues. See `ISSUES_REPORT.md` for current notes.