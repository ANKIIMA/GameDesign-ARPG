using System;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Base.Model;

namespace SharpUI.Source.Client.UI.Game.VendorScreen
{
    public interface IVendorScreenComponent : IBaseComponent
    {
    }

    public interface IVendorScreenPresenter
    {
        void OnVendorWindowDestroyed();
    }

    public interface IVendorScreenModel : IBaseModel
    {
        IObservable<string> GetMySceneName();
    }
}