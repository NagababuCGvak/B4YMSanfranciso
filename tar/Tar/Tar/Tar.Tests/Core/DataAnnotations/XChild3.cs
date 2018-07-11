using Tar.Core.DataAnnotations;
using Tar.ViewModel;

namespace Tar.Tests.Core.DataAnnotations
{
    public class XChild3
    {
        public XChild4 XChild4 { get; set; }
        [Required]
        public GeneralItemView GeneralItemView { get; set; }
    }
}