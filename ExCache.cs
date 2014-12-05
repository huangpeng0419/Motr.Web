using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace Motr.Web
{
    /// <summary>
    /// 扩展Cache对象操作
    /// </summary>
    public static class ExCache
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="dt">缓存的时间</param>
        public static void SetCache(this Cache cache, String key, String value, DateTime dt)
        {
            cache.SetCache(key, value, null, dt);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="cd">数据依赖项</param>
        /// <param name="dt">缓存的时间</param>
        public static void SetCache(this Cache cache, String key, String value, CacheDependency cd, DateTime dt)
        {
            cache.Insert(key, value, cd, dt, Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 获得剩余缓存大小(M)
        /// </summary>
        public static String Remains(this Cache cache)
        {
            return String.Format("{0}M", cache.EffectivePrivateBytesLimit / 1024 / 1024);
        }
    }
}
