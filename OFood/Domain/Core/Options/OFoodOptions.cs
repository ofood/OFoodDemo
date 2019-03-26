using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace OFood.Domain.Core.Options
{
    /// <summary>
    /// OSharp框架配置选项信息
    /// </summary>
    public class OFoodOptions
    {
        /// <summary>
        /// 初始化一个<see cref="OFoodOptions"/>类型的新实例
        /// </summary>
        public OFoodOptions()
        {
            DbContexts = new ConcurrentDictionary<string, OFoodDbContextOptions>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 获取 数据上下文配置信息
        /// </summary>
        public IDictionary<string, OFoodDbContextOptions> DbContexts { get; }

        /// <summary>
        /// 获取或设置 邮件发送选项
        /// </summary>
        public MailSenderOptions MailSender { get; set; }

        /// <summary>
        /// 获取或设置 JWT身份认证选项
        /// </summary>
        public JwtOptions Jwt { get; set; }

        /// <summary>
        /// 获取或设置 Redis选项
        /// </summary>
        public RedisOptions Redis { get; set; }

        /// <summary>
        /// 获取或设置 Swagger选项
        /// </summary>
        public SwaggerOptions Swagger { get; set; }

        /// <summary>
        /// 获取指定上下文类和指定数据库类型的上下文配置信息
        /// </summary>
        public OFoodDbContextOptions GetDbContextOptions(Type dbContextType)
        {
            return DbContexts.Values.SingleOrDefault(m => m.DbContextType == dbContextType);
        }
    }
}