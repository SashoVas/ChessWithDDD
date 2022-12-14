namespace Shared.Domain
{
    public static class DomainConstants
    {
        public const string DefaultBoardStartPositionFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        public const string EmptyBoardFen = "8/8/8/8/8/8/8/8";
        public const int DefaultBoardRows = 8;
        public const int DefaultBoardCols = 8;
        public const int MinNameLength = 3;
        public const int MaxNameLength = 1000;

        public const string KnightName = "knight";
        public const string BishopName = "bishop";
        public const string RookName = "rook";
        public const string PawnName = "pawn";
        public const string KingName = "king";
        public const string QueenName = "queen";

        public const string KnightIdentifier = "n";
        public const string BishopIdentifier = "b";
        public const string RookIdentifier = "r";
        public const string PawnIdentifier = "p";
        public const string KingIdentifier = "k";
        public const string QueenIdentifier = "q";

        public const int MinTurnDurationInMinutes = 1;
        public const int MaxTurnDurationInMinutes = 90;
        public const int MinIncrementAmounthInSeconds = 0;
        public const double MaxIncrementAmounthPercentageFromGameDuration = 0.05;

        public const int DefaultGameDurationMinutes = 30;
        public const int DefaultGameDurationHours = 0;
        public const int DefaultGameDurationSeconds = 0;

        public const int DefaultIncrementTimeSeconds=0;
        public const int DefaultIncrementTimeMinutes = 0;
        public const int DefaultIncrementTimeHours = 0;
    }
}
