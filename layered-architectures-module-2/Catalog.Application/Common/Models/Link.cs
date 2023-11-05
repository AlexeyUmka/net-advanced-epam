namespace Catalog.Application.Common.Models;

public record Link(string Rel, string Url, string HttpMethod, object? Body = null);