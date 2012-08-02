using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace CustomWorkflowLibrary
{
	public sealed partial class Workflow1 : SequentialWorkflowActivity
	{
		public Workflow1()
		{
			InitializeComponent();
		}

        public int paramUnitCost { get; set; }
        public int paramVolume { get; set; }
        public int paramCategory { get; set; }
        public int paramPrivilege { get; set; }
        public int totalCost { get; set; }

        private void initializeActivity_ExecuteCode(object sender, EventArgs e)
        {
            totalCost = paramUnitCost * paramVolume;
        }
    }
}
