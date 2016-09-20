using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class UrlRedirectProvider : ProviderBase<UrlRedirect>, IUrlRedirectProvider
    {
        #region .ctor

        private IUrlRedirectProvider inner;

        public UrlRedirectProvider(IUrlRedirectProvider inner)
            : base(inner)
        {
            this.inner = inner;
        }

        #endregion

        public void Export(Site site, System.IO.Stream outputStream)
        {
            inner.Export(site, outputStream);
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            inner.Import(site, zipStream, @override);
            site.ClearCache();
        }

        public IEnumerable<UrlRedirect> All(Site site)
        {
            return inner.All(site);
        }

        public void InitializeToDB(Site site)
        {
            inner.InitializeToDB(site);
        }

        public void ExportToDisk(Site site)
        {
            inner.ExportToDisk(site);
        }

        protected override string GetItemCacheKey(UrlRedirect o)
        {
            var cacheKey = string.Format("UrlRedirect:{0}", o.InputUrl.ToLower());
            return cacheKey;
        }

        protected override string GetListCacheKey()
        {
            return "All-UrlRedirects:";
        }
    }
}