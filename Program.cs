/***********************************************************************
 * Task: Letter Game
 *
 * q  w  e  r  t  y  u  i  o  p 
 *  7  6  1  2  2  5  4  1  3  5
 *  a  s  d  f  g  h  j  k  l
 *   2  1  4  6  5  5  7  6  3
 *   z  x  c  v  b  n  m
 *    7  7  4  6  5  2  5
 *    
 * Figure 1: Each of the 26 lowercase letters and its value
 *
 * Letter games are popular at home and on television. In one version of
 * the game, every letter  has a value, and you  collect letters to form 
 * one or more words  giving the highest possible score. Unless you have
 * `a way with words', you  will try  all the words you know,  sometimes 
 * looking up the spelling, and then compute the scores. Obviously, this
 * can be done more accurately by computer.
 * 
 * Given  the  values  in Figure 1,  a list of  English  words, and  the 
 * letters  collected: find  the highest scoring words or pairs of words 
 * that can be formed.
 * 
 * Input Data
 * The input file INPUT.TXT contains one line with a string of lowercase
 * letters (from `a' to `z'): the letters collected. The string consists
 * of  at  least 3  and  at  most 7  letters  in  arbitrary  order.  The 
 * `dictionary' file WORDS.TXT  consists of at most 40,000 input. At the
 * end of this file is  a line with  a  single period (`.'). Each of the 
 * other  input contains a string of at least 3  and at most 7 lowercase
 * letters. The file WORDS.TXT  is sorted alphabetically and contains no
 * duplicates.
 * 
 * Output Data
 * On the first line  of file  OUTPUT.TXT, your program should write the
 * highest  score (Subtask A), and  on each of the  following input, all 
 * the  words  and/or word  pairs from  file  WORDS.TXT with  this score 
 * (Subtask B). A  letter  must  not occur more often in  an output line
 * than in the input line. Use the letter values given in Figure 1. When 
 * a combination of  two words can be formed with the given letters, the 
 * words should be printed on the same line separated by a space. Do not 
 * duplicate  pairs; for example, `rag prom' and `prom rag' are the same 
 * pair,  therefore only one  of them  should  be written. A  pair in an 
 * output line may consist of two identical words.
 *
 * Example Input and Output
 * Figure 2 gives example input and output.
 * _____________ _____________ ______________
 * | WORDS.TXT | | INPUT.TXT | | OUTPUT.TXT |
 * |___________| |___________| |____________|
 * | profile   | | prmgroa   | | 24         |
 * | program   | |___________| | program    |
 * | prom      |               | prom rag   |
 * | rag       |               |____________|
 * | ram       |
 * | rom       |
 * | .         |
 * |___________|
 *
 * Figure 2: Example input and output
 */
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic; // List
////----****####,,,,\\\\^^^^....>>>>////----3333````<<<<####----
using String   = System.Collections.Generic.List<char>; // thats
using Sentance = System.Collections.Generic.List<string>;
namespace CS {
  class Perms { // Algorithm L (lexicographic, Knuth)
    int n;        // number of lmnts
    List<char> a; // reference
    void Swap(int i, int j) {
      char temp = a[i];
      a[i] = a[j];
      a[j] = temp;
    }
    public Perms(List<char> @ref) {
      a = @ref;
      n = a.Count;
      // add Sentinel
      a.Insert(0, '=');
    }
    public bool Next() {
      // find j
      int j = n - 1;
      for (; a[j + 1] <= a[j]; --j) {}
      if (j == 0) return false; // no moar
      // find the first bigger than [j] and swap
      int i = n;
      for (; a[i] <= a[j]; --i) {}
      Swap(i, j);
      // reverse a[j + 1:n]
      i = j + 1;
      j = n;
      while (i < j) {
        Swap(i++, j--);               
      }
      return true;
    }
    override public string ToString() {
      return string.Join("", a.Skip(1));
    }
  }
  class Combinat {
    readonly int n;
    readonly int t;
    int[] c;
    public const int MINT = 3; // minimum word size
    public Combinat(int n, int t) {
      this.n = n;
      this.t = t;
      c = new int[t + 3];
      c[0] = 0;
      for (int j = 1; j <= t; ++j) {
        c[j] = j - 1;
      }
      c[t + 1] = n;
      c[t + 2] = 0;
    }
    // Again this is from 4A
    public bool Next() {
      // find j
      int j = 1;
      for (; c[j] + 1 == c[j + 1]; ++j)
        ;
      if (t < j) return false; // no moar
      // increment
      ++c[j];
      return true;
    }
    public IEnumerable<int> C()
    {
      return c.Skip(1).Take(t);
    }
    public override string ToString() {
      return "[" + string.Join(", ", C()) + "]";
    }
  }
  class LookUp {
    const int TABSIZ = 127;
    List<string>[] Bucket;
    public static uint Hash(string s) { // sdbm
      uint hash = 0;
      foreach (char c in s) {
        hash = c + (hash << 6) + (hash << 16) - hash;
      }
      return hash%TABSIZ;
    }
    public LookUp() {
      Bucket = new List<string> [TABSIZ];
      for (int i = 0; i < TABSIZ; ++i) {
        Bucket[i] = new List<string> ();
      }
    }
    // return false on duplicate
    public bool Push(string s) {
      uint i = Hash(s);
      if (Bucket[i].Contains(s)) return false; 
      Bucket[i].Add(s);
      return true;
    }
    public bool Find (string s)
    {
      return Bucket[Hash(s)].Contains(s);
    }
  }
  class Alphabet {
    static readonly int[] values = {
   // a b c d e f g h i j k l m
      2,5,4,4,1,6,5,5,1,7,6,3,5,
      2,3,5,7,2,1,2,4,6,6,7,5,7,
   // n o p q r s t u v w x y z
    };
    public static int GetValue(string s) {
      int value = 0;
      foreach (char c in s) {
        value += values[c - 'a'];
      }
      return value;
    }
    public static void Sort(List<char> s) 
      /* Straight Insert Sort */
    {   // j - loop
      for (int j = 1; j < s.Count; ++j) 
      {   // set i and key
        int i = j - 1;
        char key = s[j];
        for (; -1 < i && key < s[i]; --i)
          s[i + 1] = s[i];
        s[i + 1] = key;
      }
    }
    public static List<char> Extract(IEnumerable<int> c, 
        List<char> residue)
      /* : [Ok] c is a list with indices to extract from
         residue, which I think is the opposite of 
         extract, the remaining substance in residue can be
         jused to looking for pair words. */
    {
      List<char> extract = new List<char> ();
      foreach (int j in c.Reverse()) 
      {
        extract.Insert(0, residue[j]);
        residue.RemoveAt(j);
      }
      return extract;
    }
  }
  class Testo {
    public static void Boom()
    {
      IEnumerable<int> c = new int[] { 0, 2, 6 };
      Match match = new Match (c);
    }
  }
  class Match {
    const int SPLITZ = 2;
    String[] split; // extract and residue
    //
    public static LookUp dict = new LookUp();
    public static LookUp ckck = new LookUp(); // check check
    public static String letters;
    public int[]      Value { get; private set; }
    public Sentance[] Words { get; private set; }
    //
    override public string ToString ()
    { // Dunfing
      return string.Format ("({0}, {1}), [{2}, {3}]",
                            Value[0], Value[1],
                            string.Join(", ", Words[0]),
                            string.Join(", ", Words[1]));
    }
    public Match(IEnumerable<int> c)
    {
      String r = new String (letters);
      String e = Alphabet.Extract (c, r);
      split = new String[] { e, r };
      Console.WriteLine ("Matching {0} and {1}",
                         string.Join("", e),
                         string.Join("", r));
      Value = new int [SPLITZ] { 0, 0 };
      Words = new Sentance [SPLITZ];
      for (int j = 0; j < SPLITZ; ++j)
      {
        GetWords (j);
        if (0 < Words[j].Count)
        {
          Value[j] = Alphabet.GetValue (Words[j][0]);
          Console.WriteLine ("{0}: {1}", Value[j],
                             string.Join (", ", Words[j]));
        }
      }
      // There is one ugly case if we have 7 letters and a
      // 3 + 3 match, but I didn't find beautiful solution:)
      if (0 < Value[0] && Value[1] == 0 && Combinat.MINT < r.Count)
      {
        r.RemoveAt (0); // discard = Sentinel
        Alphabet.Sort (r);
        Console.WriteLine ("Here we are... " + string.Join ("", r));
        for (int j = 0; j < r.Count; ++j)
        {
          var q = new String (r);
          q.RemoveAt (j);
          Perms p = new Perms (q);
          int maxv = 0;
          do {
            string s = p.ToString ();
            if (dict.Find (s) == false) continue;
            if (Words[0].Contains (s)) continue;
            int value = Alphabet.GetValue (s);
            if (value < maxv) continue;
            if (maxv < value) {
              maxv = value;
              Words[1].Clear ();
            }
            Words[1].Add (s);
          } while (p.Next ());
        }
        if (0 < Words[1].Count)
        {
          Value[1] = Alphabet.GetValue (Words[1][0]);
          Console.WriteLine ("{0}: {1}", Value[1],
                             string.Join (", ", Words[1]));
        } else {
          Console.WriteLine ("No 3+3 match");
        }
      }
    }
    public void GetWords (int j)
    {
      Words[j] = new Sentance ();
      if (split[j].Count < Combinat.MINT) return;
      Perms p = new Perms (split[j]);
      do {
        string s = p.ToString ();
        if (ckck.Push (s) == false) continue;
        if (dict.Find (s) == false) continue;
        Words[j].Add (s);
      } while (p.Next ());
    }
    public int Total () 
    {
      return Value[0] + Value[1];
    }
  }
  class LetterGame {
    static bool TES_ING = false;
    static void LoadDict() {
      string[] words = File.ReadAllLines("WORDS.TXT");
      foreach (string s in words) {
        if (s[0].Equals('.')) break;
        Console.WriteLine("{0}: {1}", s, LookUp.Hash(s));
        Match.dict.Push(s);
      }
    }
    static void LoadLetters()
    {
      string[] lines = File.ReadAllLines("INPUT.TXT");
      Match.letters = new List<char> (lines[0]);  
      Alphabet.Sort(Match.letters);
      Console.WriteLine("Letters: " + string.Join("", Match.letters));
    }
    static void GetMax ()
    {
      int n = Match.letters.Count;
      int maxv = 0;
      List<Match> ls = new List<Match> ();
      for (int t = n; Combinat.MINT <= t; --t) {
        Combinat combi = new Combinat (n, t);
        do {
          Console.WriteLine (combi);
          Match match = new Match (combi.C ());
          int total = match.Total ();
          if (total < maxv) continue;
          if (maxv < total) {
            maxv = total;
            ls.Clear ();
          }
          ls.Add (match);
        } while (combi.Next ());
      }
      Console.WriteLine ("=== Max ===: {0}", maxv);
      foreach (var m in ls) {
        Console.WriteLine (m);
      }
    }
    static void Main(string[] args) {
      Console.WriteLine("Letter Game");
      LoadDict();
      LoadLetters();
      if (TES_ING)
      {
        Testo.Boom();
        goto Finish; // thats I like!
      }
      GetMax ();
Finish:
      Console.WriteLine("DONE");
    }
  }
}
////////////////////////////////////////////////////////////////////////
// log: Ce4koBa, Moke ga npeBekgaE Ha Mos 4oBek. Ako uckaTe ga Mu kaKeTe
// Hemo ga 3HaeTe 4e ocBeH HaFuoHa7 reorpa8uK Ha675gaBaM u 6pa3gu.
