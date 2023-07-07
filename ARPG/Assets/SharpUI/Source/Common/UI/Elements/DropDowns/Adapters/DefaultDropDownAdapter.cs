namespace SharpUI.Source.Common.UI.Elements.DropDowns.Adapters
{
    public class DefaultDropDownAdapter : DropDownAdapter<string>
    {
        public override string GetItemTextAt(int index)
        {
            return data[index];
        }
    }
}