using SharpUI.Source.Common.UI.Elements.List.Holder;

namespace SharpUI.Source.Common.UI.Elements.List.Selection
{
    public static class SelectionStrategyFactory
    {
        public static ISelectionStrategy<THolder> CreateSelectionStrategy<THolder, TData>(
            ItemSelectionType type, int amount = 0)
            where THolder : ItemHolder<TData>
            where TData : class
        {
            switch (type)
            {
                case ItemSelectionType.None: return CreateNoSelectionStrategy<THolder, TData>();
                case ItemSelectionType.Single: return CreateSingleSelectionStrategy<THolder, TData>();
                case ItemSelectionType.Limited: return CreateLimitedSelectionStrategy<THolder, TData>(amount);
                case ItemSelectionType.All: return CreateAllSelectionStrategy<THolder, TData>();
                default:
                    throw new UnknownSelectionStrategyException($"Unknown selection strategy with type {type}!");
            }
        }

        private static SelectionStrategyNone<THolder, TData> CreateNoSelectionStrategy<THolder, TData>()
            where THolder : ItemHolder<TData>
            where TData : class
            => new SelectionStrategyNone<THolder, TData>();
        
        private static SelectionStrategySingle<THolder, TData> CreateSingleSelectionStrategy<THolder, TData>()
            where THolder : ItemHolder<TData>
            where TData : class
            => new SelectionStrategySingle<THolder, TData>();
        
        private static SelectionStrategyLimited<THolder, TData> CreateLimitedSelectionStrategy<THolder, TData>(int amount)
            where THolder : ItemHolder<TData>
            where TData : class
            => new SelectionStrategyLimited<THolder, TData>(amount);
        
        private static SelectionStrategyAll<THolder, TData> CreateAllSelectionStrategy<THolder, TData>()
            where THolder : ItemHolder<TData>
            where TData : class
            => new SelectionStrategyAll<THolder, TData>();
    }
}