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
				"description":  "used for testing, surrogage for unique key during testing, remove",
				"type": "string"
			},
		"workflowName": {
				"type": "string"
			},
			"steps": {
				"description": "a collection of steps",
				"type": "array",
				"items": {
					"title": "Step",
					"type": "object",
					"properties": {
						"step_id": {
							"description": "step unique identifier",
							"type": "string"
						},
						"ITrackables": {
							"description": "array of ojects within a given step",
							"type": "array",
							"items": {
								"title": "itrackable",
								"description": "object being tracked within this step",
								"type": "object",
								"properties": {
									"trackingId": {
										"description": "unique identifier for this item",
										"type": "string"
									},
									"itemId": {
										"description": "unique identifier for this item",
										"type": "number"
									},
									"trackingName:" {
										"description": "human friendly name for this item",
										"type": "string"
									}
								}
							}
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
							"description": "starting step for movement",
							"type": "string"
						},
						"to": {
							"description": "ending step for movement",
							"type": "string"
						}
					}
				},
			}
	},
	"required": [ "WorkflowId", "Key", "workflowName" ]
}
```

Workflow Example with simple steps and one item being tracked in first step

``` {
	"workflowId": "WorkflowExample1",
	"key": "_WorkflowExample1",
	"workflowId": "shouldbeGUID",
	"steps": [
		{
			"step_id": "Step1",
			"ITrackables": [
				{
					"trackingId": "docId1Guid",
					"itemId": 1,
					"trackingName": "Document1"
				},
				{
					"trackingId": "docId2Guid",
					"itemId": 2,
					"trackingName": "Document2"
				}
			]
		},
		{
			"step_id": "Step2",
			"ITrackables": []
		},
		{
			"step_id": "Step3",
			"ITrackables": []
		},
		{
			"step_id": "Step4",
			"ITrackables": []
		}
	],
	"moves": [
		{
			"from": "Step1",
			"to": "Step2"
		},
		{
			"from": "Step2",
			"to": "Step3"
		},
		{
			"from": "Step3",
			"to": "Step4"
		}
	]
}

```