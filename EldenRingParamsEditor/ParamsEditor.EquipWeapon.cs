// SPDX-License-Identifier: GPL-3.0-only

namespace EldenRingParamsEditor;

public partial class ParamsEditor
{
    public const int ColIndexReinforcePrice = 9;

    public void SetEquipWeaponReinforcePrice(int equipWeaponId, int reinforcePrice)
    {
        SetValueAtCell(_equipWeapon, _idToRowIndexEquipWeapon, equipWeaponId, ColIndexReinforcePrice, reinforcePrice);
    }
}
