using UnityEngine;

namespace ChessMainLoop
{
    public class Pawn : Piece
    {
        public override void CreatePath()
        {
            /*
             * Potrebno je nadopuniti kod koji će stvoriti objekte za odabir polja za kretanje. Potrebno je po potrebni stvoriti 
             * polja za dijagonalni napad, En passant napad (https://en.wikipedia.org/wiki/En_passant), te polja za kretanje 
             * jedno i dva mjesta prema naprijed.
             */


            if (this.PieceColor == SideColor.White)
            {
                // move one field forward
                Piece enemyPieceInFront = BoardState.Instance.GetField(_row - 1, _column);
                if (enemyPieceInFront == null)
                {
                    PathManager.CreatePathInSpotDirection(this, -1, 0);
                }

                // move two fields forward
                if (enemyPieceInFront == null && _row == 6)
                {
                    Piece enemyPiece2FieldsInFront = BoardState.Instance.GetField(_row - 2, _column);
                    if (enemyPiece2FieldsInFront == null)
                    {
                        PathManager.CreatePathInSpotDirection(this, -2, 0);
                    }
                }

                // attack diagonally
                Piece enemyPieceDiagonalLeft = null;
                if (_column - 1 >= 0)
                    enemyPieceDiagonalLeft = BoardState.Instance.GetField(_row - 1, _column - 1);
                if (enemyPieceDiagonalLeft != null
                    && BoardState.Instance.IsInBorders(enemyPieceDiagonalLeft.Location.Row, enemyPieceDiagonalLeft.Location.Column)
                    && enemyPieceDiagonalLeft.PieceColor != this.PieceColor)
                {
                    PathManager.CreatePathInSpotDirection(this, -1, -1);
                }

                Piece enemyPieceDiagonalRight = null;
                if (_column + 1 <= 7)
                    enemyPieceDiagonalRight = BoardState.Instance.GetField(_row - 1, _column + 1);
                if (enemyPieceDiagonalRight != null
                    && BoardState.Instance.IsInBorders(enemyPieceDiagonalRight.Location.Row, enemyPieceDiagonalRight.Location.Column)
                    && enemyPieceDiagonalRight.PieceColor != this.PieceColor)
                {
                    PathManager.CreatePathInSpotDirection(this, -1, 1);
                }

                // en passant
                Piece enemyLeft = null;
                if (_column - 1 >= 0)
                    enemyLeft = BoardState.Instance.GetField(_row, _column - 1);
                if (enemyLeft != null && GameManager.Instance.Passantable == enemyLeft)
                {
                    CreatePassantSpace(-1, -1);
                }
                else
                {
                    Piece enemyRight = null;
                    if (_column + 1 <= 7)
                        enemyRight = BoardState.Instance.GetField(_row, _column + 1);
                    if (enemyRight != null && GameManager.Instance.Passantable == enemyRight)
                    {
                        CreatePassantSpace(-1, 1);
                    }
                }
                
                // PERSONALIZED ELEMENT: move 1 field backwards if free
                Piece pieceBehind = null;
                if (_row + 1 >= 0 && _row + 1 <= 7)
                {
                    pieceBehind = BoardState.Instance.GetField(_row + 1, _column);
                }

                if (pieceBehind == null)
                {
                    PathManager.CreatePathInSpotDirection(this, 1, 0);
                }
            }
            else if (this.PieceColor == SideColor.Black)
            {
                // move one field forward
                Piece enemyPieceInFront = BoardState.Instance.GetField(_row + 1, _column);
                if (enemyPieceInFront == null)
                {
                    PathManager.CreatePathInSpotDirection(this, 1, 0);
                }

                // move two fields forward
                if (enemyPieceInFront == null && _row == 1)
                {
                    Piece enemyPiece2FieldsInFront = BoardState.Instance.GetField(_row + 2, _column);
                    if (enemyPiece2FieldsInFront == null)
                    {
                        PathManager.CreatePathInSpotDirection(this, 2, 0);
                    }
                }

                // attack diagonally
                Piece enemyPieceDiagonalLeft = null;
                if (_column - 1 >= 0)
                    enemyPieceDiagonalLeft = BoardState.Instance.GetField(_row + 1, _column - 1);
                if (enemyPieceDiagonalLeft != null
                    && _column - 1 >= 0
                    && BoardState.Instance.IsInBorders(enemyPieceDiagonalLeft.Location.Row, enemyPieceDiagonalLeft.Location.Column)
                    && enemyPieceDiagonalLeft.PieceColor != this.PieceColor)
                {
                    PathManager.CreatePathInSpotDirection(this, 1, -1);
                }

                Piece enemyPieceDiagonalRight = null;
                if (_column + 1 <= 7)
                    enemyPieceDiagonalRight = BoardState.Instance.GetField(_row + 1, _column + 1);
                if (enemyPieceDiagonalRight != null
                    && _column + 1 <= 7
                    && BoardState.Instance.IsInBorders(enemyPieceDiagonalRight.Location.Row, enemyPieceDiagonalRight.Location.Column)
                    && enemyPieceDiagonalRight.PieceColor != this.PieceColor)
                {
                    PathManager.CreatePathInSpotDirection(this, 1, 1);
                }

                // en passant
                Piece enemyLeft = null;
                if (_column - 1 >= 0)
                    enemyLeft = BoardState.Instance.GetField(_row, _column - 1);
                if (enemyLeft != null && GameManager.Instance.Passantable == enemyLeft)
                {
                    CreatePassantSpace(1, -1);
                }
                else
                {
                    Piece enemyRight = null;
                    if (_column + 1 <= 7)
                        enemyRight = BoardState.Instance.GetField(_row, _column + 1);
                    if (enemyRight != null && GameManager.Instance.Passantable == enemyRight)
                    {
                        CreatePassantSpace(1, 1);
                    }
                }
                
                // PERSONALIZED ELEMENT: move 1 field backwards if free
                Piece pieceBehind = null;
                if (_row - 1 >= 0 && _row - 1 <= 7)
                {
                    pieceBehind = BoardState.Instance.GetField(_row - 1, _column);
                }

                if (pieceBehind == null)
                {
                    PathManager.CreatePathInSpotDirection(this, -1, 0);
                }
            }
        }

