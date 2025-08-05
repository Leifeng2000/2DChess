using UnityEngine;
using System.Collections;

public struct PVEntry
{
    public int move;
    public long hash;
}

public class PVTable
{
    public PVEntry[] data;
    public int numEntries;
}