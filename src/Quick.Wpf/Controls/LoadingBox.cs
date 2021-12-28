using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Concurrent;

namespace Quick
{
    public class LoadingBox : ContentControl, IDisposable
    {
        public LoadingBox()
        {
            s_loadingBoxs.Add(this);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            string token = GetRealToken(Token);
            if (_visibleTokens.TryGetValue(token, out int refCount) && refCount > 0)
            {
                Visibility = Visibility.Visible;
            }
        }

        public string Token { get; set; }

        private static List<LoadingBox> s_loadingBoxs = new List<LoadingBox>();
        private static ConcurrentDictionary<string, int> _visibleTokens = new ConcurrentDictionary<string, int>();

        private static string GetRealToken(string token)
        {
            if (token == null)
            {
                return "2071D491-75DD-4CCB-A52C-A3513D1B4000";
            }
            return token;
        }

        public static void Show(object content, string token)
        {
            LoadingBox loadingBox = s_loadingBoxs.FirstOrDefault(p => p.Token == token);
            token = GetRealToken(token);
            if (_visibleTokens.TryGetValue(token, out int refCount))
            {
                refCount++;
            }
            else
            {
                refCount = 1;
            }
            _visibleTokens[token] = refCount;

            if (loadingBox != null)
            {
                loadingBox.Content = content;
                loadingBox.Visibility = refCount > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        public static void Hide(string token)
        {
            LoadingBox loadingBox = s_loadingBoxs.FirstOrDefault(p => p.Token == token);
            token = GetRealToken(token);
            if (_visibleTokens.TryGetValue(token, out int refCount))
            {
                if (refCount > 0)
                    refCount--;
            }
            else
            {
                refCount = 0;
            }
            if (refCount == 0)
            {
                _visibleTokens.TryRemove(token, out _);
            }
            else
            {
                _visibleTokens[token] = refCount;
            }
            if (loadingBox != null)
            {
                loadingBox.Visibility = refCount > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public static void Show(object content)
        {
            Show(content, null);
        }
        public static void Hide()
        {
            Hide(null);
        }

        public void Dispose()
        {
            s_loadingBoxs.Remove(this);
        }
    }
}
