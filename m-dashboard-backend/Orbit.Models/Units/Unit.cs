
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orbit.Models.Units
{
    public class Unit : BaseEntity
    {

        public virtual string Symbol { get; set; }
        public virtual double Multiplicand { get; set; }
        public virtual double Addend { get; set; }
        public virtual Quantity Quantity { get; set; }
        public virtual IList<Multiplier> Multipliers { get; set; }
        public virtual string IQuantityRef { get; set; }
        public virtual string GetSymbol(Multiplier multiplier)
        {
            string symbol = this.Symbol;

            if (multiplier != null)
            {
                //if we have a '\' or '/' stroke in the unit symbol we break it up into parts
                string firstPart = string.Empty;
                string secondPart = string.Empty;
                string delimeter = string.Empty;
                int Index;
                if (this.Symbol.IndexOf(BACKSTROKE) > 0 || this.Symbol.IndexOf(FRONTSTROKE) > 0)
                {
                    Index = this.Symbol.IndexOf(BACKSTROKE);
                    if (Index > 0)
                    {
                        firstPart = Symbol.Substring(0, Index);
                        secondPart = Symbol.Substring(Index + 1, this.Symbol.Length - Index - 1);
                        delimeter = BACKSTROKE.ToString();
                    }
                    else if (this.Symbol.IndexOf(FRONTSTROKE) > 0)
                    {
                        Index = this.Symbol.IndexOf(FRONTSTROKE);
                        firstPart = Symbol.Substring(0, Index);
                        secondPart = Symbol.Substring(Index + 1, this.Symbol.Length - Index - 1);
                        delimeter = FRONTSTROKE.ToString();
                    }


                }
                else
                {
                    firstPart = this.Symbol;
                }

                string topMultiplier = string.Empty;
                string bottomMultiplier = string.Empty;
                if (multiplier.MultiplierSymbol.IndexOf(BACKSTROKE) > 0 || multiplier.MultiplierSymbol.IndexOf(FRONTSTROKE) > 0)
                {
                    Index = multiplier.MultiplierSymbol.IndexOf(BACKSTROKE);
                    if (Index > 0)
                    {
                        topMultiplier = multiplier.MultiplierSymbol.Substring(0, Index);
                        bottomMultiplier = multiplier.MultiplierSymbol.Substring(Index + 1, multiplier.MultiplierSymbol.Length - Index - 1);

                    }
                    else if (multiplier.MultiplierSymbol.IndexOf(FRONTSTROKE) > 0)
                    {
                        Index = multiplier.MultiplierSymbol.IndexOf(FRONTSTROKE);
                        topMultiplier = multiplier.MultiplierSymbol.Substring(0, Index);
                        bottomMultiplier = multiplier.MultiplierSymbol.Substring(Index + 1, multiplier.MultiplierSymbol.Length - Index - 1);
                    }

                    bool reset = false;
                    foreach (var ch in topMultiplier)
                    {
                        if (Char.IsDigit(ch))
                        {
                            reset = true;
                            break;
                        }
                    }
                    if (reset)
                    {
                        topMultiplier = string.Empty;
                    }

                    reset = false;
                    foreach (var ch in bottomMultiplier)
                    {
                        if (Char.IsDigit(ch))
                        {
                            reset = true;
                            break;
                        }
                    }
                    if (reset)
                    {
                        bottomMultiplier = string.Empty;
                    }
                }
                else
                {
                    topMultiplier = multiplier.MultiplierSymbol;
                }

                firstPart = AddMultiplierSymbol(firstPart, topMultiplier);
                secondPart = AddMultiplierSymbol(secondPart, bottomMultiplier);
                symbol = firstPart + delimeter + secondPart;
            }

            return symbol;
        }
        public override string ToString() => Symbol;
        private string AddMultiplierSymbol(string unitSymbol, string multSymbol)
        {
            string symbol = string.Empty;
            bool addParenthesis = false;
            if (unitSymbol.Contains(LEFT_PARENTHESIS) && unitSymbol.Contains(RIGHT_PARENTHESIS))
            {
                addParenthesis = true;
            }
            else
            {
                if (multSymbol.Contains(LEFT_PARENTHESIS) && multSymbol.Contains(RIGHT_PARENTHESIS))
                {
                    addParenthesis = true;
                }
            }
            unitSymbol.Trim();
            unitSymbol.Trim(new char[] { LEFT_PARENTHESIS, RIGHT_PARENTHESIS });
            multSymbol.Trim(new char[] { LEFT_PARENTHESIS, RIGHT_PARENTHESIS });
            multSymbol.Trim();

            if (addParenthesis)
            {
                symbol += LEFT_PARENTHESIS + multSymbol + unitSymbol + RIGHT_PARENTHESIS;
            }
            else
            {
                symbol += multSymbol + unitSymbol;
            }
            return symbol;
        }
        
        private readonly char BACKSTROKE = '\\';
        private readonly char FRONTSTROKE = '/';
        private readonly char LEFT_PARENTHESIS = '(';
        private readonly char RIGHT_PARENTHESIS = ')';

       
    }
}