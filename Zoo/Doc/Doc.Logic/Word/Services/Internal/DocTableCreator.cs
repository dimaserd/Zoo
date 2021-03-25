using Doc.Logic.Word.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;

namespace Doc.Logic.Word.Implementations
{
    internal static class DocTableCreator
    {
        const int WidthForTable = 9026;

        public static DocTableSettings GetDefaultSettings()
        {
            return new DocTableSettings
            {
                BorderColor = "bdd6ee",
                BorderSize = 4,
                HeaderFontSize = 12,
                BoldHeader = false,
                TableRowFontSize = 12,
                BoldTableRow = false
            };
        }

        public static Table GetTable(DocumentTable model)
        {
            // Use the file name and path passed in as an argument 
            // to open an existing Word 2007 document.

            // Create an empty table.
            Table table = new Table();

            var borderType = new EnumValue<BorderValues>(BorderValues.Single);

            var settings = GetDefaultSettings();

            var color = settings.BorderColor;

            // Create a TableProperties object and specify its border information.
            TableProperties tblProp = new TableProperties(

                new TableBorders(
                    new TopBorder()
                    {
                        Val = borderType,
                        Size = settings.BorderSize,
                        Color = new StringValue
                        {
                            Value = color
                        }
                    },
                    new BottomBorder
                    {
                        Val = borderType,
                        Size = settings.BorderSize,
                        Color = new StringValue
                        {
                            Value = color
                        }
                    },
                    new LeftBorder
                    {
                        Val = borderType,
                        Size = settings.BorderSize,
                        Color = new StringValue
                        {
                            Value = color
                        }
                    },
                    new RightBorder
                    {
                        Val = borderType,
                        Size = settings.BorderSize,
                        Color = new StringValue
                        {
                            Value = color
                        }
                    },
                    new InsideHorizontalBorder
                    {
                        Val = borderType,
                        Size = settings.BorderSize,
                        Color = new StringValue
                        {
                            Value = color
                        }
                    },
                    new InsideVerticalBorder
                    {
                        Val = borderType,
                        Size = settings.BorderSize,
                        Color = new StringValue
                        {
                            Value = color
                        }
                    }
                ),
                new TableWidth
                {
                    Type = new EnumValue<TableWidthUnitValues>(TableWidthUnitValues.Auto),

                }
            );

            // Append the TableProperties object to the empty table.
            table.AppendChild(tblProp);

            table.Append(AppendHeader(model, settings));

            foreach (var tr in model.Data)
            {
                table.Append(CreateTableRowWithSomeStyles(tr, settings.TableRowFontSize, settings.BoldTableRow));
            }

            return table;
        }

        public static TableRow AppendHeader(DocumentTable model, DocTableSettings settings)
        {
            return CreateTableRowWithSomeStyles(model.Header, settings.HeaderFontSize, settings.BoldHeader, JustificationValues.Center);
        }

        public static TableRow CreateTableRowWithSomeStyles(List<string> values, int fontSize, bool isBold, JustificationValues justification = JustificationValues.Left)
        {
            TableRow tr = new TableRow();

            var count = values.Count;

            foreach (var val in values)
            {
                // Create a cell.
                TableCell tc = new TableCell();

                var props = new TableCellProperties(new TableCellWidth()
                {
                    Width = new StringValue
                    {
                        Value = $"{WidthForTable / count}"
                    },
                    Type = TableWidthUnitValues.Dxa
                },
                new TableCellMargin
                (
                    new TableCellLeftMargin
                    {
                        Width = 108,
                        Type = new EnumValue<TableWidthValues>(TableWidthValues.Dxa)
                    },
                    new TableCellRightMargin
                    {
                        Width = 108,
                        Type = new EnumValue<TableWidthValues>(TableWidthValues.Dxa)
                    }
                ));

                tc.Append(props);

                var run = GetRun(fontSize, isBold);

                run.Append(new Text(val));

                var para = new Paragraph(run);

                para.Append(new ParagraphProperties
                {
                    Justification = new Justification
                    {
                        Val = new EnumValue<JustificationValues>(justification)
                    }
                });

                // Specify the table cell content.
                tc.Append(para);

                // Append the table cell to the table row.
                tr.Append(tc);
            }

            return tr;
        }

        private static Run GetRun(int fontSize, bool isBold)
        {
            var runProps = new RunProperties
            {
                FontSize = new FontSize()
                {
                    Val = (fontSize * 2).ToString()
                }
            };

            if (isBold)
            {
                runProps.Bold = new Bold();
            }

            var run = new Run(runProps);

            return run;
        }
    }
}