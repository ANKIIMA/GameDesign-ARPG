using System.Collections.Generic;

namespace SharpUI.Source.Common.UI.Elements.Decorators
{
    public static class SharpDecoratorExtensions
    {
        public static void OnSelected(this IEnumerable<IDecorator> decorators)
        {
            foreach (var decorator in decorators)
                decorator.OnSelected();
        }
        
        public static void OnDeselected(this IEnumerable<IDecorator> decorators)
        {
            foreach (var decorator in decorators)
                decorator.OnDeselected();
        }
        
        public static void OnEnter(this IEnumerable<IDecorator> decorators)
        {
            foreach (var decorator in decorators)
                decorator.OnEnter();
        }
        
        public static void OnExit(this IEnumerable<IDecorator> decorators)
        {
            foreach (var decorator in decorators)
                decorator.OnExit();
        }
        
        public static void OnEnabled(this IEnumerable<IDecorator> decorators)
        {
            foreach (var decorator in decorators)
                decorator.OnEnabled();
        }
        
        public static void OnDisabled(this IEnumerable<IDecorator> decorators)
        {
            foreach (var decorator in decorators)
                decorator.OnDisabled();
        }

        public static void OnPressed(this IEnumerable<IDecorator> decorators)
        {
            foreach (var decorator in decorators)
                decorator.OnPressed();
        }

        public static void OnReleased(this IEnumerable<IDecorator> decorators)
        {
            foreach (var decorator in decorators)
                decorator.OnReleased();
        }
    }
}