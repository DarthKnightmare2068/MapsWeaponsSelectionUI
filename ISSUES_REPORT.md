# Naming Issues and Potential Problems Report

This document identifies misspellings, unclear naming, and inconsistencies found in the codebase (excluding folder names).

## Current Issues

3) Missing Application.isQuitting check in OnDestroy  
   - Files: 
     - `Assets/Scripts/Weapon/WeaponCardManager.cs` (Line 124-135)  
     - `Assets/Scripts/SpawnCardLogic.cs` (Line 86-97)  
   - Issue: `OnDestroy()` methods call `Destroy()` on GameObjects without checking `Application.isQuitting`. During application quit, Unity may have already destroyed these objects, causing errors in the console.  
   - Recommendation: Add `if (Application.isQuitting) return;` at the start of `OnDestroy()` methods before destroying objects.

## Resolved Issues

1) ~~Potential null reference in ChooseLevelWarning~~ **RESOLVED**  
   - ~~File: `Assets/Scripts/ChooseLevelWarning.cs` (Line 107)~~  
   - **Fixed:** Added null check before accessing `cachedMapCards[0]`. Now checks `cachedMapCards[0] != null` before assignment to prevent null reference exceptions when the cached array contains null entries.

2) ~~Missing null check in MapCardUI.Initialize~~ **RESOLVED**  
   - ~~File: `Assets/Scripts/Maps/MapCardUI.cs` (Line 81)~~  
   - **Fixed:** Added null validation at the start of `Initialize()` method. Returns early with error log if `data` parameter is null, preventing NullReferenceException when accessing `data.LocalizationKey` and `data.MapName`.

## Notes

- Codebase generally reads cleanly; remaining items are minor safety/performance concerns.
