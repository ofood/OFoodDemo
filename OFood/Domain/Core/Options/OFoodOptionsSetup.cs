using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OFood.Domain.Data;
using OFood.Domain.Entity;
using OFood.Exceptions;
using OFood.Extensions;


namespace OFood.Domain.Core.Options
{
    /// <summary>
    /// OSharp配置选项创建器
    /// </summary>
    public class OFoodOptionsSetup : IConfigureOptions<OFoodOptions>
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 初始化一个<see cref="OFoodOptionsSetup"/>类型的新实例
        /// </summary>
        public OFoodOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>Invoked to configure a TOptions instance.</summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(OFoodOptions options)
        {
            SetDbContextOptionses(options);

            //MailSender
            IConfigurationSection section = _configuration.GetSection("OFood:MailSender");
            MailSenderOptions sender = section.Get<MailSenderOptions>();
            if (sender != null)
            {
                if (sender.Password == null)
                {
                    sender.Password = _configuration["OFood:MailSender:Password"];
                }
                options.MailSender = sender;
            }

            //JwtOptions
            section = _configuration.GetSection("OFood:Jwt");
            JwtOptions jwt = section.Get<JwtOptions>();
            if (jwt != null)
            {
                if (jwt.Secret == null)
                {
                    jwt.Secret = _configuration["OFood:Jwt:Secret"];
                }
                options.Jwt = jwt;
            }

            // RedisOptions
            section = _configuration.GetSection("OFood:Redis");
            RedisOptions redis = section.Get<RedisOptions>();
            if (redis != null)
            {
                if (redis.Configuration.IsMissing())
                {
                    throw new OFoodException("配置文件中Redis节点的Configuration不能为空");
                }
                options.Redis = redis;
            }

            // SwaggerOptions
            section = _configuration.GetSection("OFood:Swagger");
            SwaggerOptions swagger = section.Get<SwaggerOptions>();
            if (swagger != null)
            {
                if (swagger.Url.IsMissing())
                {
                    throw new OFoodException("配置文件中Swagger节点的Url不能为空");
                }
                options.Swagger = swagger;
            }
        }

        /// <summary>
        /// 初始化上下文配置信息，首先以OSharp配置节点中的为准，
        /// 不存在OSharp节点，才使用ConnectionStrings的数据连接串来构造SqlServer的配置，
        /// 保证同一上下文类型只有一个配置节点
        /// </summary>
        /// <param name="options"></param>
        private void SetDbContextOptionses(OFoodOptions options)
        {
            IConfigurationSection section = _configuration.GetSection("OFood:DbContexts");
            IDictionary<string, OFoodDbContextOptions> dict = section.Get<Dictionary<string, OFoodDbContextOptions>>();
            if (dict == null || dict.Count == 0)
            {
                string connectionString = _configuration["ConnectionStrings:DefaultDbContext"];
                if (connectionString == null)
                {
                    return;
                }
                OFoodDbContextOptions dbContextOptions = new OFoodDbContextOptions()
                {
                    DbContextTypeName = "OFood.Entity.DefaultDbContext,OSharp.EntityFrameworkCore",
                    ConnectionString = connectionString,
                    DatabaseType = DatabaseType.SqlServer
                };
                options.DbContexts.Add("DefaultDbContext", dbContextOptions);
                return;
            }
            var repeated = dict.Values.GroupBy(m => m.DbContextType).FirstOrDefault(m => m.Count() > 1);
            if (repeated != null)
            {
                throw new OFoodException($"数据上下文配置中存在多个配置节点指向同一个上下文类型：{repeated.First().DbContextTypeName}");
            }

            foreach (KeyValuePair<string, OFoodDbContextOptions> pair in dict)
            {
                options.DbContexts.Add(pair.Key, pair.Value);
            }
        }
    }
}