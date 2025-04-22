using System;
using System.Linq;

public class SudokuValidator
{
    public static void Main()
    {
        // Valid 9x9 Sudoku example
        int[][] goodSudoku = {
            new int[] {7,8,4, 1,5,9, 3,2,6},
            new int[] {5,3,9, 6,7,2, 8,4,1},
            new int[] {6,1,2, 4,3,8, 7,5,9},
            new int[] {9,2,8, 7,1,5, 4,6,3},
            new int[] {3,5,7, 8,4,6, 1,9,2},
            new int[] {4,6,1, 9,2,3, 5,8,7},
            new int[] {8,7,6, 3,9,4, 2,1,5},
            new int[] {2,4,3, 5,6,1, 9,7,8},
            new int[] {1,9,5, 2,8,7, 6,3,4}
        };

        int[][] badSudoku = {
            new int[] {7,8,4, 1,5,9, 3,2,6},
            new int[] {5,3,9, 6,7,2, 8,4,4}, // Duplicated 4
            new int[] {6,1,2, 4,3,8, 7,5,9},
            new int[] {9,2,8, 7,1,5, 4,6,3},
            new int[] {3,5,7, 8,4,6, 1,9,2},
            new int[] {4,6,1, 9,2,3, 5,8,7},
            new int[] {8,7,6, 3,9,4, 2,1,5},
            new int[] {2,4,3, 5,6,1, 9,7,8},
            new int[] {1,9,5, 2,8,7, 6,3,4}
        };

        Console.WriteLine("Testing valid Sudoku:");
        var validResult = ValidateSudoku(goodSudoku);
        Console.WriteLine(validResult.isValid ? "Valid Sudoku" : $"Invalid: {validResult.error}");

        Console.WriteLine("\nTesting invalid Sudoku:");
        var invalidResult = ValidateSudoku(badSudoku);
        Console.WriteLine(invalidResult.isValid ? "Valid Sudoku" : $"Invalid: {invalidResult.error}");
    }

    public static (bool isValid, string error) ValidateSudoku(int[][] sudoku)
    {
        if (sudoku == null || sudoku.Length == 0)
            return (false, "Grid is empty");

        int n = sudoku.Length;
        int sqrtN = (int)Math.Sqrt(n);

        if (sqrtN * sqrtN != n)
            return (false, $"Grid size {n}x{n} is invalid (must be perfect square)");

        for (int row = 0; row < n; row++)
        {
            if (sudoku[row] == null || sudoku[row].Length != n)
                return (false, $"Row {row + 1} has incorrect length");

            var rowError = ValidateSet(sudoku[row], n, $"Row {row + 1}");
            if (rowError != null)
                return (false, rowError);
        }

        for (int col = 0; col < n; col++)
        {
            var column = new int[n];
            for (int row = 0; row < n; row++)
                column[row] = sudoku[row][col];

            var colError = ValidateSet(column, n, $"Column {col + 1}");
            if (colError != null)
                return (false, colError);
        }

        for (int box = 0; box < n; box++)
        {
            var subgrid = new int[n];
            int index = 0;
            int startRow = (box / sqrtN) * sqrtN;
            int startCol = (box % sqrtN) * sqrtN;

            for (int i = 0; i < sqrtN; i++)
                for (int j = 0; j < sqrtN; j++)
                    subgrid[index++] = sudoku[startRow + i][startCol + j];

            var boxError = ValidateSet(subgrid, n, $"Box {box + 1}");
            if (boxError != null)
                return (false, boxError);
        }

        return (true, null);
    }

    private static string ValidateSet(int[] numbers, int n, string setName)
    {
        var invalidNumbers = numbers.Where(num => num < 1 || num > n).Distinct().ToList();
        if (invalidNumbers.Any())
            return $"{setName} contains invalid numbers: {string.Join(",", invalidNumbers)} (must be 1-{n})";

        var duplicates = numbers
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicates.Any())
            return $"{setName} contains duplicates: {string.Join(",", duplicates)}";

        return null;
    }
}