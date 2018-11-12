using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SentoniClient
{
    class Cell
    {
        private Label m_label;
        private Label[] m_linkedLabels;
        private string m_formatString;
        private bool m_visibleIfZero;
        private double? m_threshold = null;
        private System.Drawing.Color m_lowColor = System.Drawing.Color.Black;
        private System.Drawing.Color m_highColor = System.Drawing.Color.Black;
        private System.Drawing.Color m_equalColor = System.Drawing.Color.Black;

        public Cell(Label label, string formatString, bool visibleIfZero = true, Label[] linkedLabels = null)
        {
            m_label = label;
            m_linkedLabels = linkedLabels;
            m_formatString = formatString;
            m_visibleIfZero = visibleIfZero;
        }

        public Cell(Label label, string formatString, double? threshold, System.Drawing.Color lowColor, System.Drawing.Color highColor, bool visibleIfZero=true, Label[] linkedLabels = null)
            : this(label, formatString, visibleIfZero, linkedLabels)
        {
            m_threshold = threshold;
            m_lowColor = lowColor;
            m_highColor = highColor;
        }

        public void SetValue(object value, bool invertColors)
        {
            SetValue(value);

            if ((m_linkedLabels != null) && invertColors)
            {
                foreach (Label linkedLabel in m_linkedLabels)
                {
                    if (linkedLabel.ForeColor != m_equalColor)
                    {
                        linkedLabel.BackColor = linkedLabel.ForeColor;
                        linkedLabel.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
        }

        public void SetValue(string value)
        {
            m_label.Text = String.Format(m_formatString, value);
        }

        public void SetValue(double value)
        {
            m_label.Visible = true;
            if (!m_visibleIfZero)
            {
                if (Math.Abs(value - 0) < .0001)
                    m_label.Visible = false;
            }

            if (m_label.Visible)
            {
                m_label.ForeColor = m_equalColor;
                m_label.Text = String.Format(m_formatString, value);

                if (m_threshold.HasValue)
                {
                    if (Math.Abs(value - Threshold.Value) >= .0001)
                    {
                        if (value < Threshold)
                            m_label.ForeColor = m_lowColor;
                        else if (value > Threshold)
                            m_label.ForeColor = m_highColor;
                    }
                }
            }


            if (m_linkedLabels != null)
            {
                foreach (Label linkedLabel in m_linkedLabels)
                {
                    linkedLabel.Visible = m_label.Visible;
                    linkedLabel.ForeColor = m_label.ForeColor;
                    linkedLabel.BackColor = System.Drawing.Color.Transparent;
                }
            }
        }

        public void SetValue(TimeSpan value)
        {
            m_label.Visible = true;
            m_label.ForeColor = m_equalColor;
            m_label.Text = String.Format(m_formatString, value);

            if (value <= DateTime.Now.TimeOfDay)
                m_label.ForeColor = m_highColor;
            else
                m_label.ForeColor = m_lowColor;


            if (m_linkedLabels != null)
            {
                foreach (Label linkedLabel in m_linkedLabels)
                {
                    linkedLabel.Visible = m_label.Visible;
                    linkedLabel.ForeColor = m_label.ForeColor;
                    linkedLabel.BackColor = System.Drawing.Color.Transparent;
                }
            }
        }

        public void SetValue(int value)
        {
            SetValue((double)value);
        }

        public void SetValue(decimal value)
        {
            SetValue((double)value);
        }

        public void SetValue(object value)
        {
            if (value.GetType() == typeof (string))
            {
                SetValue((string)value);
            }
            else if (value.GetType() == typeof(double))
            {
                SetValue((double)value);
            }
            else if (value.GetType() == typeof(int))
            {
                SetValue((int)value);
            }
            else if (value.GetType() == typeof(decimal))
            {
                SetValue((decimal)value);
            }
            else if (value.GetType() == typeof(TimeSpan))
            {
                SetValue((TimeSpan)value);
            }
        }
        public double? Threshold { get { return m_threshold; } set { m_threshold = value; } }
    }
}
