﻿using CommonPluginsShared.Controls;
using IsThereAnyDeal.Views;
using Playnite.SDK;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace IsThereAnyDeal.Services
{
    public class ItadViewSidebar : SidebarItem
    {
        private static IResourceProvider resources = new ResourceProvider();
        private SidebarItemControl sidebarItemControl;


        public ItadViewSidebar(IsThereAnyDeal plugin)
        {
            Type = SiderbarItemType.View;
            Title = resources.GetString("LOCItad");
            Icon = new TextBlock
            {
                Text = "\uea63",
                FontFamily = resources.GetResource("CommonFont") as FontFamily
            };
            Opened = () =>
            {
                if (sidebarItemControl == null)
                {
                    sidebarItemControl = new SidebarItemControl(API.Instance);
                    sidebarItemControl.SetTitle(resources.GetString("LOCItad"));
                    sidebarItemControl.AddContent(new IsThereAnyDealView(plugin, plugin.GetPluginUserDataPath(), plugin.PluginSettings.Settings));
                }
                return sidebarItemControl;
            };
            Visible = plugin.PluginSettings.Settings.EnableIntegrationButtonSide;
        }


        public void ResetView()
        {
            sidebarItemControl = null;
        }
    }
}
