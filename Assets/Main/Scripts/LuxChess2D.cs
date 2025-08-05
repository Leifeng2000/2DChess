using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class LuxChess2D : Engine 
{
    [Serializable]
    public class PieceSet
    {
        public Sprite Pawn;
        public Sprite Bishop;
        public Sprite Knight;
        public Sprite Rook;
        public Sprite Queen;
        public Sprite King;
        public Sprite Square;
    }

    public PieceSet WhitePieces;

    public PieceSet BlackPieces;

    [HideInInspector]
    public Transform[] SquaresObj;
    public Transform CanvasTr;
    public GameObject SquareObj;
    public Sprite NoPieceSprite;

    public int ComputerThinkingTime = 3;

    public string StartingFen = FEN.Default;

    [HideInInspector]
    public int DragingFrom;

    public UnityEvent OnWhiteTurn;
    public UnityEvent OnBlackTurn;

    
    public InputField FENOutputUI;

    private void Start() {

        DragingFrom = -1;
        SquaresObj = new Transform[64];

        for (int i = 0; i < 64; i++)
        {
            int x= i%8;
            int y = i/8;

            
            GameObject obj = Instantiate(SquareObj, Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(CanvasTr, true);
            obj.transform.localPosition = new Vector3(-244 + 64 * x, 204 - 64 * y, 0);

            PieceMover pm = obj.GetComponent<PieceMover>();
            pm.index = i;
            pm.manager = this;

            if ((i + y) % 2 == 0)
            {
                obj.GetComponent<Image>().sprite = WhitePieces.Square;
            }
            else
            {
                obj.GetComponent<Image>().sprite = BlackPieces.Square;
            }
            SquaresObj[i] = obj.transform;
        }

        InitChess(StartingFen);
        
        UpdateBoard();

    }

    public void UpdateBoard() {
        for (int i = 0; i < 64; i++)
        {
            int piece = GetPieceAt(i);

            switch (piece)
            {
                case Defs.WPawn: CreatePiece(WhitePieces.Pawn, i); break;
                case Defs.WBishop: CreatePiece(WhitePieces.Bishop, i); break;
                case Defs.WKnight: CreatePiece(WhitePieces.Knight, i); break;
                case Defs.WRook: CreatePiece(WhitePieces.Rook, i); break;
                case Defs.WQueen: CreatePiece(WhitePieces.Queen, i); break;
                case Defs.WKing: CreatePiece(WhitePieces.King, i); break;
                case Defs.BPawn: CreatePiece(BlackPieces.Pawn, i); break;
                case Defs.BBishop: CreatePiece(BlackPieces.Bishop, i); break;
                case Defs.BKnight: CreatePiece(BlackPieces.Knight, i); break;
                case Defs.BRook: CreatePiece(BlackPieces.Rook, i); break;
                case Defs.BQueen: CreatePiece(BlackPieces.Queen, i); break;
                case Defs.BKing: CreatePiece(BlackPieces.King, i); break;
                case Defs.Empty:
                    CreatePiece(NoPieceSprite, i);
                    break;
            }

        }

    
    }


    private Squares LastSquare = Squares.None;
    public override void OnTurnSwitched()
    { 

        //Who's turn?
        if (SideToPlay() == 1)
        {
            if (OnWhiteTurn != null)
                OnWhiteTurn.Invoke();
        }
        else {
            if (OnBlackTurn != null)
                OnBlackTurn.Invoke();
            ComputerPlay(ComputerThinkingTime);
        }


        if (GameState() == BoardState.WhiteMate)
        {
            GameManager.isGameOver = true;
            GameManager.gameOverState = "WhiteMate";
        }
        if (GameState() == BoardState.BlackMate)
        {
            GameManager.isGameOver = true;
            GameManager.gameOverState = "BlackMate";
        }
        if (GameState() == BoardState.StaleMate)
        {
            GameManager.isGameOver = true;
            GameManager.gameOverState = "StaleMate";
        }
        
        Squares sq = IsInCheck();

        if (sq != Squares.None) {
            LastSquare = sq;
            SquaresObj[(int)LastSquare].gameObject.GetComponent<Image>().color = new Color32(255,15,15,255);
            GameManager.Instance.PlayChessDownAudio(GameManager.Instance.clips[3]);

        }
        else if (LastSquare != Squares.None) {
            SquaresObj[(int)LastSquare].gameObject.GetComponent<Image>().color = Color.white;
        }
    }



    private Vector2 origPieceSize = Vector2.zero;
    private void CreatePiece(Sprite piece, int index, float scale = 1f)
    {
        Image image = SquaresObj[index].GetChild(0).gameObject.GetComponent<Image>();

        
        if (origPieceSize == Vector2.zero)
            origPieceSize = image.rectTransform.sizeDelta;

        image.rectTransform.sizeDelta = origPieceSize*scale;
        image.sprite = piece;

    }

    private Squares LastSquareFrom = Squares.None;
    private Squares LastSquareTo = Squares.None;
    public override void OnComputerPlayed(int from, int to)
    {

        
        SquaresObj[(int)from].gameObject.GetComponent<Image>().color = new Color32(49, 73, 54, 255);
        SquaresObj[(int)to].gameObject.GetComponent<Image>().color = new Color32(36, 62, 46, 255);

        
        if ((int)LastSquareFrom != from && (int)LastSquareFrom != to && LastSquareFrom != Squares.None)
        {
            SquaresObj[(int)LastSquareFrom].gameObject.GetComponent<Image>().color = Color.white;
        }

        if ((int)LastSquareTo != to && (int)LastSquareTo != from && LastSquareTo != Squares.None)
        {
            SquaresObj[(int)LastSquareTo].gameObject.GetComponent<Image>().color = Color.white;
        }

        
        LastSquareFrom = (Squares)from;
        LastSquareTo = (Squares)to;

        
        UpdateBoard();
    }


    public void Undo() 
    {
        GameManager.Instance.PlayChessDownAudio(GameManager.Instance.clips[1]);
        UndoMove();
        SquaresObj[(int)LastSquareFrom].gameObject.GetComponent<Image>().color = Color.white;
        SquaresObj[(int)LastSquareTo].gameObject.GetComponent<Image>().color = Color.white;
        
        UpdateBoard();
    }
}
