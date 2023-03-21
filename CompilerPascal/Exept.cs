using System;

namespace CompilerPascal
{
    public class Except : Exception
    {
        private string _message;
        private int _positionLine;
        private int _positionSymbol;
        public Except(int positionLine, int positionSymbol, string message)
        {
            _message = message;
            _positionLine = positionLine;
            _positionSymbol = positionSymbol;
        }
        public override string ToString()
        {
            return $"({_positionLine},{_positionSymbol}) ERROR: {_message}";
        }
    }
}
