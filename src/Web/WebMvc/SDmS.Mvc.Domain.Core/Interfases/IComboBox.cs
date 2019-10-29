using System.Collections.Generic;

namespace SDmS.Domain.Core.Interfases
{
    public interface IComboBox<T> where T : class
    {
        string DataValueField { get; }
        string DataTextField { get; }
        bool IsArgsRequired { get; }
        object Args { get; set; }
        IEnumerable<T> GetEntities();
    }
}
