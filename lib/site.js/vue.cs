using Bridge;
using Retyped;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnateGlory
{
    public abstract class VueComponent : vue.ComponentOptions<vue.Vue>
    {
        public vue.Vue vm { get; set; }

        public VueComponent()
        {
            this.Init();
            this.vm = new vue.Vue(this);
        }

        public abstract void Init();

        [Template("{0}")]
        protected static methodsConfig.keyFn KeyFn(Action action) => action.As<methodsConfig.keyFn>();
        [Template("{0}")]
        protected static methodsConfig.keyFn KeyFn<T>(Action<T> action) => action.As<methodsConfig.keyFn>();
    }
}
