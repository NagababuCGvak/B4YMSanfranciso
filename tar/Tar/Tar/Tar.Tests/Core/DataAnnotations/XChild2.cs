using Tar.Core.DataAnnotations;
using Tar.ViewModel;

namespace Tar.Tests.Core.DataAnnotations
{
    public class XChild2
    {
        public XChild3 XChild3 { get; set; }
        [Required]
        public GeneralItemView GeneralItemView { get; set; }
    }
}