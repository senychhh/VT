using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace Kolbasin_lab1.TagHelpers
{
    [HtmlTargetElement("img", Attributes = "img-action, img-controller")]
    public class ImageTagHelper : TagHelper
    {
        private readonly LinkGenerator _linkGenerator;

        public ImageTagHelper(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        [HtmlAttributeName("img-controller")]
        public string ImgController { get; set; } = "";

        [HtmlAttributeName("img-action")]
        public string ImgAction { get; set; } = "";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ImgAction) || string.IsNullOrEmpty(ImgController))
                return;

            var url = _linkGenerator.GetPathByAction(ImgAction, ImgController);

            output.Attributes.SetAttribute("src", url);
        }
    }
}
