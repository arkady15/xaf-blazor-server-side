using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DevExpress.ExpressApp.Blazor {
    public class PatchSvgSettings {
        public string MainColor { get; set; }
        public int ImageSize { get; set; }
    }

    public class ImageToStringHelper {
        static object lockObject = new object();
        static string ImageToBase64(Image image, ImageFormat format) {
            lock (lockObject) {
                using (MemoryStream ms = new MemoryStream()) {
                    image.Save(ms, format);
                    byte[] imageBytes = ms.ToArray();

                    return Convert.ToBase64String(imageBytes);
                }
            }
        }
        public static string GetImageBase64(string imageName, PatchSvgSettings patchSvgSettings = null) {
            if (imageName != null) {
                string[] path = imageName.Split('\\', '/');
                if (path.Length > 1 && path[0].ToLowerInvariant() == "devextreme") {
                    return path[1].ToLowerInvariant();
                }
                ImageInfo imageInfo = ImageLoader.Instance.GetLargeImageInfo(imageName);
                if (imageInfo.IsEmpty) {
                    imageInfo = ImageLoader.Instance.GetImageInfo(imageName);
                }
                if (imageInfo.IsSvgImage && imageInfo.ImageBytes != null) {
                    byte[] svg = imageInfo.ImageBytes;
                    if (patchSvgSettings != null && imageInfo.IsDevExpressSvgImage) {
                        svg = PatchSvg(svg, patchSvgSettings);
                    }
                    return "data:image/svg+xml;base64," + Convert.ToBase64String(svg);
                }
                if (!imageInfo.IsSvgImage && imageInfo.Image != null) {
                    return GetImageBase64FromPng(imageInfo.Image);
                }
                return imageName.ToLowerInvariant();
            }
            return null;
        }

        private static byte[] PatchSvg(byte[] svgBytes, PatchSvgSettings patchSvgSettings) {
            using (MemoryStream memoryStream = new MemoryStream(svgBytes)) {
                XElement svg = XElement.Load(memoryStream);
                if (patchSvgSettings.ImageSize > 0) {
                    int newImageSize = patchSvgSettings.ImageSize * 2;

                    XAttribute viewBoxAttribute = svg.Attribute("viewBox");
                    if (viewBoxAttribute != null && viewBoxAttribute.Value == "0 0 32 32") {
                        string newViewBoxValue = string.Format("0 0 {0} {0}", newImageSize);
                        viewBoxAttribute.SetValue(newViewBoxValue);
                    }
                    XAttribute styleAttribute = svg.Attribute("style");
                    string oldBackgroudValue = "enable-background:new 0 0 32 32;";
                    if (styleAttribute != null && styleAttribute.Value.Contains(oldBackgroudValue)) {
                        string newBackgroudValue = string.Format("enable-background:new 0 0 {0} {0};", newImageSize);
                        styleAttribute.SetValue(styleAttribute.Value.Replace(oldBackgroudValue, newBackgroudValue));
                    }
                }
                if (!String.IsNullOrEmpty(patchSvgSettings.MainColor)) {
                    XNamespace svgNamespace = "http://www.w3.org/2000/svg";
                    XElement styleElement = svg.Element(svgNamespace + "style");
                    //styleElement.FirstNode.ReplaceWith(".Black{fill:" + patchSvgSettings.MainColor + ";}");
                    string styleValue = ((XText)styleElement.FirstNode).Value;
                    string patchedStyleValue = Regex.Replace(styleValue, @"#[0-9A-F]+", patchSvgSettings.MainColor);
                    patchedStyleValue = Regex.Replace(patchedStyleValue, @"opacity:[0-9](\.[0-9]+)?;", "opacity:1;");
                    styleElement.FirstNode.ReplaceWith(patchedStyleValue);
                }
                string resultString = svg.ToString();
                return Encoding.ASCII.GetBytes(resultString);
            }
        }

        public static string GetImageBase64FromPng(Image image) {
            return "data:image/png;base64," + ImageToBase64(image, ImageFormat.Png);
        }
    }
}
