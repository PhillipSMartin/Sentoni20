using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SentoniServiceLib;

namespace DeltasToTradeTester
{
    public partial class Form1 : Form
    {
        double m_currentEquity;
        double m_deltaGoal;
        double m_totalFaceValuePutsPct;

        int m_numberOfIndices;
        List<SentoniServiceLib.Index> m_indicesTable = new List<SentoniServiceLib.Index>();
        TextBox[][] m_inputMap;
        Label[][] m_outputMap;
        double m_currentDelta;

        public Form1()
        {
            InitializeComponent();

            // set defaults
            textBoxCurrentEquity.Text = "127858944.37";
            textBoxDeltaGoal.Text = ".5";
            textBoxNumIndices.Text = "3";

            textBoxIndex1Weight.Text = ".028";
            textBoxIndex1Price.Text = "5768.47";
            textBoxIndex1Delta.Text = ".489";
            textBoxIndex1NumPuts.Text = "0";

            textBoxIndex2Weight.Text = ".334";
            textBoxIndex2Price.Text = "1356.59";
            textBoxIndex2Delta.Text = ".744";
            textBoxIndex2NumPuts.Text = "126";

            textBoxIndex3Weight.Text = ".638";
            textBoxIndex3Price.Text = "2425.12";
            textBoxIndex3Delta.Text = ".382";
            textBoxIndex3NumPuts.Text = "140";

        }
 
 
        private void Form1_Load(object sender, EventArgs e)
        {
            m_inputMap = new TextBox[][]
            {
                new TextBox[] { textBoxIndex1Weight, textBoxIndex1Price, textBoxIndex1Delta, textBoxIndex1NumPuts },
                new TextBox[] { textBoxIndex2Weight, textBoxIndex2Price, textBoxIndex2Delta, textBoxIndex2NumPuts },
                new TextBox[] { textBoxIndex3Weight, textBoxIndex3Price, textBoxIndex3Delta, textBoxIndex3NumPuts },
            };
            m_outputMap = new Label[][]
            {
                new Label[] { labelIndex1DeltasToTrade, labelIndex1ResultingDelta, labelIndex1PutsToRebalance, labelIndex1FaceValuePutsPct, labelIndex1PutsToTrade },
                new Label[] { labelIndex2DeltasToTrade, labelIndex2ResultingDelta, labelIndex2PutsToRebalance, labelIndex2FaceValuePutsPct, labelIndex2PutsToTrade },
                new Label[] { labelIndex3DeltasToTrade, labelIndex3ResultingDelta, labelIndex3PutsToRebalance, labelIndex3FaceValuePutsPct, labelIndex3PutsToTrade },
            };
        }


        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            buttonCalculate.Enabled = false;

            if (m_deltaGoal > 0)
            {
                SentoniHost.DataProvider.AdjustDeltaSandbox[] sandBox = SentoniHost.DataProvider.CalculateDeltasToTrade(m_deltaGoal > m_currentDelta, m_deltaGoal, m_indicesTable);

                for (int i = 0; i < m_numberOfIndices; i++)
                {
                    m_outputMap[i][0].Text = sandBox[i].deltasToTrade.ToString("F3");
                    m_outputMap[i][1].Text = sandBox[i].targetDeltaPercent.ToString("F3");
                }
            }

            for (int i = 0; i < m_numberOfIndices; i++)
            {

                if (m_indicesTable[i].LastPrice > 0)
                {
                    m_outputMap[i][2].Text = String.Format("{0}", (int)Math.Round((.4 - m_indicesTable[i].FaceValuePutsPct) * m_indicesTable[i].TargetValue / (100 * m_indicesTable[i].LastPrice.Value), 0));
                }
            }

            if (Math.Abs(m_totalFaceValuePutsPct - .4) > .001)
            {
                int putsToTrade = SentoniHost.DataProvider.CalculatePutsToTrade((.4 - m_totalFaceValuePutsPct) * m_currentEquity, m_indicesTable);
                labelTotalPutsToTrade.Text = String.Format("{0}", putsToTrade);

                  for (int i = 0; i < m_numberOfIndices; i++)
                    {

                        if (m_indicesTable[i].LastPrice > 0)
                        {
                            m_outputMap[i][4].Text = String.Format("{0}", m_indicesTable[i].PutsToTrade);
                        }
                    }

            }
  //          labelTotalPutsToTrade.Text = String.Format("{0}", SentoniHost.DataProvider.CalculateTotalPutsToTrade(.4, m_currentEquity, m_totalFaceValuePutsPct, m_indicesTable));
        }

