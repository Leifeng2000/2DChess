﻿public class HMove
{
    public int move;
    public int ep;
    public int castle;
    public long pos;
    public int fiftyMove;
}

public struct SMove
{
    public int move;
    public int score;
}

public static class Move
{
    private const int FromMask = 0x3f;
    private const int ToMask = 0x3f;
    private const int PieceMask = 0xf;
    private const int CaptureMask = 0xf;
    private const int PromoMask = 0xf;
    private const int CaptureFlag = 0xf0000;
    private const int EnPassantFlag = 0x100000;
    private const int PromotionFlag = 0x200000;
    private const int CastleFlag = 0x200000;

    public static int CreateMove(int from, int to, int piece, int capture, int promo)
    {
        return (from | (to << 6) | (piece << 12) | (capture << 16) | (promo << 20));
    }

    #region MethodsToGetMoveData

    public static int GetFrom(this int value)
    {
        return value & FromMask;
    }

    public static int GetTo(this int value)
    {
        return (value >> 6) & ToMask;
    }

    public static int GetPiece(this int value)
    {
        return (value >> 12) & PieceMask;
    }

    public static int GetCapture(this int value)
    {
        return (value >> 16) & CaptureMask;
    }

    public static int GetPromo(this int value)
    {
        return (value >> 20) & PromoMask;
    }

    #endregion

    #region MoveCheckMethods

    public static int MovingPieceSide(this int value)
    {
        return value >> 24;
    }

    public static bool IsCapture(this int value)
    {
        return (value & CaptureFlag) != 0;
    }

    public static bool IsEnPassantCapt(this int value)
    {
        return (value & 0x700000) == EnPassantFlag;
    }

    public static bool IsPromotion(this int value)
    {
        return (value & 0x700000) > PromotionFlag;
    }

    public static bool IsCastle(this int value)
    {
        return (value & 0x700000) == CastleFlag;
    }

    public static string PrintMove(int move)
    {
        return ((Squares)move.GetFrom()).ToString().ToLower() + ((Squares)move.GetTo()).ToString().ToLower();
    }

    #endregion
}