        private void CreateAttackSpace(int rowDirection, int columnDirection)
        {
            if (!BoardState.Instance.IsInBorders(_row + rowDirection, _column + columnDirection)) 
                return;
            Piece piece = BoardState.Instance.GetField(_row + rowDirection, _column + columnDirection);
            if (piece != null && piece.PieceColor != this.PieceColor)
            {
                PathManager.CreatePathInSpotDirection(this, rowDirection, columnDirection);
            }
        }

        private void CreatePassantSpace(int rowDirection, int columnDirection)
        {
            if (!BoardState.Instance.IsInBorders(_row, _column + columnDirection) == true) return;
            Piece piece = BoardState.Instance.GetField(_row, _column + columnDirection);
            if (piece != null && piece.PieceColor != PieceColor && piece == GameManager.Instance.Passantable)
            {
                PathManager.CreatePassantSpot(piece, _row + rowDirection, _column + columnDirection);
            }
        }

        /// <summary>
        /// Adds checks for making the piece passantable if it moved for two sapces and promoting the pawn if it reached the end of the board to Move method of base class.
        /// </summary>
        public override void Move(int newRow, int newColumn)
        {
            int oldRow = _row;

            base.Move(newRow, newColumn);

            if (Mathf.Abs(oldRow - newRow) == 2)
            {
                GameManager.Instance.Passantable = this;
            }
            
            if (PieceColor == SideColor.White && newRow == 0)
            {
                GameManager.Instance.PawnPromoting(this);
            }
            else if (PieceColor == SideColor.Black && newRow == BoardState.Instance.BoardSize - 1)
            {
                GameManager.Instance.PawnPromoting(this);
            }
            /*
            if (newRow == 0 || newRow == BoardState.Instance.BoardSize - 1)
            {
                GameManager.Instance.PawnPromoting(this);
            }
            */
        }

        public override bool IsAttackingKing(int row, int column)
        {
            int _direction = PieceColor == SideColor.Black ? 1 : -1;

            if (CheckStateCalculator.IsEnemyKingAtLocation(row, column, _direction, 1, PieceColor))
            {
                return true;
            }

            if (CheckStateCalculator.IsEnemyKingAtLocation(row, column, _direction, -1, PieceColor))
            {
                return true;
            }

            return false;
        }

        public override bool CanMove(int row, int column)
        {
            int _direction = PieceColor == SideColor.Black ? 1 : -1;

            //Following two sections perform checks if there are attackable units diagonally in looking direction of the pawn, and if moving to them would not resolve in a check for turn player
            if (BoardState.Instance.IsInBorders(row + _direction, column + 1))
            {
                Piece piece = BoardState.Instance.GetField(row + _direction, column + 1);
                if (piece != null && piece.PieceColor != PieceColor)
                {
                    if (GameEndCalculator.CanMoveToSpot(row, column, _direction, 1, PieceColor))
                    {
                        return true;
                    }
                }
            }

            if (BoardState.Instance.IsInBorders(row + _direction, column - 1))
            {
                Piece piece = BoardState.Instance.GetField(row + _direction, column - 1);
                if (piece != null && piece.PieceColor != PieceColor)
                {
                    if (GameEndCalculator.CanMoveToSpot(row, column, _direction, -1, PieceColor))
                    {
                        return true;
                    }
                }
            }

            //Following sections check if one in looking direction of the pawn is available for moving to
            if (!BoardState.Instance.IsInBorders(row + _direction, column)) 
                return false;
            if (BoardState.Instance.GetField(row + _direction, column) != null) 
                return false;

            if (GameEndCalculator.CanMoveToSpot(row, column, _direction, 0, PieceColor)) 
                return true;

            return false;
        }
    }
}
