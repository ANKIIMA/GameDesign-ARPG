namespace SharpUI.Source.Common.UI.Elements.SkillTrees
{
    public interface ISkillProgressLine
    {
        void OnSkillAmountChanged(int amount);
        int GetUnlockAtSkillAmount();
        int GetTotalAmount();
    }
}