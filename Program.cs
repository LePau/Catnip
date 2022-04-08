/*
After catching your classroom students cheating before, you realize your students are getting craftier and hiding words in 2D grids of letters. The word may start anywhere in the grid, and consecutive letters can be either immediately below or immediately to the right of the previous letter.

Given a grid and a word, write a function that returns the location of the word in the grid as a list of coordinates. If there are multiple matches, return any one.
grid1 = [
  ['c', 'c', 'x', 't', 'i', 'b'],
  ['c', 'c', 'a', 't', 'n', 'i'],
  ['a', 'c', 'n', 'n', 't', 't'],
  ['t', 'c', 's', 'i', 'p', 't'],
  ['a', 'o', 'o', 'o', 'a', 'a'],
  ['o', 'a', 'a', 'a', 'o', 'o'],
  ['k', 'a', 'i', 'c', 'k', 'i'],
]
word1 = "catnip"
word2 = "cccc"
word3 = "s"
word4 = "bit"
word5 = "aoi"
word6 = "ki"
word7 = "aaa"
word8 = "ooo"

grid2 = [['a']]
word9 = "a"

find_word_location(grid1, word1) => [ (1, 1), (1, 2), (1, 3), (2, 3), (3, 3), (3, 4) ]
find_word_location(grid1, word2) =>
       [(0, 1), (1, 1), (2, 1), (3, 1)] x
    OR [(0, 0), (1, 0), (1, 1), (2, 1)] 
    OR [(0, 0), (0, 1), (1, 1), (2, 1)] x
    OR [(1, 0), (1, 1), (2, 1), (3, 1)] x
find_word_location(grid1, word3) => [(3, 2)]
find_word_location(grid1, word4) => [(0, 5), (1, 5), (2, 5)]
find_word_location(grid1, word5) => [(4, 5), (5, 5), (6, 5)]
find_word_location(grid1, word6) => [(6, 4), (6, 5)]
find_word_location(grid1, word7) => [(5, 1), (5, 2), (5, 3)]
find_word_location(grid1, word8) => [(4, 1), (4, 2), (4, 3)]
find_word_location(grid2, word9) => [(0, 0)]

Complexity analysis variables:

r = number of rows
c = number of columns
w = length of the word
*/
using System;
using System.Collections.Generic;
using System.Linq;

class Coordinate
{
    public int x = 0;
    public int y = 0;
    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }

    public bool IsEqual(Coordinate other)
    {
        return this.x == other.x && this.y == other.y;
    }
}


class Solution
{
    static void PrintCoordinates(List<List<Coordinate>> allCoordinates)
    {
        var strings = allCoordinates.Select(coordinates => $"[{String.Join(", ", coordinates)}]");
        Console.WriteLine(String.Join("\n OR ", strings));
    }

    static bool IsEqualCoordinates(List<Coordinate> left, List<Coordinate> right)
    {
        if (left.Count == 0 && right.Count == 0) return true;
        if (left.Count != right.Count) return false;

        for (int i = 0; i < left.Count; i++)
        {
            if (!left[i].IsEqual(right[i]))
            {
                return false;
            }
        }

        return true;
    }

    static List<Coordinate> FindCoordinatesHelper(char[][] grid, string word, int x, int y, bool yFirst)
    {
        var coordinates = new List<Coordinate>();

        var charToMatch = word[0];
        var charAtGrid = grid[y][x];
        var hasMoreCharactersInWord = word.Length > 1;
        var hasMoreX = grid[y].Length - 1 > x;
        var hasMoreY = grid.Length - 1 > y;

        if (charToMatch == charAtGrid)
        {

            if (hasMoreCharactersInWord)
            {
                //Console.WriteLine($"Looking for more, x:{x}, y:{y} xL:{grid[y].Length} yL:{grid.Length}");
                if (hasMoreX && !yFirst)
                {
                    coordinates = FindCoordinatesHelper(grid, word.Substring(1), x + 1, y, yFirst);
                }
                if (coordinates.Count == 0 && hasMoreY)
                {
                    coordinates = FindCoordinatesHelper(grid, word.Substring(1), x, y + 1, yFirst);
                }
                if (hasMoreX && yFirst && coordinates.Count == 0)
                {
                    coordinates = FindCoordinatesHelper(grid, word.Substring(1), x + 1, y, yFirst);
                }

                if (coordinates.Count == 0)
                {
                    return coordinates;
                }
            }

            coordinates = new List<Coordinate>() { new Coordinate(y, x) }.Concat(coordinates).ToList();
        }

        //Console.WriteLine($"match: {charToMatch}, grid: {charAtGrid}, x:{x}, y:{y}, coordinatesL: {coordinates.Count}");
        return coordinates;
    }

    static List<List<Coordinate>> FindCoordinates(char[][] grid, string word)
    {
        var x = 0;
        var y = 0;
        var allCoordinates = new List<List<Coordinate>>();
        Func<bool> yInBounds = () => y < grid.Length;
        Func<bool> xInBounds = () => yInBounds() && x < grid[y].Length;
        Func<List<Coordinate>, bool> addCoordinates = coordinates =>
        {
            var exists = allCoordinates.Any(existingCoordinates => IsEqualCoordinates(coordinates, existingCoordinates));
            if (!exists) allCoordinates.Add(coordinates);
            return true;
        };

        while (xInBounds() && yInBounds())
        {
            var coordinates = FindCoordinatesHelper(grid, word, x, y, yFirst: false);
            if (coordinates.Count > 0)
            {
                addCoordinates(coordinates);
            }

            coordinates = FindCoordinatesHelper(grid, word, x, y, yFirst: true);
            if (coordinates.Count > 0)
            {
                addCoordinates(coordinates);
            }

            //Console.WriteLine($"trying next {coordinates.Count}");
            x++;
            if (!xInBounds())
            {
                x = 0;
                y++;
            }
        }

        return allCoordinates;
    }


    static void Main(String[] args)
    {
        char[][] grid1 = new[] {
            new [] {'c', 'c', 'x', 't', 'i', 'b'},
            new [] {'c', 'c', 'a', 't', 'n', 'i'},
            new [] {'a', 'c', 'n', 'n', 't', 't'},
            new [] {'t', 'c', 's', 'i', 'p', 't'},
            new [] {'a', 'o', 'o', 'o', 'a', 'a'},
            new [] {'o', 'a', 'a', 'a', 'o', 'o'},
            new [] {'k', 'a', 'i', 'c', 'k', 'i'}
        };
        string word1 = "catnip";
        string word2 = "cccc";
        string word3 = "s";
        string word4 = "bit";
        string word5 = "aoi";
        string word6 = "ki";
        string word7 = "aaa";
        string word8 = "ooo";

        char[][] grid2 = new[] { new[] { 'a' } };
        string word9 = "a";

        PrintCoordinates(FindCoordinates(grid1, word1));
        PrintCoordinates(FindCoordinates(grid1, word2));
        PrintCoordinates(FindCoordinates(grid1, word3));
        PrintCoordinates(FindCoordinates(grid1, word4));
        PrintCoordinates(FindCoordinates(grid1, word5));
        PrintCoordinates(FindCoordinates(grid1, word6));
        PrintCoordinates(FindCoordinates(grid1, word7));
        PrintCoordinates(FindCoordinates(grid1, word8));

        PrintCoordinates(FindCoordinates(grid2, word9));


    }
}
