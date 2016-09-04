using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Gravity {
    class AutoCanvas : Canvas {

        protected override Size MeasureOverride(Size constraint) {

            base.MeasureOverride(constraint);

            var width = InternalChildren
                .OfType<UIElement>()
                .Max(i => i.DesiredSize.Width + (double) i.GetValue(LeftProperty));

            var height = InternalChildren
                .OfType<UIElement>()
                .Max(i => i.DesiredSize.Height + (double) i.GetValue(TopProperty));

            return new Size(width, height);
        }
    }
}
