// SPDX-License-Identifier: GPL-3.0-only
namespace EldenRingParamsEditor;

public partial class ParamsEditor
{
    public const int ColEventId = 3;
    uint graceflagOn = 6001;

    public void SetGraceEventFlagId(int graceId)
    {
        // Cast int → uint explicitly
        SetValueAtCell(_bonfireWarpParam, _idToRowIndexBonfireWarp, graceId, ColEventId, graceflagOn);
    }
}
