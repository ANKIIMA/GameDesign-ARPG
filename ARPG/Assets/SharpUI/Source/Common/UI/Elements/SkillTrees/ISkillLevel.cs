using UniRx;

namespace SharpUI.Source.Common.UI.Elements.SkillTrees
{
    public interface ISkillLevel
    {
        void SetMaxLevel(int value);
        void SetCurrentLevel(int value);
        void IncrementLevel();
        void DecrementLevel();
        int GetCurrentLevel();
        int GetMaxLevel();
        bool HaveLevels();
        bool IsFullLevels();
        bool IsEmptyLevels();
        Subject<Unit> ObserveMaxLevelChanged();
        Subject<Unit> ObserveCurrentLevelChanged();
    }
}