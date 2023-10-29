using Catalog.Application.Common.Models;

namespace Catalog.Application.Common.Interfaces;

public interface IHaveHypermediaLinks
{
    IEnumerable<Link> HypermediaLinks { get; protected set; }
}