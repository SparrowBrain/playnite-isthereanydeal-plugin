﻿using IsThereAnyDeal.Models;
using Playnite.SDK;
using CommonPluginsShared;
using System;
using System.Collections.Generic;
using System.IO;
using CommonPlayniteShared.Common;
using Playnite.SDK.Data;

namespace IsThereAnyDeal.Services
{
    public class GenericWishlist
    {
        internal static readonly ILogger logger = LogManager.GetLogger();
        internal static readonly IResourceProvider resources = new ResourceProvider();

        protected static IWebView _WebViewOffscreen;
        internal static IWebView WebViewOffscreen
        {
            get
            {
                if (_WebViewOffscreen == null)
                {
                    _WebViewOffscreen = API.Instance.WebViews.CreateOffscreenView();
                }
                return _WebViewOffscreen;
            }

            set
            {
                _WebViewOffscreen = value;
            }
        }

        internal static string PluginUserDataPath;


        public List<Wishlist> LoadWishlists(string clientName, string PluginUserDataPath, bool force = false)
        {
            GenericWishlist.PluginUserDataPath = PluginUserDataPath;

            try
            {
                string DirPath = Path.Combine(PluginUserDataPath, "IsThereAnyDeal");
                string FilePath = Path.Combine(DirPath, $"{clientName}.json");

                if (!Directory.Exists(DirPath))
                {
                    Directory.CreateDirectory(DirPath);
                    return null;
                }

                if (!File.Exists(FilePath))
                {
                    return null;
                }
                else if (File.GetLastWriteTime(FilePath).AddDays(1) < DateTime.Now)
                {
                    if (!force)
                    {
                        return null;
                    }
                }

                logger.Info($"Load wishlist from local for {clientName}");

                return Serialization.FromJsonFile<List<Wishlist>>(FilePath);
            }
            catch (Exception ex)
            {
                Common.LogError(ex, false, $"Error for load {clientName} wishlist", true, "IsThereAnyDeal");
                return null;
            }
        }

        public void SaveWishlist(string clientName, string PluginUserDataPath, List<Wishlist> Wishlist)
        {
            try
            {
                string DirPath = Path.Combine(PluginUserDataPath, "IsThereAnyDeal");
                string FilePath = Path.Combine(DirPath, $"{clientName}.json");

                FileSystem.WriteStringToFile(FilePath, Serialization.ToJson(Wishlist));
            }
            catch (Exception ex)
            {
                Common.LogError(ex, false, $"Error for save {clientName} wishlist", true, "IsThereAnyDeal");
            }
        }


        public List<Wishlist> SetCurrentPrice(List<Wishlist> ListWishlist, IsThereAnyDealSettings settings)
        {
            IsThereAnyDealApi isThereAnyDealApi = new IsThereAnyDealApi();
            return isThereAnyDealApi.GetCurrentPrice(ListWishlist, settings);
        }
    }
}
