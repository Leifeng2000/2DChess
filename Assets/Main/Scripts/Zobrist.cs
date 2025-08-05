using UnityEngine;
using System.Collections;
using System;

public static class Zobrist {

    public static long[][] Pieces;
    public static long[] CastleRights;
    public static long[] SideToPlay;
    public static long[] EPSquares;

    public static void Init() {

        System.Random rand = new System.Random(700);
        Pieces = new long[64][];
        CastleRights = new long[16];
        EPSquares = new long[64];
        SideToPlay = new long[2];

        for (int c = 0; c < 16; c++)
        {
            CastleRights[c] = rand.NextInt64();
        }

        for (int i = 0; i < 64; i++) 
        {

            Pieces[i] = new long[13];

            for (int p = 0; p < Pieces[i].Length; p++)
            {
                Pieces[i][p] = rand.NextInt64();
            }

            EPSquares[i] = rand.NextInt64();
        }


        SideToPlay[0] = rand.NextInt64();
        SideToPlay[1] = rand.NextInt64();

    }
    public static long GetHashPosition(this Board board) 
    {
        long pos = 0;

        for (int i = 0; i < 64; i++) {
            pos ^= Pieces[i][board.pieces[i]];
        }
        pos ^= SideToPlay[board.SideToPlay];

        if (board.EnPassantSq != Squares.None)
            pos ^= EPSquares[(int)board.EnPassantSq];

        return pos;
    }

    public static long NextInt64(this System.Random rnd)
    {
        var buffer = new byte[sizeof(Int64)];
        rnd.NextBytes(buffer);
        return Math.Abs(BitConverter.ToInt64(buffer, 0));
    }
}
