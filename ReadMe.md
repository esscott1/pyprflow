This API application provides endpoints for creating, managing, and deleting an orchestration workflow.  It allow for items (trackables) to be submitted into and managed through 
the lifecycle of the orchestration workflow.  To access the endpoints you will need to have a HTTP Header key = user-key with a value of "test" in each of your
API calls.

###What is an orchestration workflow?
An orchestration workflow defines a logical flow of activities or tasks from a start event to an end event to accomplish a specific purpose or service.

###What is an example of an orchestration workflow (OW) in action?
One typical example is the process of reviewing and approving documents.  In this example the OW defines the logical flow or path a document would 
travel through in it's journey to be 'approved.'  First a document is created, then submitted into a previously defined OW.  As different actions
 are performed (physical review by individuals; an amount of time has passed, a computerized event occurs),the document is advanced (or retarded) to the 
 next step(s) in the OW.  In this example the specific purpose is to produce an "approved" document.  this may require various changes and re-reviews,
 all of which are tracked and facilitated by the OW.
 
 ###Are orchestration workflows single directional?
 No, orchestration workflows support multiple direction.  In the above example, a document that needs modification may be retarded backwards to a less
 mature step in the defined activity flow.

 ###Are orchestration workflows single dimensional or serial in nature? 
 No, simple OW are single dimensional, where one task leads to another task and another in a serial mannor.  OW can also be complex where completion of a 
 single task may spawn need for multiple other tasks to be executed in parallel. Similarly, a task that needs to be done may require the completion of multiple 
 unrelated tasks to be complete before that task can be attempted.

 ###How are orchestration workflows represented in this tool?
Workflows are created by submitting a JSON object that represent the structure and rules of the workflow.  Below are the JSON Schema and examples of both a Simple
 and Complex workflow.  Upon submission workflows are validated.  You can pre-validate by using the API/Workflow/Validate api.  Workflows must have at least one starting 
 node and at least one ending node.  Path validation is also required, where every node must have at least one entry point and one exit point.  the exceptions are, 
 starting nodes do not need a explicitly entry paths, and ending nodes do not need to have explicit exit paths.

 ###How do I initiate a orchestration workflow?
 Orchestration workflows are structured representations of business processes.  Initiating a business process requires something (ie. a document) to be submitted to 
 process (OW) for facilitation and tracking.

 ###How do I submit something to a orchestration workflow?
 Any item can be submitted to an orchestration workflow.  The details of the item should be stored in a system other than the orchestration workflow system.  However, 
 a unique identifier is required for the orchestration workflow to track and facilitate.  It also enable the items system of record to easily match the items information 
 with it's location and status within the orchestration workflow.  Below is a JSON schema and JSON example of a WorkflowItemUpdate.  all item management requires submission 
 of a WorkflowItemUpdate to the OW engine.'
 
Workflow JSON Schema
```JSON
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

```JSON
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

## Submitting and Managing items through a orchestration Workflow

To submit an item to an orchestration workflow you simply Post a workflowUpdate JSON object to the API/trackable/start.  Note that the workflowUpdate JSON object requires 
the workflowID and a trackableId for the item you are submitting.  This trackableId can be created by the submitter or you can request a trackableId from the system 
via Get API/trackable/newId.  If the API/trackable/start receives a trackableId that is already being used within the OW, an error will be returned and the item will not 
be accepted to the OW.  To verify the uniqueness of the trackableId, you can Post to API/trackable/isunique/{trackableId} for a boolean result.