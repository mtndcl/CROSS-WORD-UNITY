using System;
using System.Collections.Generic;


public class Cell
{


    private char key;

    private bool used;

    private int x;

    private int y;


    private bool is_intercell;
    public Cell(int x, int y, char key)
    {


        this.Is_intercell = false;
        this.x = x;
        this.y = y;
        this.Key = key;
        this.Used = false;
    }

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public char Key { get => key; set => key = value; }
    public bool Used { get => used; set => used = value; }
    public bool Is_intercell { get => is_intercell; set => is_intercell = value; }
}
public class Word
{


    private String value;

    private char direction;

    private List<Cell> cells;

    private bool added;
    public Word(String value)
    {
        Cells = new List<Cell>();
        this.Value = value;
        this.Added = false;
    }

    public string Value { get => value; set => this.value = value; }
    public char Direction { get => direction; set => direction = value; }
    public bool Added { get => added; set => added = value; }
    internal List<Cell> Cells { get => cells; set => cells = value; }
}

public class Location
{
    private int x;
    private int y;
    public Location(int x, int y)
    {
        this.X = x;
        this.Y = y;

    }

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
}

public class Option
{


    private Cell cell;

    private int victimpoint;

    public Option(Cell cell, int victimpoint)
    {
        this.Cell = cell;
        this.Victimpoint = victimpoint;

    }

    public int Victimpoint { get => victimpoint; set => victimpoint = value; }
    internal Cell Cell { get => cell; set => cell = value; }
}


public class Section
{

    private int min_X;
    private int min_Y;
    private int max_X;
    private int max_Y;
    private List<Cell> part;
    public Section()
    {
        Part = new List<Cell>();

    }

    public List<Cell> Part { get => part; set => part = value; }
    public int Min_X { get => min_X; set => min_X = value; }
    public int Min_Y { get => min_Y; set => min_Y = value; }
    public int Max_X { get => max_X; set => max_X = value; }
    public int Max_Y { get => max_Y; set => max_Y = value; }
}

