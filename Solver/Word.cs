using System;
using System.Collections.Generic;
using System.Text;

namespace WordleHelper.Solver
{
    public class Word
    {
        private readonly string word;

        public Word(string word)
        {
            this.word = word ?? throw new Exception("Null Word");
            this.word = this.word.ToLower();
        }

        public char LetterAt(int pos)
        {
            if (pos < 0 || pos >= word.Length)
            {
                return default;
            }

            return word[pos];
        }

        public bool Contains(char letter)
        {
            return word.Contains(Char.ToLower(letter));
        }

        public bool DoesNotContain(char letter)
        {
            return !Contains(Char.ToLower(letter));
        }

        public override string ToString()
        {
            return word.ToString();
        }
    }

    public enum Result
    {
        Green = 'o',
        Yellow = 'm',
        Grey = 'n'
    }

    public abstract class Rule
    {
        protected readonly char letter;
        protected readonly int position;

        public Rule(char letter, int position)
        {
            this.letter = letter;
            this.position = position;
        }

        public abstract bool Matches(Word word);
    }

    public static class RuleFactory
    {
        public static Rule Create(char charResult, char letter, int pos)
        {
            var result = (Result)charResult;
            
            switch (result)
            {
                case Result.Green: return new GreenRule(letter, pos);
                case Result.Yellow: return new YellowRule(letter, pos);
                case Result.Grey: return new GreyRule(letter, pos);
            }

            throw new Exception("Invalid Result");
        }
    }

    public class YellowRule : Rule
    {
        public YellowRule(char letter, int position) : base(letter, position) { }

        public override bool Matches(Word word)
        {
            return word.Contains(letter) && word.LetterAt(position) != letter;
        }
    }

    public class GreyRule : Rule
    {
        public GreyRule(char letter, int position) : base(letter, position) { }

        public override bool Matches(Word word)
        {
            return word.DoesNotContain(letter);
        }
    }

    public class GreenRule : Rule
    {
        public GreenRule(char letter, int position) : base(letter, position) { }

        public override bool Matches(Word word)
        {
            return word.LetterAt(position) == letter;
        }
    }
}
