﻿// SPDX-License-Identifier: GPL-3.0-only
using SoulsFormats;
using Andre.Formats;
using System.Diagnostics;
using System.IO.Enumeration;

namespace EldenRingParamsEditor;

public partial class ParamsEditor
{
    private BND4 _regulationBnd;

    private Param _itemLotMap;
    private Dictionary<int, int> _idToRowIndexItemLotMap;

    private Param _itemLotEnemy;
    private Dictionary<int, int> _idToRowIndexItemLotEnemy;

    private Param _shopLineup;
    private Dictionary<int, int> _idToRowIndexShopLineup;

    private Param _charaInit;
    private Dictionary<int, int> _idToRowIndexCharaInit;

    private Param _equipGoods;
    public Dictionary<int, int> _idToRowIndexEquipGoods;

    private Param _equipWeapon;
    public Dictionary<int, int> _idToRowIndexEquipWeapon;

    private Param _equipCustomWeapon;
    public Dictionary<int, int> _idToRowIndexEquipCustomWeapon;

    private Param _equipProtector;
    public Dictionary<int, int> _idToRowIndexEquipProtector;

    private Param _spEffect;
    public Dictionary<int, int> _idToRowIndexSpEffect;

    // only has 1 row
    private Param _gameSystemCommon;

    // Params constants for reading/writing to regulation.bin
    public const string ItemLotParam_map = "ItemLotParam_map.param";
    public const string ItemLotParam_enemy = "ItemLotParam_enemy.param";
    public const string ItemLotParamDef = "ItemLotParam";

    public const string EquipParamGoods = "EquipParamGoods.param";
    public const string EquipParamGoodsParamDef = "EquipParamGoods";

    public const string EquipParamWeapon = "EquipParamWeapon.param";
    public const string EquipParamWeaponParamDef = "EquipParamWeapon";

    public const string EquipParamCustomWeapon = "EquipParamCustomWeapon.param";
    public const string EquipParamCustomWeaponParamDef = "EquipParamCustomWeapon";

    public const string EquipParamProtector = "EquipParamProtector.param";
    public const string EquipParamProtectorParamDef = "EquipParamProtector";

    public const string ShopLineupParam = "ShopLineupParam.param";
    public const string ShopLineupParamDef = "ShopLineupParam";

    public const string CharaInitParam = "CharaInitParam.param";
    public const string CharaInitParamDef = "CharaInitParam";

    public const string SpEffectParam = "SpEffectParam.param";
    public const string SpEffectParamDef = "SpEffectParam";

    public const string GameSystemCommonParam = "GameSystemCommonParam.param";
    public const string GameSystemCommonParamDef = "GameSystemCommonParam";

    private ParamsEditor(string regulationPath)
    {
        _regulationBnd = SFUtil.DecryptERRegulation(regulationPath);

        foreach (BinderFile file in _regulationBnd.Files)
        {
            string fileName = Path.GetFileName(file.Name);
            switch (fileName)
            {
                case ItemLotParam_map:
                    {
                        _idToRowIndexItemLotMap = new Dictionary<int, int>();
                        _itemLotMap = initParam(file, ItemLotParamDef, _idToRowIndexItemLotMap);
                        break;
                    }
                case ItemLotParam_enemy:
                    {
                        _idToRowIndexItemLotEnemy = new Dictionary<int, int>();
                        _itemLotEnemy = initParam(file, ItemLotParamDef, _idToRowIndexItemLotEnemy);
                        break;
                    }
                case ShopLineupParam:
                    {
                        _idToRowIndexShopLineup = new Dictionary<int, int>();
                        _shopLineup = initParam(file, ShopLineupParamDef, _idToRowIndexShopLineup);
                        break;
                    }
                case CharaInitParam:
                    {
                        _idToRowIndexCharaInit = new Dictionary<int, int>();
                        _charaInit = initParam(file, CharaInitParamDef, _idToRowIndexCharaInit);
                        break;
                    }
                case EquipParamGoods:
                    {
                        _idToRowIndexEquipGoods = new Dictionary<int, int>();
                        _equipGoods = initParam(file, EquipParamGoodsParamDef, _idToRowIndexEquipGoods);
                        break;
                    }
                case EquipParamWeapon:
                    {
                        _idToRowIndexEquipWeapon = new Dictionary<int, int>();
                        _equipWeapon = initParam(file, EquipParamWeaponParamDef, _idToRowIndexEquipWeapon);
                        break;
                    }
                case EquipParamCustomWeapon:
                    {
                        _idToRowIndexEquipCustomWeapon = new Dictionary<int, int>();
                        _equipCustomWeapon = initParam(file, EquipParamCustomWeaponParamDef, _idToRowIndexEquipCustomWeapon);
                        break;
                    }
                case EquipParamProtector:
                    {
                        _idToRowIndexEquipProtector = new Dictionary<int, int>();
                        _equipProtector = initParam(file, EquipParamProtectorParamDef, _idToRowIndexEquipProtector);
                        break;
                    }
                case SpEffectParam:
                    {
                        _idToRowIndexSpEffect = new Dictionary<int, int>();
                        _spEffect = initParam(file, SpEffectParamDef, _idToRowIndexSpEffect);
                        break;
                    }
                case GameSystemCommonParam:
                    {
                        _gameSystemCommon = initParam(file, GameSystemCommonParamDef);
                        break;
                    }
            }
        }
        if (_itemLotMap == null || _idToRowIndexItemLotMap == null
            || _itemLotEnemy == null || _idToRowIndexItemLotEnemy == null
            || _shopLineup == null || _idToRowIndexShopLineup == null
            || _charaInit == null || _idToRowIndexCharaInit == null
            || _equipWeapon == null || _idToRowIndexEquipWeapon == null
            || _equipCustomWeapon == null || _idToRowIndexEquipCustomWeapon == null
            || _equipProtector == null || _idToRowIndexEquipProtector == null
            || _spEffect == null || _idToRowIndexSpEffect == null
            || _gameSystemCommon == null)
        {
            throw new Exception("Failed to read expected params from given regulation path");
        }
    }

