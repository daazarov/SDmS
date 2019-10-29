   using System.Web.Mvc;

namespace SDmS.Domain.Core.Interfases.Services
{
    public interface ICatalogService
    {
        SelectList GetComboBox<T>(IComboBox<T> comboBox, params object[] args) where T : class;
    }
}
