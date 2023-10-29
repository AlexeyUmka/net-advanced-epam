namespace Catalog.Application.Common.Models;

public class Link
{
    public Link(string rel, string url, string httpMethod, object? body = null)
    {
        Rel = rel;
        Url = url;
        HttpMethod = httpMethod;
        Body = body;
    }

    public string Rel { get; }
    public string Url { get; }
    public string HttpMethod { get; }
    
    public object? Body { get; }
}