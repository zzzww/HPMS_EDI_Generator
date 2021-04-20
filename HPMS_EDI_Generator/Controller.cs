using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace HPMS_EDI_Generator
{
    public class CenteredDateTimePicker : DateTimePicker
    {
        public CenteredDateTimePicker()
        {
            SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), ClientRectangle, new StringFormat
			{
				// Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center
			});

			// Render the dropdown button: shrink button area by one px
			var rect = new Rectangle(ClientRectangle.Left + ClientRectangle.Width - 19, ClientRectangle.Top + 1,
									 19, ClientRectangle.Height - 2);
			if (ComboBoxRenderer.IsSupported)
			{
				var bstate = this.Enabled ? ComboBoxState.Normal : ComboBoxState.Disabled;
				ComboBoxRenderer.DrawDropDownButton(e.Graphics, rect, bstate);
			}
			else
			{
				var bstate = this.Enabled ? ButtonState.Flat : ButtonState.Inactive;
				ControlPaint.DrawComboButton(e.Graphics, rect, bstate);
			}
			ControlPaint.DrawBorder(e.Graphics, DisplayRectangle, Color.Gray, ButtonBorderStyle.Solid);
			base.OnPaint(e);
		}
    }



}
