﻿// SPDX-License-Identifier: GPL-3.0-only

namespace EldenRingParamsEditor;

public partial class ParamsEditor
{
    // 920 = 130
    // 1225 = 170
    public const int ColIndexReinforcePrice = 9;
    public const int ColIndexMaterialSetId = 25;
    public const int ColIndexReinforceTypeId = 63;
    public const int ColIndexReinforceShopCategory = 230;
    public const int ColIndexIsCustom = 116;

    public const int ColIndexProperStrength = 80;
    public const int ColIndexProperDexterity = 81;
    public const int ColIndexProperIntelligence = 82;
    public const int ColIndexProperFaith = 83;
    public const int ColIndexProperArcane = 186;


    public static readonly int[] ColIndexProperStats = { ColIndexProperStrength,
                                                         ColIndexProperDexterity,
                                                         ColIndexProperIntelligence,
                                                         ColIndexProperFaith,
                                                         ColIndexProperArcane };

    public void SetEquipWeaponReinforcePrice(int equipWeaponId, int reinforcePrice)
    {
        SetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexReinforcePrice, reinforcePrice);
    }

    public int GetEquipWeaponMaterialSetId(int equipWeaponId)
    {
        return (int)GetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexMaterialSetId);
    }

    public void SetEquipWeaponMaterialSetId(int equipWeaponId, int materialSetId)
    {
        SetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexMaterialSetId, materialSetId);
    }

    public byte GetEquipWeaponProperStat(int equipWeaponId, int stat)
    {
        return (byte)GetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexProperStats[stat]);
    }

    public byte GetEquipWeaponIsCustom(int equipWeaponId)
    {
        return (byte)GetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexIsCustom);
    }

    public void SetEquipWeaponIsCustom(int equipWeaponId, byte isCustom)
    {
        SetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexIsCustom, isCustom);
    }

    public short GetEquipWeaponReinforceTypeId(int equipWeaponId)
    {
        return (short)GetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexReinforceTypeId);
    }

    public void SetEquipWeaponReinforceTypeId(int equipWeaponId, short reinforceTypeId)
    {
        SetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexReinforceTypeId, reinforceTypeId);
    }

    public byte GetEquipWeaponReinforceShopCategory(int equipWeaponId)
    {
        return (byte)GetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexReinforceShopCategory);
    }

    public void SetEquipWeaponReinforceShopCategory(int equipWeaponId, byte reinforceShopCategory)
    {
        SetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexReinforceShopCategory, reinforceShopCategory);
    }
}
