using Blazorise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbeckDev.Dlrgdd.RegistrationTool.Frontend.Themes
{
    public class DlrgTheme
    {
        public const string DlrgRed = "#e30613";
        public const string DlrgYellow = "#ffed00";
        public const string DlrgGray = "#31353d";

        public static Theme MainTheme = new Theme
        {
            BarOptions = new ThemeBarOptions
            {
                HorizontalHeight = "64px",
                //LightColors = new ThemeBarColorOptions()
                //{
                //    BackgroundColor = DlrgRed,
                //    Color = DlrgYellow,
                //    ItemColorOptions = new ThemeBarItemColorOptions
                //    {
                //        HoverColor = "#02D248",
                //        HoverBackgroundColor = "#6c757d",
                //        ActiveBackgroundColor = "#2993EF",
                //        ActiveColor = "#5E550A"

                //    }
                //},
                VerticalBrandHeight = "100",


            },
            BreakpointOptions = new ThemeBreakpointOptions
            {
            },
            ColorOptions = new ThemeColorOptions
            {
                Primary = DlrgRed,
                Secondary = DlrgYellow,
                //Success = "#23C02E",
                Info = "#9BD8FE",
                //Warning = "#F8B86C",
                //Danger = "#F95741",
                //Light = "#F0F0F0",
                //Dark = "#535353",
            },
            BackgroundOptions = new ThemeBackgroundOptions
            {
                Primary = "#e30613",
            },
            TextColorOptions = new ThemeTextColorOptions
            {
                Secondary = DlrgYellow,
            },
            //ButtonOptions = new ThemeButtonOptions { },
            //DropdownOptions = new ThemeDropdownOptions { },
            InputOptions = new ThemeInputOptions
            {
                CheckColor = "#0288D1",
            },
            //CardOptions = new ThemeCardOptions { },
            //ModalOptions = new ThemeModalOptions { },
            //TabsOptions = new ThemeTabsOptions { },
            //ProgressOptions = new ThemeProgressOptions { },
            //AlertOptions = new ThemeAlertOptions { },
            //BreadcrumbOptions = new ThemeBreadcrumbOptions { },
            //BadgeOptions = new ThemeBadgeOptions { },
            //PaginationOptions = new ThemePaginationOptions { },
            //TooltipOptions = new ThemeTooltipOptions
            //{
            //    BackgroundColor = "#22c8ce",
            //    BackgroundOpacity = .7f,
            //    Color = "#ff0000",
            //},
        };
    }
}

