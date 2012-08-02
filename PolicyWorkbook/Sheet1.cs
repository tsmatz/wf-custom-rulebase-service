using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using System.Workflow.Activities.Rules;
using System.CodeDom;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;

namespace PolicyWorkbook
{
    public partial class Sheet1
    {
        private void Sheet1_Startup(object sender, System.EventArgs e)
        {
        }

        private void Sheet1_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO で生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InternalStartup()
        {
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.fileRefButton.Click += new System.EventHandler(this.fileRefButton_Click);
            this.Shutdown += new System.EventHandler(this.Sheet1_Shutdown);
            this.Startup += new System.EventHandler(this.Sheet1_Startup);

        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Excel.Range cell;

            RuleSet myRuleset = new RuleSet("RuleSet1");

            // Define property and activity reference expressions through CodeDom functionality
            CodeThisReferenceExpression refexp = new CodeThisReferenceExpression();
            CodePropertyReferenceExpression refTotalCost = new CodePropertyReferenceExpression(refexp, "totalCost");
            CodePropertyReferenceExpression refParamCategory = new CodePropertyReferenceExpression(refexp, "paramCategory");
            CodePropertyReferenceExpression refParamPrivilege = new CodePropertyReferenceExpression(refexp, "paramPrivilege");

            // Example :
            // IF paramCategory == 3
            // THEN totalCost = totalCost + 300

            for (int row = 4; row <= 8; row++)
            {
                cell = (Excel.Range)this.Cells[row, 2];
                if (String.IsNullOrEmpty((string)cell.Value2))
                    break;
                Rule myRule = new Rule("Rule" + row);
                myRuleset.Rules.Add(myRule);

                // Example :
                // paramCategory == 3
                CodeBinaryOperatorExpression ruleCondition = new CodeBinaryOperatorExpression();
                if ((string)cell.Value2 == "種別 (category)")
                    ruleCondition.Left = refParamCategory;
                else if ((string)cell.Value2 == "特典 (privilige)")
                    ruleCondition.Left = refParamPrivilege;
                ruleCondition.Operator = CodeBinaryOperatorType.ValueEquality;
                cell = (Excel.Range)this.Cells[row, 3];
                ruleCondition.Right = new CodePrimitiveExpression((int)(double)cell.Value2);
                myRule.Condition = new RuleExpressionCondition(ruleCondition);

                // Example :
                // totalCost = totalCost + 300
                CodeAssignStatement ruleAction = new CodeAssignStatement();
                ruleAction.Left = refTotalCost;
                CodeBinaryOperatorExpression actionRight = new CodeBinaryOperatorExpression();
                actionRight.Left = refTotalCost;
                cell = (Excel.Range)this.Cells[row, 4];
                if((string)cell.Value2 == "+")
                    actionRight.Operator = CodeBinaryOperatorType.Add;
                else if ((string)cell.Value2 == "-")
                    actionRight.Operator = CodeBinaryOperatorType.Subtract;
                else if ((string)cell.Value2 == "*")
                    actionRight.Operator = CodeBinaryOperatorType.Multiply;
                else if ((string)cell.Value2 == "/")
                    actionRight.Operator = CodeBinaryOperatorType.Divide;
                cell = (Excel.Range)this.Cells[row, 5];
                actionRight.Right = new CodePrimitiveExpression((int)(double)cell.Value2);
                ruleAction.Right = actionRight;
                myRule.ThenActions.Add(new RuleStatementAction(ruleAction));
            }

            // 必要に応じ、RuleValidation オブジェクトを使ってチェック！
            // (今回は省略 . . . . . . .)

            // RuleDefinitions を設定して保存
            RuleDefinitions ruleDef = new RuleDefinitions();
            ruleDef.RuleSets.Add(myRuleset);
            WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
            cell = (Excel.Range) this.Cells[11, 2];
            XmlTextWriter writer = new XmlTextWriter((string) cell.Value2, System.Text.Encoding.Unicode);
            serializer.Serialize(writer, ruleDef);
            writer.Close();

            // ここでは、すぐにコンパイルして実行
            // (Custom WorkflowRuntime Service と Custom Policy Activity を作成して、データベースなど独自の Rule Cache を作成することも可能 . . .)

            MessageBox.Show("ルールを反映しました");
        }

        private void fileRefButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = @"Ruleファイル(*.rules;*.rule)|*.rules;*.rule|すべてのファイル(*.*)|*.*";
            dlg.CheckFileExists = true;
            if (dlg.ShowDialog() == DialogResult.OK)
                this.Cells[11, 2] = dlg.FileName;
        }

    }
}
