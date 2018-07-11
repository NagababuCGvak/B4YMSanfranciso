using Tar.Core.DataAnnotations;
using Tar.ViewModel;

namespace Tar.Tests.Core.DataAnnotations
{
    public class XChild1
    {
        public XChild2 XChild2 { get; set; }
        [Required]
        public GeneralItemView GeneralItemView { get; set; }
    }
}