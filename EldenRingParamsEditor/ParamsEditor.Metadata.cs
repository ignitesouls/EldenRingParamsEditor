using DotNext.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingParamsEditor;

public partial class ParamsEditor
{
    public static Dictionary<int, List<ItemLotEntry>> GetWeaponIdsToItemLotMap()
    {
        return ResourceManager.GetWeaponIdsToItemLot(ItemLotType.Map);
    }

    public static Dictionary<int, List<ItemLotEntry>> GetWeaponIdsToItemLotEnemy()
    {
        return ResourceManager.GetWeaponIdsToItemLot(ItemLotType.Enemy);
    }

    public static Dictionary<int, List<int>> GetWeaponIdsToShopLineup()
    {
        return ResourceManager.GetWeaponIdsToShopLineup();
    }

    public void GenerateMappingWeaponIdsToShopLineup(List<int> weaponIds)
    {
        // Only work with known weaponIds
        List<int> equipWeaponIds = _idToRowIndexEquipWeapon.Keys.ToList();
        List<int> equipCustomWeaponIds = _idToRowIndexEquipCustomWeapon.Keys.ToList();
        foreach (int weaponId in weaponIds)
        {
            if (!equipWeaponIds.Contains(weaponId) && !equipCustomWeaponIds.Contains(weaponId))
            {
                throw new Exception($"Unknown weaponId: {weaponId}");
            }
        }

        Dictionary<int, List<int>> weaponIdsToShopLineupMap = new();

        // Iterate through all known ShopLineup Ids
        List<int> shopLineupIds = _idToRowIndexShopLineup.Keys.ToList();
        foreach (int shopLineupId in shopLineupIds)
        {
            // Get the itemId.
            int itemId = GetShopLineupEquipId(shopLineupId);
            if (itemId == 0)
            {
                continue; // If it's 0, then just skip it.
            }
            byte itemEquipType = GetShopLineupEquipType(shopLineupId);
            if (itemEquipType != 0 && itemEquipType != 5) // weapon or customWeapon
            {
                continue; // If it's not a weapon, then skip it.
            }

            // Now check if this is one of our target Ids.
            if (weaponIds.Contains(itemId))
            {
                // Add this itemId to the locations
                List<int> weaponIdLocations = weaponIdsToShopLineupMap.GetOrAdd(itemId, id =>
                {
                    return new List<int>() { };
                });
                weaponIdLocations.Add(shopLineupId);
                weaponIdsToShopLineupMap[itemId] = weaponIdLocations;
            }
        }

        // Save the results
        ResourceManager.SaveWeaponIdsToShopLineup(weaponIdsToShopLineupMap);
    }

    public void GenerateMappingWeaponIdsToItemLot(List<int> weaponIds)
    {
        // Only work with known weaponIds
        List<int> equipWeaponIds = _idToRowIndexEquipWeapon.Keys.ToList();
        List<int> equipCustomWeaponIds = _idToRowIndexEquipCustomWeapon.Keys.ToList();
        foreach (int weaponId in weaponIds)
        {
            if (!equipWeaponIds.Contains(weaponId) && !equipCustomWeaponIds.Contains(weaponId))
            {
                throw new Exception($"Unknown weaponId: {weaponId}");
            }
        }

        // Start with ItemLot_map
        Dictionary<int, List<ItemLotEntry>> weaponIdsToItemLotMap = new();

        // Iterate through all known ItemLotMap Ids
        List<int> itemLotMapIds = _idToRowIndexItemLotMap.Keys.ToList();
        foreach (int itemLotId in itemLotMapIds)
        {
            // We need to check all 8 ItemLotMap.lotItemIds
            for (int i = 0; i < 8; i++)
            {
                // Get the itemId.
                int itemId = GetItemLotMapLotItemId(itemLotId, i);
                if (itemId == 0)
                {
                    continue; // If it's 0, then just skip it. (Most will be 0).
                }
                int itemCategory = GetItemLotMapCategory(itemLotId, i);
                if (itemCategory != 2 && itemCategory != 6) // weapon or customWeapon
                {
                    continue; // If it's not a weapon, then skip it.
                }

                // Now check if this is one of our target Ids.
                if (weaponIds.Contains(itemId))
                {
                    // Add this itemId to the locations (first layer key)
                    List<ItemLotEntry> weaponIdLocations = weaponIdsToItemLotMap.GetOrAdd(itemId, id =>
                    {
                        return new List<ItemLotEntry>() { new ItemLotEntry() { ID = itemLotId, LotItems = new() } };
                    });

                    // Add this itemLotId to the locations (second layer index)
                    var thisLocation = weaponIdLocations.FirstOrDefault(l => l.ID == itemLotId);
                    if (thisLocation == null)
                    {
                        thisLocation = new ItemLotEntry() { ID = itemLotId, LotItems = new() };
                        weaponIdLocations.Add(thisLocation);
                    }

                    // Add this ItemLotMap.lotItemId to the list of known locations
                    thisLocation.LotItems.Add(i);
                }
            }
        }

        // Save the results
        ResourceManager.SaveWeaponIdsToItemLot(ItemLotType.Map, weaponIdsToItemLotMap);

        // Also process ItemLot_enemy
        Dictionary<int, List<ItemLotEntry>> weaponIdsToItemLotEnemy = new();

        // Iterate through all known ItemLotEnemy Ids
        List<int> itemLotEnemyIds = _idToRowIndexItemLotEnemy.Keys.ToList();
        foreach (int itemLotId in itemLotEnemyIds)
        {
            // We need to check all 8 ItemLotEnemy.lotItemIds
            for (int i = 0; i < 8; i++)
            {
                // Get the itemId.
                int itemId = GetItemLotEnemyLotItemId(itemLotId, i);
                if (itemId == 0)
                {
                    continue; // If it's empty, then skip it.
                }
                int itemCategory = GetItemLotEnemyCategory(itemLotId, i);
                if (itemCategory != 2 && itemCategory != 6) // weapon or customWeapon
                {
                    continue; // If it's not a weapon, then skip it.
                }

                // Now check if this is one of our target Ids.
                if (weaponIds.Contains(itemId))
                {
                    // Add this lotItemId to the list for this itemLotId.
                    List<ItemLotEntry> weaponIdLocations = weaponIdsToItemLotEnemy.GetOrAdd(itemId, id =>
                    {
                        List<int> lotItems = new();
                        ItemLotEntry itemLotEntry = new ItemLotEntry() { ID = itemLotId, LotItems = lotItems };
                        return new List<ItemLotEntry>() { itemLotEntry };
                    });

                    // Add this itemLotId to the locations (second layer index)
                    var thisLocation = weaponIdLocations.FirstOrDefault(l => l.ID == itemLotId);
                    if (thisLocation == null)
                    {
                        thisLocation = new ItemLotEntry() { ID = itemLotId, LotItems = new() };
                        weaponIdLocations.Add(thisLocation);
                    }

                    // Add this ItemLotMap.lotItemId to the list of known locations
                    thisLocation.LotItems.Add(i);
                }
            }
        }

        // Save the results
        ResourceManager.SaveWeaponIdsToItemLot(ItemLotType.Enemy, weaponIdsToItemLotEnemy);
    }
}
