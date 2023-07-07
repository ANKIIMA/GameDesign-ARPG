using System;

namespace SharpUI.Source.Common.UI.Elements.List.Selection
{
    public class UnknownSelectionStrategyException : Exception
    {
        public UnknownSelectionStrategyException(string message = "") : base(message) { }
    }
}