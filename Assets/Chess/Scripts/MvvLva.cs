using UnityEngine;
using System.Collections;

public static class MvvLva 
{
    public static int[][] Table;

    public static int[] VictimScore = new int[13] { 0, 100, 200, 300, 400, 500, 600, 100, 200, 300, 400, 500, 600}; 

    public static void Init() {

        Table = new int[14][];
        for (int i = 0; i<Table.Length; i++) {
            Table[i] = new int[13];
        }

        for (int attacker = 0; attacker < 13; attacker++) {
            for (int victim = 0; victim < 13; victim++)
            {
                Table[victim][attacker] = VictimScore[victim] + 6 - (VictimScore[attacker] / 100);
            }
        }
    }

}
