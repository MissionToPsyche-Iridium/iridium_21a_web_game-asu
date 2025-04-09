using System;

/* Helper class used to represent each winning condition lines and their indexes */
public class SolutionLine
{
    /* Indexes */
    int index1; // index 1
    int index2; // index 2
    int index3; // index 3

    int[] playingBoard; // board

    /* Constructor */
    public SolutionLine(int[] board, int whichLine)
    {
        playingBoard = board;
        switch (whichLine) // Case for all 8 solution lines
        {
            case 1:
                index1 = 0;
                index2 = 1;
                index3 = 2;
                break;
            case 2:
                index1 = 3;
                index2 = 4;
                index3 = 5;
                break;
            case 3:
                index1 = 6;
                index2 = 7;
                index3 = 8;
                break;
            case 4:
                index1 = 0;
                index2 = 3;
                index3 = 6;
                break;
            case 5:
                index1 = 1;
                index2 = 4;
                index3 = 7;
                break;
            case 6:
                index1 = 2;
                index2 = 5;
                index3 = 8;
                break;
            case 7:
                index1 = 0;
                index2 = 4;
                index3 = 8;
                break;
            case 8:
                index1 = 2;
                index2 = 4;
                index3 = 6;
                break;
            default:
                break;
        }
    }



    /* Functions */

    // Returns sum of SolutionLine
    public int Sum()
    {
        return playingBoard[index1] + playingBoard[index2] + playingBoard[index3];
    }

    // Return the index in the board of the winning move
    public int WinningMove()
    {
        if (playingBoard[index1] == 0)
            return index1;
        else if (playingBoard[index2] == 0)
            return index2;
        else
            return index3;
    }

    // Checks if there is a winner
    public Boolean HasWinner()
    {
        return Sum() == -3 || Sum() == 3;
    }

    // Returns who winner is
    public int Winner()
    {
        if (HasWinner())
            return playingBoard[index1];
        return 0;

    }
}