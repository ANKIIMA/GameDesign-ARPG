using System;
using TMPro;
using UniRx;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.ActionBars
{
    public interface IActionBarCooldown
    {
        bool IsConsumingTime();
        void CoolDown(float seconds);
        void ConsumeSeconds(float seconds);
        void TakeCooldownImage(Image image);
        IObservable<Unit> ObserveCooldownFinished();
        void TakeCooldownText(TMP_Text text);
        void SetFillMethod(Image.FillMethod method);
        void Expire();
        void Update();
    }
}