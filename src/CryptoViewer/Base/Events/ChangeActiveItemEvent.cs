using Caliburn.Micro;

namespace CryptoViewer.Base.Events
{
    internal class ChangeActiveItemEvent
    {
        public IScreen NewActiveItem { get; }

        public ChangeActiveItemEvent(IScreen newActiveItem)
        {
            NewActiveItem = newActiveItem;
        }
    }
}
