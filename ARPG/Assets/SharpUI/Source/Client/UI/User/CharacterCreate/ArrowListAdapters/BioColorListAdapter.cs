using SharpUI.Source.Common.UI.Elements.ArrowLists.Adapter;

namespace SharpUI.Source.Client.UI.User.CharacterCreate.ArrowListAdapters
{
    public class BioColorListAdapter : ArrowListAdapter<BioColor>
    {
        public override string CurrentItem() => HasCurrentData() ? data[currentIndex].ToString() : null;

        public override string PreviousItem() => HasPreviousData() ? data[currentIndex - 1].ToString() : null;

        public override string NextItem() => HasNextData() ? data[currentIndex + 1].ToString() : null;
    }
}