using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnateGlory
{
    public class a
    {
        public static void xx(object input)
        {
            HTMLCanvasElement canvas = input as HTMLCanvasElement;
            if (canvas == null) return;
            var cc = canvas.GetContext(CanvasTypes.CanvasContext2DType.CanvasRenderingContext2D);
        }
    }
}
