using SplendidWonders.Common.Model;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View
{
    public abstract class WidgetBase
    {

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

        protected RgbColor GetDarkDevelopmentColor(DevelopmentColor bonus)
        {
            return bonus switch
            {
                DevelopmentColor.Yellow => WidgetHelper.DarkSetYellow,
                DevelopmentColor.Blue => WidgetHelper.DarkSetBlue,
                DevelopmentColor.Green => WidgetHelper.DarkSetGreen,
                DevelopmentColor.Brown => WidgetHelper.DarkSetBrown,
                DevelopmentColor.Red => WidgetHelper.DarkSetRed,
                _ => WidgetHelper.DarkSetDefault
            };
        }

        protected RgbColor GetLightDevelopmentColor(DevelopmentColor bonus)
        {
            return bonus switch
            {
                DevelopmentColor.Yellow => WidgetHelper.LightSetYellow,
                DevelopmentColor.Blue => WidgetHelper.LightSetBlue,
                DevelopmentColor.Green => WidgetHelper.LightSetGreen,
                DevelopmentColor.Brown => WidgetHelper.LightSetBrown,
                DevelopmentColor.Red => WidgetHelper.LightSetRed,
                _ => WidgetHelper.LightSetDefault
            };
        }
    }
}