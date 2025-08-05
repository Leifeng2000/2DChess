public static class FEN
{

    public static string Default = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

	public static void BoardFromFen(this Board board, string FeN){
        int i = 0, j = 0;
        char letter;

        string[] FENData = FeN.Split(' ');

        while (i < 64 && j < FENData[0].Length)
        {

            letter = FENData[0][j];

            switch (letter)
            {
                case 'p': board.BPawn |= Ops.Pow2[i]; board.pieces[i] = Defs.BPawn; break;
                case 'n': board.BKnight |= Ops.Pow2[i]; board.pieces[i] = Defs.BKnight; break;
                case 'b': board.BBishop |= Ops.Pow2[i]; board.pieces[i] = Defs.BBishop; break;
                case 'r': board.BRook |= Ops.Pow2[i]; board.pieces[i] = Defs.BRook; break;
                case 'q': board.BQueen |= Ops.Pow2[i]; board.pieces[i] = Defs.BQueen; break;
                case 'k': board.BKing |= Ops.Pow2[i]; board.pieces[i] = Defs.BKing; break;
                case 'P': board.WPawn |= Ops.Pow2[i]; board.pieces[i] = Defs.WPawn; break;
                case 'N': board.WKnight |= Ops.Pow2[i]; board.pieces[i] = Defs.WKnight; break;
                case 'B': board.WBishop |= Ops.Pow2[i]; board.pieces[i] = Defs.WBishop; break;
                case 'R': board.WRook |= Ops.Pow2[i]; board.pieces[i] = Defs.WRook; break;
                case 'Q': board.WQueen |= Ops.Pow2[i]; board.pieces[i] = Defs.WQueen; break;
                case 'K': board.WKing |= Ops.Pow2[i]; board.pieces[i] = Defs.WKing; break;
                case '/': i--; break;
                case '1': break;
                case '2': i++; break;
                case '3': i += 2; break;
                case '4': i += 3; break;
                case '5': i += 4; break;
                case '6': i += 5; break;
                case '7': i += 6; break;
                case '8': i += 7; break;
            }

            j++;
            i++;
        }
        if (FENData.Length > 1)
        {
            letter = FENData[1][0];
			if (letter == 'w') board.SideToPlay = 1;
			else if (letter == 'b') board.SideToPlay = 0;
        }

        if (FENData.Length > 2)
        {
            if (FENData[2].Contains("K")) board.CastlePermission |= Defs.CastleRightsKWCa;
            if (FENData[2].Contains("Q")) board.CastlePermission |= Defs.CastleRightsQWCa;
            if (FENData[2].Contains("k")) board.CastlePermission |= Defs.CastleRightsKBCa;
            if (FENData[2].Contains("q")) board.CastlePermission |= Defs.CastleRightsQBCa;
        }

		board.EnPassantSq = Squares.None;

        if (FENData.Length > 3) {

            if (FENData[3].Length > 1)
            {

                int y = (int)FENData[3][1] - 49;

                if (y == 2 || y == 5)
                {

                    int x = (int)FENData[3][0];
                    x -= 97;

                    if ((x >= 0 && x < 8))
                    {
						board.EnPassantSq = (Squares)(Defs.Mirror64[x + y * 8]);
                    }
                }
            }
        }
    }

    public static string FenFromBoard(this Board board) {

        string fen = "";
        char[] pieces = new char[13]{
            'x', 'P', 'N', 'B', 'R', 'Q', 'K',
            'p', 'n', 'b', 'r', 'q', 'k',
        };

        int count = 0;
        
        for (int i = 0; i < 64; i++) {
            
			int piece = board.GetPieceAt(i);

			if (piece == 0)
            {
                count++;
            }
            else
            {
                if (count > 0)
                {
                    fen += count;
                    count = 0;
                }

                fen += pieces[piece];
            }

            if ((i + 1) % 8 == 0 && i+1!=64)
            {

                if (count > 0)
                {
                    fen += count;
                    count = 0;
                }

                fen += '/';
            }
        }


        char[] side = new char[2] { 'b', 'w' };

        fen += " " + side[board.SideToPlay] + " ";

        if ((board.CastlePermission & Defs.CastleRightsKWCa) != 0)
            fen += "K";
        if ((board.CastlePermission & Defs.CastleRightsQWCa) != 0)
            fen += "Q";
        if ((board.CastlePermission & Defs.CastleRightsKBCa) != 0)
            fen += "k";
        if ((board.CastlePermission & Defs.CastleRightsQBCa) != 0)
            fen += "q";

        if (board.EnPassantSq != Squares.None)
            fen += " " + (board.EnPassantSq).ToString().ToLower();
        else
            fen += " - ";

        return fen;
    }
    

}