    private Param initParam(BinderFile file, string paramdefName, Dictionary<int, int>? idToRowIndex = null)
    {
        PARAMDEF paramdef = ResourceManager.GetParamDefByName(paramdefName);
        Param param = Param.Read(file.Bytes);
        param.ApplyParamdef(paramdef);
        if (idToRowIndex != null)
        {
            //Debug.WriteLine($"Fetching IDs to rowIndex for {paramdefName}");
            int i = 0;
            foreach (Param.Row row in param.Rows)
            {
                idToRowIndex[row.ID] = i++;
            }
        }
        return param;
    }

    private object GetValueAtCell(Param param, Dictionary<int, int>? idToRowIndex, int idOrRowIndex, int colIndex)
    {
        int rowIndex = idOrRowIndex;
        if (idToRowIndex != null)
        {
            if (!idToRowIndex.TryGetValue(idOrRowIndex, out rowIndex))
            {
                throw new Exception($"ID {idOrRowIndex} not found in idToRowIndex dictionary");
            }
        }
        if (rowIndex >= param.Rows.Count)
        {
            throw new Exception($"Attempted to access rowIndex {rowIndex} but there's only {param.Rows.Count} rows");
        }
        if (colIndex >= param.Columns.Count)
        {
            throw new Exception($"Attempted to access index {colIndex} but there's only {param.Columns.Count} columns");
        }
        Param.Row row = param.Rows[rowIndex];
        Param.Column column = param.Columns[colIndex];
        return column.GetValue(row);
    }

    private void SetValueAtCell(Param param, Dictionary<int, int>? idToRowIndex, int idOrRowIndex, int colIndex, object value)
    {
        int rowIndex = idOrRowIndex;
        if (idToRowIndex != null)
        {
            if (!idToRowIndex.TryGetValue(idOrRowIndex, out rowIndex))
            {
                throw new Exception($"ID {idOrRowIndex} not found in idToRowIndex dictionary");
            }
        }
        if (rowIndex >= param.Rows.Count) {
            throw new Exception($"Attempted to access index {rowIndex} but there's only {param.Rows.Count} rows");
        }
        if (colIndex >= param.Columns.Count)
        {
            throw new Exception($"Attempted to access index {colIndex} but there's only {param.Columns.Count} columns");
        }
        Param.Row row = param.Rows[rowIndex];
        Param.Column column = param.Columns[colIndex];
        column.SetValue(row, value);
    }

    public static ParamsEditor ReadFromRegulationPath(string regulationPath)
    {
        return new(regulationPath);
    }

    public void WriteToRegulationPath(string regulationPath)
    {
        foreach (BinderFile file in _regulationBnd.Files)
        {
            string fileName = Path.GetFileName(file.Name);
            switch (fileName)
            {
                case ItemLotParam_map:
                    {
                        file.Bytes = _itemLotMap.Write();
                        break;
                    }
                case ItemLotParam_enemy:
                    {
                        file.Bytes = _itemLotEnemy.Write();
                        break;
                    }
                case ShopLineupParam:
                    {
                        file.Bytes = _shopLineup.Write();
                        break;
                    }
                case CharaInitParam:
                    {
                        file.Bytes = _charaInit.Write();
                        break;
                    }
                case EquipParamGoods:
                    {
                        file.Bytes = _equipGoods.Write();
                        break;
                    }
                case EquipParamWeapon:
                    {
                        file.Bytes = _equipWeapon.Write();
                        break;
                    }
                case EquipParamCustomWeapon:
                    {
                        file.Bytes = _equipCustomWeapon.Write();
                        break;
                    }
                case EquipParamProtector:
                    {
                        file.Bytes = _equipProtector.Write();
                        break;
                    }
                case SpEffectParam:
                    {
                        file.Bytes = _spEffect.Write();
                        break;
                    }
                case GameSystemCommonParam:
                    {
                        file.Bytes = _gameSystemCommon.Write();
                        break;
                    }
            }
        }
        SFUtil.EncryptERRegulation(regulationPath, _regulationBnd);
    }
}
