// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Andre.Formats;

namespace EldenRingParamsEditor;

public partial class ParamsEditor
{
    public const int ColIndexBaseWeaponId = 0;
    public const int ColIndexGemId = 1;
    public const int ColIndexReinforceLevel = 2;

    public int? GetEquipCustomWeaponRowIndex(int equipCustomWeaponId)
    {
        if (_idToRowIndexEquipCustomWeapon.TryGetValue(equipCustomWeaponId, out int rowIndex))
        {
            return rowIndex;
        }
        return null;
    }

    public void CreateNewEquipCustomWeaponRow(int equipCustomWeaponId, string name = "")
    {
        // clone the first row. this is necessary because we can only add a row to the param if it has it as a parent
        Param.Row newRow = new(_equipCustomWeapon.Rows.ElementAt(0));
        newRow.ID = equipCustomWeaponId;
        newRow.Name = name;
        _idToRowIndexEquipCustomWeapon.Add(equipCustomWeaponId, _equipCustomWeapon.Rows.Count);
        _equipCustomWeapon.AddRow(newRow);
        SetValueAtCell(_equipCustomWeapon, _idToRowIndexEquipCustomWeapon, equipCustomWeaponId, ColIndexBaseWeaponId, 0);
        SetValueAtCell(_equipCustomWeapon, _idToRowIndexEquipCustomWeapon, equipCustomWeaponId, ColIndexGemId, 0);
        SetValueAtCell(_equipCustomWeapon, _idToRowIndexEquipCustomWeapon, equipCustomWeaponId, ColIndexReinforceLevel, (byte)0);
    }

    public int GetEquipCustomWeaponBaseWeaponId(int equipCustomWeaponId)
    {
        return (int)GetValueAtCell(_equipCustomWeapon, _idToRowIndexEquipCustomWeapon, equipCustomWeaponId, ColIndexBaseWeaponId);
    }

    public void SetEquipCustomWeaponBaseWeaponId(int equipCustomWeaponId, int baseWeaponId)
    {
        SetValueAtCell(_equipCustomWeapon, _idToRowIndexEquipCustomWeapon, equipCustomWeaponId, ColIndexBaseWeaponId, baseWeaponId);
    }

    public int GetEquipCustomWeaponGemId(int equipCustomWeaponId)
    {
        return (int)GetValueAtCell(_equipCustomWeapon, _idToRowIndexEquipCustomWeapon, equipCustomWeaponId, ColIndexGemId);
    }

    public void SetEquipCustomWeaponGemId(int equipCustomWeaponId, int gemId)
    {
        SetValueAtCell(_equipCustomWeapon, _idToRowIndexEquipCustomWeapon, equipCustomWeaponId, ColIndexGemId, gemId);
    }

    public byte GetEquipCustomWeaponReinforceLevel(int equipCustomWeaponId)
    {
        return (byte)GetValueAtCell(_equipCustomWeapon, _idToRowIndexEquipCustomWeapon, equipCustomWeaponId, ColIndexReinforceLevel);
    }

    public void SetEquipCustomWeaponReinforceLevel(int equipCustomWeaponId, byte reinforceLevel)
    {
        SetValueAtCell(_equipCustomWeapon, _idToRowIndexEquipCustomWeapon, equipCustomWeaponId, ColIndexReinforceLevel, reinforceLevel);
    }
}
