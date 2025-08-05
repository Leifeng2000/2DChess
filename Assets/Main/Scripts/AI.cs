using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class AI
{
    private Board board;
    public int ThinkingTime;
    private SMove[] Moves;

    private int NodesEvaluated;
    private int QNodes;
    private Stopwatch Watch;

    private float FailHigh;
    private float FailHighFirst;

    private bool RunOutOfTime;

    public AI(Board board) {
        this.board = board;
        Moves = new SMove[Defs.MaxMoves * 64];
    }


    //Iterative depending, search init
    public int SearchPosition()
    {


        int bestMove = 0;
        RunOutOfTime = false;
        ClearForSearch();

        Watch = new Stopwatch();
        Watch.Start();

        for (int i = 1; i <= 20; i++) 
        {
            AlphaBeta(-5000000, 5000000, i);
            if (RunOutOfTime == true) 
            {
                UnityEngine.Debug.Log("DEPTH " + (i-1));
                GameManager.Instance.PlayChessDownAudio(GameManager.Instance.clips[2]);
                break;
            }

            board.GetPVLine(i);
            bestMove = board.PvArray[0];
            NodesEvaluated = 0;
            QNodes = 0;
        }

        Watch.Stop();

        return bestMove;
    }

    public void ClearForSearch()
    {
        for (int i = 0; i < 14; i++) {
            for(int sq=0; sq<64; sq++){
                board.SearchHistory[i][sq] = 0;
            }
        }

        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < Defs.MaxDepth; j++) {
                board.SearchKillers[i][j] = 0;
            }
        }

        board.ClearPVTable();
        FailHigh = 0;
        FailHighFirst = 0;

    }

    public int AlphaBeta(int alpha, int beta, int depth)
    {
        if (depth == 0)
        {
            return Quiescence(alpha, beta, 63);
        }

        NodesEvaluated++;

        if (board.IsRepetition() || board.FiftyMove >= 100)
        return 0;
        CheckTime();
        
        if (RunOutOfTime) {
            return 0;
        }


        int OldAlpha = alpha;
        int BestMove = 0;
        int Score = -5000000;
        int MadeALegalMove = 0;

        int add = depth * Defs.MaxMoves;
        int num = MoveGen.GenerateMoves(board, Moves, add, depth) + add;
        int move;

        int PvMove = board.ProbePVTable();
        if (PvMove != 0)
        {
            for (int i = add; i < num; i++) {
                if (Moves[i].move == PvMove)
                {
                    Moves[i].score = 2000000;
                }
            }
        
        }

        for (int i = add; i < num; i++)
        {
            PickBestMove(i, num);
            move = Moves[i].move;
            board.MakeMove(move);
            if (!board.MoveWasIllegal())
            {
                MadeALegalMove++;
                Score = -AlphaBeta(-beta, -alpha, depth - 1);
                board.UndoMove();

                if (Score > alpha)
                {

                    if (Score >= beta)
                    {

                        if (MadeALegalMove == 1)
                        {
                            FailHighFirst++;
                        }
                        FailHigh++;

                        if (!move.IsCapture())
                        {
                            board.SearchKillers[1][depth] = board.SearchKillers[0][depth];
                            board.SearchKillers[0][depth] = move;
                        }

                        return beta;
                    }

                    if (!move.IsCapture())
                    {
                        board.SearchHistory[board.pieces[move.GetFrom()]][move.GetTo()] += depth;
                    }

                    alpha = Score;
                    BestMove = move;
                }
            }
            else
                board.UndoMove();
        }

        if (MadeALegalMove == 0)
        {

            if (board.IsAttacked(board.BKing, 1))
            {
                if ((int)board.SideToPlay == 0)
                {

                    return -32767 - depth;
                }

                return 32767 + depth;
            }
            else if (board.IsAttacked(board.WKing, 0))
            {
                if ((int)board.SideToPlay == 0)
                {


                    return 32767 + depth;
                }

                return -32767 - depth;

            }
            else
            {
                return 0;
            }

        }
        if (alpha != OldAlpha) {
            board.StorePVMove(BestMove);
        }


        return alpha;
    }

    public int Quiescence(int alpha, int beta, int depth) {

        int Score = (Evaluation.Evaluate(board) * ((board.SideToPlay * 2) - 1));
        QNodes++;

        CheckTime();

        if (RunOutOfTime)
        {
            return 0;
        }

        if (Score >= beta) {
            return beta;
        }

        if (Score > alpha) {
            alpha = Score;
        }

        if (depth <= 21) {
            return Score;
        }


        int MadeALegalMove = 0;
        Score = -5000000;
        
        int add = depth * Defs.MaxMoves;
        int num = MoveGen.GenerateCapturingMoves(board, Moves, add) + add; //Generates only capturing moves
        int move;

        for (int i = add; i < num; i++)
        {
            PickBestMove(i, num);
            move = Moves[i].move;

            board.MakeMove(move);
            if (!board.MoveWasIllegal())
            {
                MadeALegalMove++;
                Score = -Quiescence(-beta, -alpha, depth - 1);

                if (Score > alpha)
                {
                    if (Score >= beta)
                    {

                        if (MadeALegalMove == 1)
                        {
                            FailHighFirst++;
                        }
                        FailHigh++;

                        board.UndoMove();
                        return beta;
                    }

                    alpha = Score;
                }
            }
            board.UndoMove();
        }


        return alpha;
    }
    public void PickBestMove(int pos, int max) {
        SMove temp;
        int bestScore = -1;
        int bestIndex = pos;
        for (int i = pos; i < max; i++) {

            if (Moves[i].score >= bestScore) {
                bestIndex = i;
                bestScore = Moves[i].score;
            }
        }
        temp = Moves[pos];
        Moves[pos] = Moves[bestIndex];
        Moves[bestIndex] = temp;
    }
    public void CheckTime()
    {
        if (Watch.Elapsed.Seconds >= ThinkingTime)
        {
            RunOutOfTime = true;
        }
    }

}