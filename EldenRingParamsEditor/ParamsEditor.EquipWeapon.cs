// SPDX-License-Identifier: GPL-3.0-only

namespace EldenRingParamsEditor;

public partial class ParamsEditor
{
    public const int ColIndexReinforcePrice = 9;
    public const int ColIndexMaterialSetId = 25;

    public void SetEquipWeaponReinforcePrice(int equipWeaponId, int reinforcePrice)
    {
        SetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexReinforcePrice, reinforcePrice);
    }

    public int GetEquipWeaponMaterialSetId(int equipWeaponId)
    {
        return (int) GetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexMaterialSetId);
    }
}
