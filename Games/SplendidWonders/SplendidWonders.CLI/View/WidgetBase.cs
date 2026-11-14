using SplendidWonders.Common.Model;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View
{
    public abstract class WidgetBase
    {
        private static string _horizSingleBorderSymbol = "-";
        private static string _vertSingleBorderSymbol = "|";
        private static string _horizDoubleBorderSymbol = "=";
        private static string _vertDoubleBorderSymbol = "\u2551";
        private static string _cornerSymbol = "+";

        protected ConxelCanvas Cornered(ConxelCanvas targetCanvas, RgbColor borderColor)
        {
            targetCanvas[0, 0] = new Conxel(_cornerSymbol, borderColor);
            targetCanvas[0, targetCanvas.Columns - 1] = new Conxel(_cornerSymbol, borderColor);
            targetCanvas[targetCanvas.Rows - 1, 0] = new Conxel(_cornerSymbol, borderColor);
            targetCanvas[targetCanvas.Rows - 1, targetCanvas.Columns - 1] = new Conxel(_cornerSymbol, borderColor);

            return targetCanvas;
        }

        protected ConxelCanvas ColoredBlock(RgbColor fillColor, int width)
        {
            var line = new ConxelCanvas(1, width);
            line.WriteText(0, 0, $"{new string(' ', width)}", null, fillColor);

            return line;
        }

        protected ConxelCanvas EmptyBlock(int width)
        {
            var line = new ConxelCanvas(1, width);
            line.WriteText(0, 0, $"{new string(' ', width)}", null, null);

            return line;
        }

        protected ConxelCanvas SingleBorder(ConxelCanvas innerCanvas, RgbColor borderColor)
        {
            return BorderBlock(innerCanvas, borderColor, _vertSingleBorderSymbol, _horizSingleBorderSymbol);
        }

        protected ConxelCanvas DoubleBorder(ConxelCanvas innerCanvas, RgbColor borderColor)
        {
            return BorderBlock(innerCanvas, borderColor, _vertDoubleBorderSymbol, _horizDoubleBorderSymbol);
        }

        protected ConxelCanvas BorderBlock(ConxelCanvas innerCanvas, RgbColor borderColor, string vertBorder, string horizBorder)
        {
            var borderedCanvas = new ConxelCanvas(innerCanvas.Rows + 2, innerCanvas.Columns + 2);
            borderedCanvas.Blit(innerCanvas, 1, 1);

            borderedCanvas.WriteText(0, 1, string.Concat(Enumerable.Repeat(horizBorder, innerCanvas.Columns)), borderColor);

            for (int i = 1; i < borderedCanvas.Rows - 1; i++)
            {
                borderedCanvas[i, 0] = new Conxel(vertBorder, borderColor);
                borderedCanvas[i, borderedCanvas.Columns - 1] = new Conxel(vertBorder, borderColor);
            }

            borderedCanvas.WriteText(borderedCanvas.Rows - 1, 1, string.Concat(Enumerable.Repeat(horizBorder, innerCanvas.Columns)), borderColor);

            return borderedCanvas;
        }

        protected RgbColor GetDarkBonusColor(DevelopmentBonus bonus)
        {
            return bonus switch
            {
                DevelopmentBonus.Yellow => RgbColor.DarkSetYellow,
                DevelopmentBonus.Blue => RgbColor.DarkSetBlue,
                DevelopmentBonus.Green => RgbColor.DarkSetGreen,
                DevelopmentBonus.Brown => RgbColor.DarkSetBrown,
                DevelopmentBonus.Red => RgbColor.DarkSetRed,
                _ => RgbColor.FullWhite
            };
        }

        protected RgbColor GetLightBonusColor(DevelopmentBonus bonus)
        {
            return bonus switch
            {
                DevelopmentBonus.Yellow => RgbColor.DarkSetYellow,
                DevelopmentBonus.Blue => RgbColor.DarkSetBlue,
                DevelopmentBonus.Green => RgbColor.DarkSetGreen,
                DevelopmentBonus.Brown => RgbColor.DarkSetBrown,
                DevelopmentBonus.Red => RgbColor.DarkSetRed,
                _ => RgbColor.FullWhite
            };
        }
    }
}