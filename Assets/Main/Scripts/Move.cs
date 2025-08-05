using UnityEngine;

public class HMove {
    public int move;
    public int ep;
    public int castle;
    public long pos;
    public int fiftyMove;
}

public struct SMove {
    public int move;
    public int score;
}

public static class Move 
{

    public static int CreateMove(int from, int to, int piece, int capture, int promo) {
        return (from | (to << 6) | (piece << 12) | (capture << 16) | (promo << 20));
    }


    #region MethodsToGetMoveData


    public static int GetFrom(this int value) {
        return value & 0x3f;
    }


    public static int GetTo(this int value) {
        return (value >> 6) & 0x3f;
    }


    public static int GetPiece(this int value)
    {
        return (value >> 12) & 0xf;
    }


    public static int GetCapture(this int value)
    {
        return (value >> 16) & 0xf;
    }



    public static int GetPromo(this int value)
    {
        return (value >> 20) & 0xf;
    }


    #endregion


    #region MoveCheckMethods

    public static int MovingPieceSide(this int value) {
        return value >> 24;
    }


    public static bool IsCapture(this int value) { 
        return (value & 0xf0000) != 0;
    }


    public static bool IsEnPassantCapt(this int value) 
    {
        return (value & 0x700000) == 0x100000;
    }

		
    public static bool IsPromotion(this int value) 
    {
        return (value & 0x700000) > 0x200000;
    }


    public static bool IsCastle(this int value) {
        return (value & 0x700000) == 0x200000;
    }

    public static string PrintMove(int move) {
        return ((Squares)move.GetFrom()).ToString().ToLower() + ((Squares)move.GetTo()).ToString().ToLower();
    }

    #endregion

}
