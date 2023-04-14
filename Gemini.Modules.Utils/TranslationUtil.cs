using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Gu.Localization;

namespace Gemini.Modules.Utils
{
    public class TranslationUtil
    {
        public static Dictionary<Type, ResourceManager>
            ResourceManagerCache = new Dictionary<Type, ResourceManager>();

        public static string GetTranslate(string key, Type resourceType)
        {
            var t = resourceType;
            if (t == null)
                return key;
            ResourceManager resourceManager = null;
            if (ResourceManagerCache.ContainsKey(t))
            {
                resourceManager = ResourceManagerCache[t];

            }
            else
            {
                //var obj = Activator.CreateInstance(null, t);

                var f = t.GetRuntimeProperties().FirstOrDefault(p => p.Name == "ResourceManager");
                resourceManager = (ResourceManager)f.GetValue(null);
                ResourceManagerCache.Add(t, resourceManager);
            }

            var translation = Translation.GetOrCreate(resourceManager, key);
            return translation.Translated;
        }
    }
}
