namespace SharpUI.Source.Common.UI.Elements.ArrowLists.Adapter
{
    public class EmptyArrowListAdapter : ArrowListAdapter<string>
    {
        public override string CurrentItem() => "";
        public override string PreviousItem() => "";
        public override string NextItem() => "";
    }
}