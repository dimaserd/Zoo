using System;
using System.IO;
using Doc.Logic.Word.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace Doc.Logic.Word.Services.Internal
{
    internal static class DocImageInserter
    {
        static readonly long ImageNormalizedMaxWidth = (long)(990000L * 3.44);

        private static Paragraph FindParagraphToMakeReplace(WordprocessingDocument document, string textToFind)
        {
            var body = document.MainDocumentPart.Document.Body;

            foreach (var child in body.ChildElements)
            {
                if (child is Paragraph asP)
                {
                    try
                    {
                        var text = asP.ChildElements.First<Run>().ChildElements.First<Text>();

                        if (text.Text == textToFind)
                        {
                            //Удаляю Run который содержит этот текст для замены
                            text.Parent.Remove();
                            return asP;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            return null;
        }

        public static void InsertAPicture(WordprocessingDocument document, DocxImageReplace imgToReplace)
        {
            var para = FindParagraphToMakeReplace(document, imgToReplace.TextToReplace);

            if (para == null)
            {
                return;
            }
            var mainPart = document.MainDocumentPart;

            var imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

            using (var stream = new FileStream(imgToReplace.ImageFilePath, FileMode.Open))
            {
                imagePart.FeedData(stream);
            }

            AddImageToElement(para, GetConvertedtoEmus(imgToReplace.ImageFilePath), mainPart.GetIdOfPart(imagePart));
        }

        private static EmuRectangle GetConvertedtoEmus(string imgFileName)
        {
            int iWidth = 0;
            int iHeight = 0;
            using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(imgFileName))
            {

                iWidth = bmp.Width;
                iHeight = bmp.Height;
            }

            iWidth = (int)Math.Round((decimal)iWidth * 9525);
            iHeight = (int)Math.Round((decimal)iHeight * 9525);

            return new EmuRectangle
            {
                Height = iHeight,
                Width = iWidth
            };
        }

        private static EmuRectangle NormalizeToWidth(EmuRectangle rect)
        {
            var k = (double)ImageNormalizedMaxWidth / rect.Width; //сколько раз старая ширина или высота содержиться в новой

            return new EmuRectangle
            {
                Width = ImageNormalizedMaxWidth,
                Height = (long)(k * rect.Height)
            };
        }

        private static Drawing GetImageElement(EmuRectangle rect, string relationshipId)
        {
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent { Cx = rect.Width, Cy = rect.Height },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = 1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = 0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = 0U,
                         DistanceFromBottom = 0U,
                         DistanceFromLeft = 0U,
                         DistanceFromRight = 0U,
                         EditId = "50D07946"
                     });

            return element;
        }

        private static void AddImageToElement(OpenXmlElement element, EmuRectangle rect, string relationshipId)
        {
            rect = NormalizeToWidth(rect);

            // Define the reference of the image.
            var imgElement = GetImageElement(rect, relationshipId);

            element.AppendChild(new Run(imgElement));
        }

        public class EmuRectangle
        {
            public long Height { get; set; }

            public long Width { get; set; }
        }
    }
}
