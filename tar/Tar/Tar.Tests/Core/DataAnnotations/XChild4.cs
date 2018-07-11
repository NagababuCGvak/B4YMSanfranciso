using Tar.Core.DataAnnotations;
using Tar.ViewModel;

namespace Tar.Tests.Core.DataAnnotations
{
    public class XChild4
    {
        public XChild5 XChild5 { get; set; }
        [Required]
        public GeneralItemView GeneralItemView { get; set; }
    }
}