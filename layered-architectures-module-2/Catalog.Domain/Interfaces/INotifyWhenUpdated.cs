namespace Catalog.Domain.Interfaces;

public interface INotifyWhenUpdated
{
    public event EventHandler Updated;

    public void IAmUpdated();
}