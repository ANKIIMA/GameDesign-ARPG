using System;
using UniRx;

namespace SharpUI.Source.Client.UI.Game.VendorScreen
{
    public class VendorScreenModel : IVendorScreenModel
    {
        private const string MyScene = "Vendor";
        
        public IObservable<string> GetMySceneName() => Observable.Return(MyScene);
    }
}