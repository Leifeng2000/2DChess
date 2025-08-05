using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Engine : MonoBehaviour {

    private Board board;
	public SMove[] Moves{get; private set;}
	public PromotePiece PromoteTo = PromotePiece.Queen;

    private AI search;

	public void InitChess(){
		InitChess("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 ");
	}

    public void InitChess(string FEN)
    {
		Magics.Init();
        Zobrist.Init();
        MvvLva.Init();
		
		board = new Board(FEN);
        Moves = new SMove[256];
        search = new AI(board);

        OnTurnSwitched();

	}

	public bool TryToMove(int From, int To){
		int num = MoveGen.GenerateMoves(board, Moves);
		for(int i =0; i<num; i++){
			if(Moves[i].move.GetFrom() == From && Moves[i].move.GetTo() == To){
				if(Moves[i].move.IsPromotion()){
					if(Moves[i].move.GetPromo() == (int)PromoteTo){
                        return MakeMove(Moves[i].move);
                    }
				
				}
				else{
                    return MakeMove(Moves[i].move);
				}
			}
		}
        return false;
	}

    private bool MakeMove(int move)
    {
        bool madeAMove = board.MakeMoveWithCheck(move);

        if (madeAMove) {
            OnTurnSwitched();
        }

        return madeAMove;
    }

    private IEnumerator PlayMove(int thinkingTime) {
        yield return null;
        search.ThinkingTime = thinkingTime;
        int myMove = search.SearchPosition();
        board.MakeMoveWithCheck(myMove);
        OnComputerPlayed(myMove.GetFrom(), myMove.GetTo());
        OnTurnSwitched();

    }

    public void ComputerPlay(int thinkingTime) {
        StartCoroutine(PlayMove(3));
    }

    public virtual void OnTurnSwitched() { 
        
    }

    public virtual void OnComputerPlayed(int from, int to)
    { 
    
    }

    public int SideToPlay() {
        return board.SideToPlay;
    }

    public Squares IsInCheck() {
        return board.IsInCheck();
    }

    public int GetPieceAt(int index) {
        return board.GetPieceAt(index);
    }

    public BoardState GameState() {
        return board.State;
    }

	public ulong GetPlayablePositions(int From){
		int num = MoveGen.GenerateMoves(board, Moves);
		ulong pos = 0;

		for(int i =0; i<num; i++){
			if(Moves[i].move.GetFrom() == From){
				pos |= Ops.Pow2[(int)Moves[i].move.GetTo()];
			}
		}


		return pos;

	}

    public string GetFen() {
        return FEN.FenFromBoard(board);
    }

    public void UndoMove() {
        board.UndoMove();
    }


}
