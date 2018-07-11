using Tar.Core.DataAnnotations;
using Tar.ViewModel;

namespace Tar.Tests.Core.DataAnnotations
{
    public class XChild5
    {
        public XChild6 XChild6 { get; set; }
        [Required]
        public GeneralItemView GeneralItemView { get; set; }
    }
}