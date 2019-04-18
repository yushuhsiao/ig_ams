using System;
using System.Collections.Generic;
using System.Text;

namespace InnateGlory
{
    internal static class SqlPatterns
    {        
        public static string Config_SetValue = new SqlBuilder
        {
            { "", nameof(Entity.Config.CorpId)        },
            { "", nameof(Entity.Config.PlatformId)    },
            { "", nameof(Entity.Config.Key1)          },
            { "", nameof(Entity.Config.Key2)          },
            { "", nameof(Entity.Config.Value)         }
        }.exec("Config_SetValue");

        public static string Lang_Set = new SqlBuilder
        {
            { " w", nameof(Entity.Lang.PlatformId)  },
            { " w", nameof(Entity.Lang.Path)        },
            { " w", nameof(Entity.Lang.Type)        },
            { " w", nameof(Entity.Lang.Key)         },
            { " w", nameof(Entity.Lang.LCID)        },
            { "Nu", nameof(Entity.Lang.Text)        }
        }.exec("Lang_Set");
        /*.build(s =>
        {
            string sql_w = s.where();
            return $@"
if not exists (select [Text] from {SqlBuilder.TableName} nolock {sql_w})
{s.insert_into()}
else
update {SqlBuilder.TableName} set {s.update()} {sql_w}";
        });*/
    }
}