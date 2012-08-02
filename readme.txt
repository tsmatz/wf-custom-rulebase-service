/// This demo is for :

  Custom Rule base application sample.
  This rule is read dynamically using outer resource (.rules local file).

  Project:
  CustomWorkflowLibrary
    This include custom activity (which uses outer resource),
    and workflow libary.
  CustomWorkflowConsole
    This launched workflow using WCF.
  ClientApplication
    This is a Windows client using workflow service.
  PolicyWorkbook
    This can change and deploy rules using Excel spreadsheet.
    (The end-user can set specific business rules !)

/// How to install

1. Modify service endpoint address in app.config at ClientApplication.
2. Set RuleFilePath properties at myPolicyActivity1 in CustomWorkflowLibrary.
  By default, this uses Workflow1.rules in solution folder.

This sample uses CodeDom and WorkflowMarkupSerializer.
