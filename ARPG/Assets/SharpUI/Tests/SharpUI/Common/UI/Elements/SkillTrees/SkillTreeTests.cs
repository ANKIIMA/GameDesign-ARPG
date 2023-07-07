using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.SkillTrees;
using SharpUI.Source.Common.UI.Elements.Toggle;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.SkillTrees
{
    public class SkillTreeTests
    {
        private const int SkillLevel1 = 5;
        private const int SkillLevel2 = 3;
        private const int SkillLevel3 = 7;

        private Subject<Unit> SkillLevelObserver1 = new Subject<Unit>();
        private Subject<Unit> SkillLevelObserver2 = new Subject<Unit>();
        private Subject<Unit> SkillLevelObserver3 = new Subject<Unit>();
        
        private GameObject _gameObject;
        private SkillTree _skillTree;
        private ToggleButton _rootNode;
        private ISkillAmountLimit _skillAmountLimit;
        private List<ISkillLevel> _skillLevels;
        private List<ISkillProgressLine> _skillProgressLines;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();

            _skillLevels = CreateSkillLevels();
            _skillProgressLines = CreateSkillProgressLines();
            _skillAmountLimit = Substitute.For<ISkillAmountLimit>();
            
            _rootNode = new GameObject().AddComponent<ToggleButton>();
            _rootNode.checkedImage = new GameObject().AddComponent<Image>();
            _rootNode.isOn = false;
            
            _skillTree = _gameObject.AddComponent<SkillTree>();
            _skillTree.rootNode = _rootNode;

            _skillTree.Awake();
            _skillTree.SetSkillAmountLimit(_skillAmountLimit);
            _skillTree.SetSkillLevels(_skillLevels);
            _skillTree.SetSkillProgressLines(_skillProgressLines);
            _skillTree.OnGUI();
        }

        private List<ISkillLevel> CreateSkillLevels()
        {
            var list = new List<ISkillLevel>();
            
            var skillLevel = Substitute.For<ISkillLevel>();
            skillLevel.GetCurrentLevel().Returns(SkillLevel1);
            skillLevel.ObserveCurrentLevelChanged().Returns(SkillLevelObserver1);
            list.Add(skillLevel);
            
            skillLevel = Substitute.For<ISkillLevel>();
            skillLevel.GetCurrentLevel().Returns(SkillLevel2);
            skillLevel.ObserveCurrentLevelChanged().Returns(SkillLevelObserver2);
            list.Add(skillLevel);
            
            skillLevel = Substitute.For<ISkillLevel>();
            skillLevel.GetCurrentLevel().Returns(SkillLevel3);
            skillLevel.ObserveCurrentLevelChanged().Returns(SkillLevelObserver3);
            list.Add(skillLevel);

            return list;
        }

        private List<ISkillProgressLine> CreateSkillProgressLines()
        {
            var list = new List<ISkillProgressLine>();

            var skillProgressLine = Substitute.For<ISkillProgressLine>();
            list.Add(skillProgressLine);
            
            skillProgressLine = Substitute.For<ISkillProgressLine>();
            list.Add(skillProgressLine);
            
            skillProgressLine = Substitute.For<ISkillProgressLine>();
            list.Add(skillProgressLine);
            
            skillProgressLine = Substitute.For<ISkillProgressLine>();
            list.Add(skillProgressLine);

            return list;
        }

        [Test]
        public void OnGUI_WillNotifySkillAmountLimit()
        {
            _skillAmountLimit.Received().UpdateSpent(Arg.Any<int>());
        }

        [Test]
        public void OnGUI_WillNotifyAllProgressLines()
        {
            _skillProgressLines.ForEach(progressLine => progressLine.Received().OnSkillAmountChanged(Arg.Any<int>()));
        }

        [Test]
        public void WhenSkillLevelsChange_WillUpdateSpent()
        {
            _skillAmountLimit.ClearReceivedCalls();
            
            SkillLevelObserver1.OnNext(Unit.Default);
            SkillLevelObserver2.OnNext(Unit.Default);
            SkillLevelObserver3.OnNext(Unit.Default);

            _skillAmountLimit.Received(3).UpdateSpent(Arg.Any<int>());
        }
    }
}