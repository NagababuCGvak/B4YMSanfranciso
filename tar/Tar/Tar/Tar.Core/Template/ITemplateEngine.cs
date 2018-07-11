using System.Collections.Generic;

namespace Tar.Core.Template
{
    public interface ITemplateEngine
    {
        string Merge(string templateString, IDictionary<string, object> parameters);
    }
}