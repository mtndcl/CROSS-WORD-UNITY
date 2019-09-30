using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Puzzle : MonoBehaviour
{


    public static List<String> Words=new List<String>();

    private static List<Word> input = new List<Word>();

    private static int size = 15;

    private static Cell[,] puzzle = new Cell[size, size];

    public static List<Section> sections = new List<Section>();

    private static GameObject HintPanel;

    private static TextAsset data = Resources.Load("data") as TextAsset;

    public static String[] leveldata = data.text.Split('\n');

    public static void SetQuestion(GameObject hintpanel)
    {

        HintPanel = hintpanel;
        hintpanel.GetComponent<Image>().color = new Color(GameData.color.r, GameData.color.g, GameData.color.b,200f/255f);
        GameObject succespanel = GameObject.FindGameObjectWithTag("succespanel");
        succespanel.transform.GetChild(0).GetComponent<Image>().color = GameData.color;
        succespanel.GetComponent<Image>().color =new Color(GameData.color.r, GameData.color.g, GameData.color.b, GameData.color.a/2);
        Set_Input();
        init_Puzzle();
        Set_Root();
        SuitWord();
        DivideMatrix();
        FitPart();
        Trim();
       

    }
    private static void SuitWord()
    {
        Word root;
        for (int i = 0; i < input.Count; i++)
        {
            root = input[i];

            for (int j = 0; j < input.Count; j++)
            {
                if (root.Direction == 'h' && input[j].Added == false)
                {
                    Add_New_Word_V(root, input[j]);
                }
                else if (root.Direction == 'v' && input[j].Added == false)
                {

                    Add_New_Word_H(root, input[j]);

                }


            }

        }
    }
    private static void FitPart()
     {

         for (int i = 0; i < sections.Count; i++)
         {
             if (sections[i].Part.Count > 4)
             {
                List<Cell> newpart = sections[i].Part;
                 sections.RemoveAt(i);
                 Section section0 = new Section();
                 Section section1 = new Section();
                 for (int j = 0; j < newpart.Count / 2; j++)
                 {
                     section0.Part.Add(newpart[j]);
                 }
                 for (int j = (newpart.Count / 2) ; j < newpart.Count; j++)
                 {
                     section1.Part.Add(newpart[j]);
                }
                 sections.Add(section0);
                 sections.Add(section1);

             }

         }
     }
    private static void DivideMatrix(){

        for (int i = 0; i < puzzle.GetLength(0); i++)
        {
            for (int j = 0; j < puzzle.GetLength(1); j++)
            {
                if (puzzle[i, j].Key != '-' && puzzle[i, j].Used == false)
                {
                    Section section = new Section();
                    sections.Add(section);
                    Dolan(i, j, section);
                }
            }
        }
    }
    private static void Dolan(int i, int j, Section section)
    {

        section.Part.Add(puzzle[i, j]);
    
        if (puzzle[i + 1, j].Key != '-' && puzzle[i + 1, j].Used == false)
        {

            puzzle[i, j].Used = true;
            if (puzzle[i, j + 1].Key != '-' && puzzle[i, j + 1].Used == false)
            {
                Dolan(i, j + 1, section);
            }
            else if (puzzle[i, j - 1].Key != '-' && puzzle[i, j - 1].Used == false)
            {
                Dolan(i, j - 1, section);
            }
            else if (puzzle[i, j - 1].Key != '-' && puzzle[i, j + 1].Key != '-' && puzzle[i, j - 1].Used == false && puzzle[i, j + 1].Used == false)
            {
                if (i % 2 == 0)
                {
                    Dolan(i, j - 1, section);
                }
                else
                {
                    Dolan(i, j + 1, section);
                }
            }
            else
            {
                Dolan(i + 1, j, section);
            }

        }
        else if (puzzle[i - 1, j].Key != '-' && puzzle[i - 1, j].Used == false)
        {

            puzzle[i, j].Used = true;

            if (puzzle[i, j + 1].Key != '-' && puzzle[i, j + 1].Used == false)
            {
                Dolan(i, j + 1, section);
            }
            else if (puzzle[i, j - 1].Key != '-' && puzzle[i, j - 1].Used == false)
            {
                Dolan(i, j - 1, section);
            }
            else if (puzzle[i, j - 1].Key != '-' && puzzle[i, j + 1].Key != '-' && puzzle[i, j - 1].Used == false && puzzle[i, j + 1].Used == false)
            {

                if (i % 2 == 0)
                {
                    Dolan(i, j - 1, section);
                }
                else
                {
                    Dolan(i, j + 1, section);
                }

            }
            else
            {

                Dolan(i - 1, j, section);
            }

        }
        else if (puzzle[i, j + 1].Key != '-' && puzzle[i, j + 1].Used == false)
        {

            puzzle[i, j].Used = true;
            if (puzzle[i + 1, j].Key != '-' && puzzle[i + 1, j].Used == false)
            {

                Dolan(i + 1, j, section);
            }
            else if (puzzle[i - 1, j].Key != '-' && puzzle[i - 1, j].Used == false)
            {
                Dolan(i - 1, j, section);
            }
            else if (puzzle[i - 1, j].Key != '-' && puzzle[i + 1, j].Key != '-' && puzzle[i - 1, j].Used == false && puzzle[i - 1, j].Used == false)
            {

                if (j % 2 == 0)
                {
                    Dolan(i - 1, j, section);
                }
                else
                {
                    Dolan(i + 1, j, section);
                }

            }
            else
            {

                Dolan(i, j + 1, section);
            }

        }
        else if (puzzle[i, j - 1].Key != '-' && puzzle[i, j - 1].Used == false)
        {
            puzzle[i, j].Used = true;

            if (puzzle[i + 1, j].Key != '-' && puzzle[i + 1, j].Used == false)
            {

                Dolan(i + 1, j, section);
            }
            else if (puzzle[i - 1, j].Key != '-' && puzzle[i - 1, j].Used == false)
            {
                Dolan(i - 1, j, section);
            }
            else if (puzzle[i - 1, j].Key != '-' && puzzle[i + 1, j].Key != '-' && puzzle[i - 1, j].Used == false && puzzle[i, i + 1].Used == false)
            {

                if (j % 2 == 0)
                {
                    Dolan(i - 1, j, section);
                }
                else
                {
                    Dolan(i + 1, j, section);
                }

            }
            else
            {

                Dolan(i, j - 1, section);
            }

        }
        else
        {

            puzzle[i, j].Used = true;

        }
    }
    private static void Trim()
    {

        int min_y = 100;
        int min_x = 100;
        int max_y = 0;
        int max_x = 0;
        for (int i = 0; i < sections.Count; i++)
        {

            min_y = 100;
             min_x = 100;
             max_y = 0;
             max_x = 0;
            for (int j = 0; j < sections[i].Part.Count; j++)
            {
                if (sections[i].Part[j].X < min_x)
                {

                    min_x = sections[i].Part[j].X;
                }
                if (sections[i].Part[j].Y < min_y)
                {

                    min_y = sections[i].Part[j].Y;
                }

                if (sections[i].Part[j].Y > max_y)
                {

                    max_y = sections[i].Part[j].Y;
                }

                if (sections[i].Part[j].X > max_x)
                {

                    max_x = sections[i].Part[j].X;
                }

            }

            sections[i].Min_X = min_x;
            sections[i].Min_Y = min_y;
            sections[i].Max_Y = max_y;
            sections[i].Max_X = max_x;

        }
    }
    private static void Add_New_Word_H(Word root, Word victim)
    {
        Cell intercell;

        List<Option> options = new List<Option>();
        for (int i = 0; i < root.Value.Length; i++)
        {
            for (int j = 0; j < victim.Value.Length; j++)
            {
                if (root.Value[i] == victim.Value[j] && victim.Added == false)
                {

                    intercell = root.Cells[i];
                    if (Can_Add_H(root, victim, intercell, j) && victim.Added == false)
                    {
                        Option option = new Option(intercell, j);
                        options.Add(option);
                    }
                }
            }
        }
        if (options.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, options.Count);
            Add_Horizantal(root, victim, options[index].Cell, options[index].Victimpoint);

            options.Clear();
        }

    }
    private static  void Add_Horizantal(Word root, Word victim, Cell intercell, int victimpoint)
    {

        String front = victim.Value.Substring(0, victimpoint);
        String back = victim.Value.Substring(victimpoint + 1, victim.Value.Length - victimpoint - 1);
        int x = intercell.X;
        int y = intercell.Y;

        front = Reverse(front);
        for (int i = 0; i < front.Length; i++)
        {
            y = y - i - 1;
            puzzle[x, y].Key = front[i];

            Cell cell = new Cell(x, intercell.Y - front.Length + i, front[front.Length - i - 1]);
            victim.Cells.Add(cell);
            y = intercell.Y;
        }
        x = intercell.X;
        y = intercell.Y;

        victim.Cells.Add(intercell);
        for (int i = 0; i < back.Length; i++)
        {
            y = y + i + 1;
            puzzle[x, y].Key = back[i];
            Cell cell = new Cell(x, y, back[i]);
            victim.Cells.Add(cell);
            y = intercell.Y;
        }

        victim.Added = true;
        victim.Direction = 'h';

    }
    private static bool Can_Add_H(Word root, Word victim, Cell intercell, int victimpoint)
    {
      
        List<Location> locations = new List<Location>();

        String front = victim.Value.Substring(0, victimpoint);
        String back = victim.Value.Substring(victimpoint + 1, victim.Value.Length - victimpoint - 1);

        int x = intercell.X;
        int y = intercell.Y;


        for (int i = 0; i < front.Length; i++)
        {
            y = y - i - 1;
            Location location = new Location(x, y);
            locations.Add(location);
            y = intercell.Y;
        }
        x = intercell.X;
        y = intercell.Y;
        for (int i = 0; i < back.Length; i++)
        {
            y = y + i + 1;
            Location location = new Location(x, y);
            locations.Add(location);
            y = intercell.Y;
        }


        char interchar = puzzle[intercell.X, intercell.Y].Key;
        puzzle[intercell.X, intercell.Y].Key = '-';
        for (int i = 0; i < locations.Count; i++)
        {


            ////Check the location in or out matrix
            if (locations[i].X <= 0 || locations[i].X >= size - 1 || locations[i].Y <= 0 || locations[i].Y >= size - 1)
            {
                puzzle[intercell.X, intercell.Y].Key = interchar;
                return false;
            }


            /////Check the matrix is avaible for the this word or not
            if (puzzle[locations[i].X + 1, locations[i].Y].Key != '-' || puzzle[locations[i].X - 1, locations[i].Y].Key != '-'
                || puzzle[locations[i].X, locations[i].Y + 1].Key != '-' || puzzle[locations[i].X, locations[i].Y - 1].Key != '-')
            {
                puzzle[intercell.X, intercell.Y].Key = interchar;
                return false;

            }


            if (puzzle[intercell.X, intercell.Y + 1].Key != '-' || puzzle[intercell.X, intercell.Y - 1].Key != '-')
            {
                puzzle[intercell.X, intercell.Y].Key = interchar;
                return false;
            }
         }
        puzzle[intercell.X, intercell.Y].Key = interchar;

        locations.Clear();
        return true;
    }
    private static void Add_New_Word_V(Word root, Word victim)
    {

        Cell intercell;

        List<Option> options = new List<Option>();
        for (int i = 0; i < root.Value.Length; i++)
        {
            for (int j = 0; j < victim.Value.Length; j++)
            {
                if (root.Value[i] == victim.Value[j] && victim.Added == false)
                {
                    intercell = root.Cells[i];
                    if (Can_Add_V(root, victim, intercell, j) && victim.Added == false)
                    {
                  
                        Option option = new Option(intercell, j);
                        options.Add(option);
                    }
                }
            }
        }

        if (options.Count > 0)
        {
           
            int index = UnityEngine.Random.Range(0, options.Count);
            Add_Vertical(root, victim, options[index].Cell, options[index].Victimpoint);

            options.Clear();

        }

    }
    private static  string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
    private static void Add_Vertical(Word root, Word victim, Cell intercell, int victimpoint)
    {

        String front = victim.Value.Substring(0, victimpoint);
        String back = victim.Value.Substring(victimpoint + 1, victim.Value.Length - victimpoint - 1);

        int x = intercell.X;
        int y = intercell.Y;

        front = Reverse(front);

        for (int i = 0; i < front.Length; i++)
        {
            x = x - i - 1;
            puzzle[x, y].Key = front[i];



            Cell cell = new Cell(intercell.X - front.Length + i, y, front[front.Length - i - 1]);


            victim.Cells.Add(cell);
            x = intercell.X;
        }



        x = intercell.X;
        y = intercell.Y;
        victim.Cells.Add(intercell);


        for (int i = 0; i < back.Length; i++)
        {
            x = x + i + 1;
            puzzle[x, y].Key = back[i];

            Cell cell = new Cell(x, y, back[i]);
            victim.Cells.Add(cell);
            x = intercell.X;
        }

        victim.Added = true;
        victim.Direction = 'v';
    }
    private static bool Can_Add_V(Word root, Word victim, Cell intercell, int victimpoint)
    {
        List<Location> locations = new List<Location>();

        String front = victim.Value.Substring(0, victimpoint);
        String back = victim.Value.Substring(victimpoint + 1, victim.Value.Length - victimpoint - 1);


        int x = intercell.X;
        int y = intercell.Y;
        for (int i = 0; i < front.Length; i++)
        {
            x = x - i - 1;
            Location location = new Location(x, y);
            locations.Add(location);
            x = intercell.X;
        }
        x = intercell.X;
        y = intercell.Y;
        for (int i = 0; i < back.Length; i++)
        {
            x = x + i + 1;
            Location location = new Location(x, y);
            locations.Add(location);
            x = intercell.X;
        }

        char interchar = puzzle[intercell.X, intercell.Y].Key;
        puzzle[intercell.X, intercell.Y].Key = '-';
        for (int i = 0; i < locations.Count; i++)
        {

            if (locations[i].X <= 0 || locations[i].X >= size - 1 || locations[i].Y <= 0 || locations[i].Y >= size - 1)
            {
                puzzle[intercell.X, intercell.Y].Key = interchar;
                return false;
            }


            if (puzzle[locations[i].X + 1, locations[i].Y].Key != '-' || puzzle[locations[i].X - 1, locations[i].Y].Key != '-'
                || puzzle[locations[i].X, locations[i].Y + 1].Key != '-' || puzzle[locations[i].X, locations[i].Y - 1].Key != '-')
            {
                puzzle[intercell.X, intercell.Y].Key = interchar;
                return false;

            }

            if (puzzle[intercell.X + 1, intercell.Y].Key != '-' || puzzle[intercell.X - 1, intercell.Y].Key != '-')
            {
                puzzle[intercell.X, intercell.Y].Key = interchar;
                return false;
            }
        }
        puzzle[intercell.X, intercell.Y].Key = interchar;

        locations.Clear();
        return true;
    }
    private static void Set_Root()
    {
        int x = puzzle.GetLength(0) / 2;
        int y = (puzzle.GetLength(1) - input[0].Value.Length) / 2;


        input[0].Direction = 'h';
        input[0].Added = true;
        for (int i = 0; i < input[0].Value.Length; i++)
        {
            puzzle[x, y].Key = input[0].Value[i];

            Cell cell = new Cell(x, y, input[0].Value[i]);
            input[0].Cells.Add(cell);


            y++;
        }


    }
    private static void Set_Input()
    {
        Words.Clear();
        sections.Clear();
        input.Clear();

        String line = leveldata[GameData.level-1].Trim();


       
            String[] tokens = line.Split(':');
            HintPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "HINT : " + tokens[0];

            print("line |" + line + "|");
            String[] thewords = tokens[1].Split(',');

            foreach (String wor in thewords)
            {
                Words.Add(wor);
            }

            for (int i = 0; i < Words.Count; i++)
            {
                Word word0 = new Word(Words[i]);
                input.Add(word0);
            }
    }
    private static void print_Puzzle()
    {

        Console.Write("    ");
        for (int j = 0; j < puzzle.GetLength(1); j++)
        {

            Console.Write(j + "   ");
        }
        Console.WriteLine();
        for (int i = 0; i < puzzle.GetLength(0); i++)
        {

            Console.Write(i + "   ");
            for (int j = 0; j < puzzle.GetLength(1); j++)
            {

                Console.Write(puzzle[i, j].Key + "   ");
            }
            Console.WriteLine();
        }
    }
    private static void init_Puzzle()
    {
        for (int i = 0; i < puzzle.GetLength(0); i++)
        {
            for (int j = 0; j < puzzle.GetLength(1); j++)
            {
                puzzle[i, j] = new Cell(i, j, '-');

            }

        }
    }


}
