using System;

public struct SlotInfo
{
    private int[] values;

    public int SlotCount { get; }

    public SlotInfo(int slotCount)
    {
        SlotCount = slotCount;
        values = new int[slotCount];
    }

    public void SetValue(int index, int value)
    {
        if (index < 0 || index >= SlotCount)
            throw new ArgumentOutOfRangeException(nameof(index));
        values[index] = value;
    }

    public int GetValue(int index)
    {
        if (index < 0 || index >= SlotCount)
            throw new ArgumentOutOfRangeException(nameof(index));
        return values[index];
    }
}
