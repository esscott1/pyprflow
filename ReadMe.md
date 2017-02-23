This API application provides endpoints for creating managing and deleting a workflow orchestration and the items which are being processed through a given workflow.

There are 2 types of workflow:  'Basic' and 'Complex' which are denoted by the Workflow property called "workflowType".
Workflows are created by submitting a JSON object that represent the structure and rules of the workflow.  Below are the JSON Schema and an example of both a Simple and Complex workflow type

 
Workflow JSON Schema
```
{
	"title": "Workflow Schema",
	"type": "object",
	"properties": {
		"workflowId": {
			"description": "The unique identifier for a product",
			"type": "string"
		},
		"key": {
			"description": "used for testing, surrogage for unique key during testing, remove",
			"type": "string"
		},
		"workflowName": {
				"description" : "human friendly name give to thw workflow",
				"type": "string"
			},
		"nodes": {
			"description": "a collection of nodes that represent a resting state in which items belong during thier journey though the workflow",
			"type": "array",
			"items": {
				"title": "node",
				"description" : "a resting state of a workflow that contains trackable items",
				"type": "object",
						"properties": {
							"trackables": {
								"description": "array of items within the given node",
								"type": "array",
								"items": {
									"title": "trackable",
									"description": "object being tracked within this step",
									"type": "object",
									"properties": {
										"trackableId": {
											"description": "unique identifier for this item",
											"type": "string"
										},
										"trackingName:" {
											"description": "human friendly name for this item",
											"type": "string"
										},
										"nodeNamesIn": {
											"description": "unique identifier for this item",
											"type": "array",
											"items": { "nodeName": "string" }
										}
									}
								}
							},
							"nodeName": {
								"type": "string"
							},
							"nodeDescription": {
								"type": "string"
							},
							"nodeId": {
								"description": "node unique identifier",
								"type": "string"
							}
						}
			}
		},
		"moves": {
			"description": "Array of available movements with the workflow",
			"type": "array",
			"items": {
				"title": "movements",
				"description": "defines a single available movement between steps in a workflow",
				"type": "object",
				"properties": {
					"from": {
						"description": "starting NodeName for movement",
						"type": "string"
					},
					"to": {
						"description": "ending NodeName for movement",
						"type": "string"
					}
				}
			}
		}
	},
	"required": [ "WorkflowId", "Key", "workflowName" ]
}

```

Workflow Example with simple steps and one item being tracked in first step

```
{
	"nodes": {
		"Step1": {
			"trackables": [
				{
					"trackableId": null,
					"trackingName": "document2",
					"nodeNamesIn": []
				}
			],
			"nodeName": "Step1",
			"nodeDescription": null,
			"nodeId": "0"
		},
		"Step2": {
			"trackables": [
				{
					"trackableId": "unique1",
					"trackingName": "document1",
					"nodeNamesIn": [
						"Step2"
					]
				}
			],
			"nodeName": "Step2",
			"nodeDescription": null,
			"nodeId": "0"
		},
		"Step3": {
			"trackables": [],
			"nodeName": "Step3",
			"nodeDescription": null,
			"nodeId": "0"
		}
	},
	"trackingComments": [],
	"moves": [
		{
			"from": "Step1",
			"to": "Step2",
			"approveUsers": []
		}
	],
	"workflowId": "5bb80835-05a8-435b-b273-205f73f1d700",
	"key": "_blankKeyes21",
	"workflowName": "_blankKeyes21"
}

```