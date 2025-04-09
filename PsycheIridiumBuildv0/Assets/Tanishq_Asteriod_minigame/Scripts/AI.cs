using System;
using System.Collections.Generic;

/* Creates AI move. */
public static class AI
{

    /* AI Functions */


    static System.Random random = new System.Random(); // used to randomize

    // Easy difficulty
    public static int Easy(int[] board)
    {
        List<int> legalMovesList = PossibleMoves(board);
        int randomMove = legalMovesList[random.Next((legalMovesList.Count))];
        return randomMove;
    }

    // Medium difficulty
    public static int Medium(int[] board, int whoseTurn)
    {
        if (whoseTurn == 0)
            return Minimize(board, int.MinValue, int.MaxValue, 2).Item1;
        else
            return Maximize(board, int.MinValue, int.MaxValue, 2).Item1;
    }

    // Hard difficulty
    public static int Hard(int[] board, int whoseTurn)
     {
        if (whoseTurn == 0)
            return Minimize(board, int.MinValue, int.MaxValue, 9).Item1;
        else
            return Maximize(board, int.MinValue, int.MaxValue, 9).Item1;
    }



    /* Minimax functions */


    // Minimax, used to calculate player X's best move
    private static Tuple<int, int> Minimize(int[] board, int alpha, int beta, int depth)
    {
        List<int> legalMovesList = LegalMoves(board, 0);

        int bestMoveSoFar = -1;
        int bestScoreSoFar = int.MaxValue;

        if (legalMovesList.Count == 0)
        {
            return new Tuple<int, int>(-1, Heuristic(board));
        }
        else if (depth == 0)
        {
            foreach (int i in legalMovesList)
            {
                int[] possibleBoard = (int[]) board.Clone();
                possibleBoard[i] = -1;
                int possibleScore = Heuristic(possibleBoard);
                if (possibleScore <= bestScoreSoFar)
                {
                    bestMoveSoFar = i;
                    bestScoreSoFar = possibleScore;
                    beta = Math.Min(beta, bestMoveSoFar);
                    if (beta <= alpha)
                        break;
                }
            }
        }
        else
        {
            foreach (int i in legalMovesList)
            {
                int[] possibleBoard = (int[])board.Clone();
                possibleBoard[i] = -1;
                Tuple<int, int> possibleMove = Maximize(possibleBoard, alpha, beta, depth - 1);
                if (possibleMove.Item2 <= bestScoreSoFar)
                {
                    bestMoveSoFar = i;
                    bestScoreSoFar = possibleMove.Item2;
                    beta = Math.Min(beta, bestMoveSoFar);
                    if (beta <= alpha)
                        break;
                }
            }
        }
        return new Tuple<int, int>(bestMoveSoFar, bestScoreSoFar);
    }

    // Minimax, used to calculate player O's best move
    private static Tuple<int, int> Maximize(int[] board, int alpha, int beta, int depth)
    {

        List<int> legalMovesList = LegalMoves(board, 1);

        int bestMoveSoFar = -1;
        int bestScoreSoFar = int.MinValue;

        if (legalMovesList.Count == 0)
        {
            return new Tuple<int, int>(-1, Heuristic(board));
        }
        else if (depth == 0)
        {
            foreach (int i in legalMovesList)
            {
                int[] possibleBoard = (int[])board.Clone();
                possibleBoard[i] = 1;
                int possibleScore = Heuristic(possibleBoard);
                if (possibleScore >= bestScoreSoFar)
                {
                    bestMoveSoFar = i;
                    bestScoreSoFar = possibleScore;
                    alpha = Math.Max(alpha, bestScoreSoFar);
                    if (beta <= alpha)
                        break;
                }
            }
        }
        else
        {
            foreach (int i in legalMovesList)
            {
                int[] possibleBoard = (int[])board.Clone();
                possibleBoard[i] = 1;
                Tuple<int, int> possibleMove = Minimize(possibleBoard, alpha, beta, depth - 1);
                if (possibleMove.Item2 >= bestScoreSoFar)
                {
                    bestMoveSoFar = i;
                    bestScoreSoFar = possibleMove.Item2;
                    alpha = Math.Max(alpha, bestScoreSoFar);
                    if (beta <= alpha)
                        break;
                }
            }
        }
        return new Tuple<int, int>(bestMoveSoFar, bestScoreSoFar);
    }



    /* Helper functions */


    // List of all open board spaces
    private static List<int> PossibleMoves(int[] board)
    {
        List<int> openSpots = new List<int>();
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == 0)
                openSpots.Add(i);
        }
        return openSpots;
    }

    // Gives legal moves based on 3 criteria
    // 1. If there is a winning move, return it.  Else
    // 2. If there is a move they need to block, return it.  Else
    // 3. Return possible moves
    private static List<int> LegalMoves(int[] board, int whoseTurn)
    {
        List<int> needToPlace = new List<int>();
        List<int> openSpot = PossibleMoves(board);

        SolutionLine[] solutionLines = GameController.SolutionLines(board);

        if (whoseTurn == 0)
        {
            for (int i = 0; i < solutionLines.Length; i++)
            {
                SolutionLine line = solutionLines[i];
                if (line.Sum() == -2)
                    return new List<int> {line.WinningMove()};
                else if (line.Sum() == 2)
                    needToPlace.Add(line.WinningMove());
            }
        }
        else if (whoseTurn == 1)
        {
            for (int i = 0; i < solutionLines.Length; i++)
            {
                SolutionLine line = solutionLines[i];
                if (line.Sum() == 2)
                    return new List<int> { line.WinningMove() };
                else if (line.Sum() == -2)
                    needToPlace.Add(line.WinningMove());
            }
        }
        if (needToPlace.Count != 0)
            return needToPlace;
        else
            return openSpot;
    }

    // Gives a score of the board
    // Negative --> better for Player X
    // Positive --> better for Player O
    private static int Heuristic(int[] board)
    {
        int score = 0;
        SolutionLine[] solutions = GameController.SolutionLines(board);
        for (int i = 0; i < solutions.Length; i++)
        {
            if (solutions[i].Sum() == -3)
            {
                score = int.MinValue;
                break;
            }
            else if (solutions[i].Sum() == 3)
            {
                score = int.MaxValue;
                break;
            }
            else if (solutions[i].Sum() == -2)
            {
                score += -100;
            }
            else if (solutions[i].Sum() == 2)
            {
                score += 100;
            }
            else
            {
                score += solutions[i].Sum();
            }
        }
        return score;
    }
}