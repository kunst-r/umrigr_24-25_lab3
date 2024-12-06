using UnityEngine;

namespace ChessMainLoop
{
    public static class CheckStateCalculator
    {
        public static SideColor CalculateCheck(Piece[,] grid)
        {
            bool whiteCheck = false;
            bool blackCheck = false;
            int gridSize = grid.GetLength(0);

            for (int i = 0; i < gridSize; i++)
            {
                for(int j = 0; j < gridSize; j++)
                {
                    if (grid[i, j] == null)
                    {
                        continue;
                    }

                    if (grid[i, j].IsAttackingKing(i, j))
                    {
                        if (grid[i, j].PieceColor == SideColor.Black)
                        {
                            whiteCheck = true;
                        }
                        else
                        {
                            blackCheck = true;
                        }
                    }
                }            
            }

            return whiteCheck ? blackCheck ? SideColor.Both : SideColor.White : blackCheck ? SideColor.Black : SideColor.None;
        }

        #region Direction Lookup Tables
        private static readonly int[,] DiagonalLookup =
        {
           { 1, 1 },
           { 1, -1 },
           { -1, 1 },
           { -1, -1 }
        };

        private static readonly int[,] VerticalLookup =
        {
           { 1, 0 },
           { -1, 0 },
           { 0, 1 },
           { 0, -1 }
        };
        #endregion

        public static bool IsAttackingKingDiagonal(int row, int column, SideColor attackerColor)
        {
            return IsAttackingKingInDirection(row, column, DiagonalLookup, attackerColor);
        }

        public static bool IsAttackingKingVertical(int row, int column, SideColor attackerColor)
        {
            return IsAttackingKingInDirection(row, column, VerticalLookup, attackerColor);
        }

        private static bool IsAttackingKingInDirection(int row, int column, int[,] directionLookupTable, SideColor attackerColor)
        {
            /*
             * Potrebno je zamijeniti liniju "return false;" logikom za provjeru napada li figura s danog polja koordinatama row i column
             * neprijateljskog kralja ovisnom o danom smjeru napada figure koji je definiran directionLookupTable parametrom.
             */
            
            Piece attackingPiece = BoardState.Instance.GetField(row, column);
            for (int i = 0; i < directionLookupTable.GetLength(0); i++)
            {
                for (int j = 1;
                     BoardState.Instance.IsInBorders(row + j * directionLookupTable[i, 0],
                         column + j * directionLookupTable[i, 1]);
                     j++)
                {
                    int targetRow = row + j * directionLookupTable[i, 0];
                    int targetColumn = column + j * directionLookupTable[i, 1];
                    Piece targetPiece = BoardState.Instance.GetField(targetRow, targetColumn);

                    if (targetPiece != null)
                    {
                        // a piece is on that field
                        // if king => check
                        // if not => no check and break, we can't jump over
                        if (targetPiece.PieceColor != attackingPiece.PieceColor && targetPiece is King)
                        {
                            return true;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            
            return false;
        }

        public static bool IsEnemyKingAtLocation(int row, int column, int rowDirection, int columnDirection, SideColor attackerColor)
        {
            if (BoardState.Instance.IsInBorders(row + rowDirection, column + columnDirection))
            {
                Piece piece = BoardState.Instance.GetField(row + rowDirection, column + columnDirection);

                if (piece == null) 
                    return false;
                if (piece is King && piece.PieceColor != attackerColor) 
                    return true;
            }

            return false;
        }
    }
}