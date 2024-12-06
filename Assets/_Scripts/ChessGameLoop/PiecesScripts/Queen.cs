namespace ChessMainLoop
{
    public class Queen : Piece
    {    
        public override void CreatePath()
        {
            /*
             * Nadopunite kod za stvaranje objekata za odabir polja koji prati logiku figure kraljice.
             */
            
            PathManager.CreateVerticalPath(this);
            PathManager.CreateDiagonalPath(this);
        }

        public override bool IsAttackingKing(int row, int column)
        {
            /*
             * Zamijenite liniju return false; sa kodom za provjeru napada li kraljica kralja sa trenutnog polja.
            */
            
            bool verticalCheck = CheckStateCalculator.IsAttackingKingVertical(row, column, PieceColor);
            bool diagonalCheck = CheckStateCalculator.IsAttackingKingDiagonal(row, column, PieceColor);
            return verticalCheck || diagonalCheck;
        }

        public override bool CanMove(int row, int column)
        {
            /*
             * Zamijenite liniju return false; sa kodom za provjeru ima li kraljica preostalih dopuštenih poteza.
            */

            bool canMoveVertical = GameEndCalculator.CanMoveVertical(row, column, PieceColor);
            bool canMoveDiagonal = GameEndCalculator.CanMoveDiagonal(row, column, PieceColor);
            return canMoveVertical || canMoveDiagonal;
        }
    }
}