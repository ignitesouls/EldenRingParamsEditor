// SPDX-License-Identifier: GPL-3.0-only

namespace EldenRingParamsEditor;

internal partial class ParamsEditor
{
    public object GetItemLotEnemyLotItemId(int itemLotId, int itemIndex)
    {
        if (itemIndex < 0 || itemIndex > 7)
        {
            throw new Exception($"Index {itemIndex} out of bounds.");
        }
        return GetValueAtCell(_itemLotEnemy, _idToRowIndexItemLotEnemy, itemLotId, ColIndexLotItemId + itemIndex);
    }

    public object GetItemLotEnemyCategory(int itemLotId, int itemIndex)
    {
        if (itemIndex < 0 || itemIndex > 7)
        {
            throw new Exception($"Index {itemIndex} out of bounds.");
        }
        return GetValueAtCell(_itemLotEnemy, _idToRowIndexItemLotEnemy, itemLotId, ColIndexCategory + itemIndex);
    }

    public void SetItemLotEnemyLotItemId(int itemLotId, int itemIndex, int itemId)
    {
        if (itemIndex < 0 || itemIndex > 7)
        {
            throw new Exception($"Index {itemIndex} out of bounds.");
        }
        SetValueAtCell(_itemLotEnemy, _idToRowIndexItemLotEnemy, itemLotId, ColIndexLotItemId + itemIndex, itemId);
    }

    public void SetItemLotEnemyCategory(int itemLotId, int itemIndex, int category)
    {
        if (itemIndex < 0 || itemIndex > 7)
        {
            throw new Exception($"Index {itemIndex} out of bounds.");
        }
        SetValueAtCell(_itemLotEnemy, _idToRowIndexItemLotEnemy, itemLotId, ColIndexCategory + itemIndex, category);
    }
}
