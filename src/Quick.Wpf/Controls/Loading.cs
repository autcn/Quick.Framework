
using System;

namespace Quick
{
    public class Loading : IDisposable
    {
        public Loading(object content) : this(content, null)
        {
            
        }
        private string _token;

        public Loading(object content, string token)
        {
            _token = token;
            if (content is string str)
            {
                if (str != null)
                {
                    LoadingBox.Show(QServiceProvider.GetService<ILocalization>().ConvertStrongText(str), token);
                    return;
                }
            }
            LoadingBox.Show(content, token);
        }

        public void Dispose()
        {
            LoadingBox.Hide(_token);
        }
    }
}