        private void buttonVerify_Click(object sender, EventArgs e)
        {
           buttonCalculate.Enabled = false;
           labelErrorMsg.Text = "";

            if (!Double.TryParse(textBoxCurrentEquity.Text, out m_currentEquity))
            {
                labelErrorMsg.Text = "Current equity must be a number";
                return;
            }
            if (m_currentEquity <= 0)
            {
                labelErrorMsg.Text = "Current equity must be positive";
                return;
            }
            if (!Double.TryParse(textBoxDeltaGoal.Text, out m_deltaGoal))
            {
                labelErrorMsg.Text = "Delta goal must be a number";
                return;
            }
            if ((m_deltaGoal < 0) || (m_deltaGoal > .9))
            {
                labelErrorMsg.Text = "Delta goal must be between 0 and .90";
                return;
            }
            if (!Int32.TryParse(textBoxNumIndices.Text, out m_numberOfIndices))
            {
                labelErrorMsg.Text = "Number of indices must be a number";
                return;
            }
            if ((m_numberOfIndices < 1) || (m_numberOfIndices > 3))
            {
                labelErrorMsg.Text = "Number of indices must be 1, 2, or 3";
                return;
            }

            m_indicesTable.Clear();
            decimal totalWeight = 0;
            m_totalFaceValuePutsPct = 0;
            m_currentDelta = 0;

            for (int i = 0; i < m_numberOfIndices; i++)
            {
                Index row = new Index();
                for (int j = 0; j < 4; j++)
                {
                    row.Symbol = String.Format("Index{0}", i + 1);
                    decimal output;
                    if (!Decimal.TryParse(m_inputMap[i][j].Text, out output))
                    {
                        labelErrorMsg.Text = "Index parameters must be numbers";
                        return;
                    }
                    if (j == 0)
                    {
                        totalWeight += output;
                        if ((output < 0) || (output > 1))
                        {
                            labelErrorMsg.Text = "Weights must be between 0 and 1";
                            return;
                        }
                        row.Weight = output;
                        row.TargetValue = m_currentEquity * (double)output;
                    }
                    else if (j == 1)
                    {
                        if (output < 0)
                        {
                            labelErrorMsg.Text = "Price must be positive";
                            return;
                        }
                        row.LastPrice = (double)output;
                    }
                    else if (j == 2)
                    {
                        if ((output < 0) || (output > 1))
                        {
                            labelErrorMsg.Text = "Delta must be between 0 and 1";
                            return;
                        }
                        row.TotalDeltaPct = (double)output;
                        m_currentDelta += row.TotalDeltaPct * (double)row.Weight;
                    }
                    else
                    {
                        int numputs = (int)Math.Round(output);
                        m_inputMap[i][j].Text = numputs.ToString();

                        row.FaceValuePutsPct = numputs * 100 * row.LastPrice.Value / row.TargetValue;
                        m_totalFaceValuePutsPct += numputs * 100 * row.LastPrice.Value;

                        m_outputMap[i][3].Text = String.Format("{0:F3}", row.FaceValuePutsPct);

                        row.PutsToRebalance = (int)Math.Round((.4 - row.FaceValuePutsPct) * row.TargetValue / (100 * row.LastPrice.Value), 0);
                        m_outputMap[i][2].Text = String.Format("{0:F3}", row.PutsToRebalance);
                    }

                }

                m_indicesTable.Add(row);
            }

            m_totalFaceValuePutsPct /= m_currentEquity;
            labelTotalFaceValuePutsPct.Text = String.Format("{0:F3}", m_totalFaceValuePutsPct);

            if (totalWeight != 1)
            {
                labelErrorMsg.Text = "Weights must add to 1";
                return;
            }

            // add a 'Total' row to index table - it won't be used
            //var totalRow = new Index();
            //totalRow.Symbol = "Total";
            //m_indicesTable.Add(totalRow);


            labelCurrentDelta.Text = m_currentDelta.ToString();
            buttonCalculate.Enabled = true;

            
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            buttonCalculate.Enabled = false;
        }
    }
}
