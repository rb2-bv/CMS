#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Caching;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class LabelProvider : ProviderBase<Label>, ILabelProvider
    {
        #region .ctor
        private ILabelProvider inner;
        public LabelProvider(ILabelProvider inner)
            : base(inner)
        {
            this.inner = inner;
        }
        #endregion

        public override Label Get(Label dummy)
        {
            if (dummy == null)
            {
                return dummy;
            }

            IDictionary<string, Label> cachedLabelsOfCategory = GetObjectCache(GetSite(dummy))
                .GetCache(string.Format("Label:category:{0}", dummy.Category),
                    () =>
                    {
                        IDictionary<string, Label> labelsOfCategoryInDictionary = new Dictionary<string, Label>();

                        List<Label> labelsOfCategory = inner.GetLabels(GetSite(dummy), dummy.Category).ToList();
                        foreach (Label label in labelsOfCategory)
                        {
                            if (!labelsOfCategoryInDictionary.ContainsKey(label.Category ?? "" + "_" + label.Name))
                            {
                                labelsOfCategoryInDictionary.Add(label.Category ?? "" + "_" + label.Name, label);
                            }
                        }

                        return labelsOfCategoryInDictionary;
                    });

            return cachedLabelsOfCategory[dummy.Category ?? "" + "_" + dummy.Name];
        }

        public IEnumerable<string> GetCategories(Site site)
        {
            return inner.GetCategories(site);
        }

        public IQueryable<Label> GetLabels(Site site, string category)
        {
            return inner.GetLabels(site, category);
        }

        public void AddCategory(Site site, string category)
        {
            inner.AddCategory(site, category);
        }

        public void RemoveCategory(Site site, string category)
        {
            try
            {
                inner.RemoveCategory(site, category);
            }
            finally
            {
                ClearObjectCache(site);
            }
        }

        public void Export(Site site, IEnumerable<Label> labels, IEnumerable<string> categories, System.IO.Stream outputStream)
        {
            inner.Export(site, labels, categories, outputStream);
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            try
            {
                inner.Import(site, zipStream, @override);
            }
            finally
            {
                ClearObjectCache(site);
            }
        }

        public void InitializeLabels(Site site)
        {
            //try
            //{
            //    inner.InitializeLabels(site);
            //}
            //finally
            //{
            //    ClearObjectCache(site);
            //}
        }

        public void ExportLabelsToDisk(Site site)
        {
            //inner.ExportLabelsToDisk(site);
        }

        public IEnumerable<Label> All(Site site)
        {
            return inner.All(site);
        }

        public IEnumerable<Label> All()
        {
            return inner.All();
        }

        protected override string GetItemCacheKey(Label o)
        {
            return string.Format("Label:category:{0}:name:{1}", o.Category, o.Name);
        }


        public void Flush(Site site)
        {
            try
            {
                this.inner.Flush(site);
            }
            finally
            {
                ClearObjectCache(site);
            }
        }

        #region ISiteElementProvider InitializeToDB/ExportToDisk
        public void InitializeToDB(Site site)
        {
            try
            {
                inner.InitializeToDB(site);
            }
            finally
            {
                ClearObjectCache(site);
            }
        }

        public void ExportToDisk(Site site)
        {
            inner.ExportToDisk(site);
        }
        #endregion
    }
}
