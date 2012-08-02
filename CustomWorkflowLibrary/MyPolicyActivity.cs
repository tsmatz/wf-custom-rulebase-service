using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Xml;

namespace CustomWorkflowLibrary
{
	public partial class MyPolicyActivity: Activity
	{
		public MyPolicyActivity()
		{
			InitializeComponent();
		}

        public string RuleFilePath
        {
            get { return (string)GetValue(RuleFilePathProperty); }
            set { SetValue(RuleFilePathProperty, value); }
        }

        public static readonly DependencyProperty RuleFilePathProperty =
            DependencyProperty.Register("RuleFilePath", typeof(string), typeof(MyPolicyActivity));
    
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            XmlReader reader = new XmlTextReader(RuleFilePath);
            WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
            RuleDefinitions ruleDef = (RuleDefinitions)serializer.Deserialize(reader);
            reader.Close();

            // ルートアクティビティを取得
            Activity root = this;
            while (root.Parent != null)
                root = root.Parent;

            for (int i = 0; i < ruleDef.RuleSets.Count; i++)
            {
                RuleSet myRuleSet = ruleDef.RuleSets[i];
                RuleEngine ruleEng = new RuleEngine(myRuleSet, root.GetType());
                ruleEng.Execute(root, executionContext);
            }

            return base.Execute(executionContext);
        }
	}
}
