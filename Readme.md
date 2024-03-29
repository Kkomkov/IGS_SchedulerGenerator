# IGS plant growing schedule generator
Schedule generator should assept json with trayNumber,recipeName and startDate
it should use external rest api as source of recipes and generates schedulere json with exect time to turn on light and what intencity and what time to turn on watter and what amount to use.


Sample of input json 
```typescript
{
  input: [
    {
      trayNumber: 1,
      recipeName: "Basil",
      startDate: "2022-01-24T12:30:00.0000000Z"
    },
    {
      trayNumber: 2,
      recipeName: "Strawberries",
      startDate: "2021-13-08T17:33:00.0000000Z"
    },
    {
      trayNumber: 3,
      recipeName: "Basil",
      startDate: "2030-01-01T23:45:00.0000000Z"
    }
  ]
}
```
## Assupmtions
According to result of Recipes API json litght phases duration are not correlate with water phase duration 
that why schedule should containe separate rows and time for light and watering operations.

An operation startDate is required. There is no information about necessity of operation endDate. 
It has sense for light and has less sense for water. 
Anyway I will add endDate field into schedule to make more obvious when each operation should end. 

## Expected result json schema
```typescript
{
	[ trayNumber:"integer",
	  lightSchedule:[ 
	  {
		startDate:"yyyy-mm-ddThh:mm:cc.0000000Z",
		endDate:"yyyy-mm-ddThh:mm:cc.0000000Z",
		intencity:"integer"		
	  }],
	  waterSchedule:[ 
	  {
		startDate:"yyyy-mm-ddThh:mm:cc.0000000Z",
		endDate:"yyyy-mm-ddThh:mm:cc.0000000Z",
		amount:"decimal"
	  }]	  
	]
}
```
According to my understandin startDate ,endDate and intencity/amount is a minimum set of field required to make a shedulere.

## How to generate a schedule:
Input containe trayNumber,recipeName,startDate.
We could use recipeName to found list of phases and operations in Recipes
Recipe return a list of phases with nested list of operations separately for light and water.
We need to go thought list and  add startDate to operation duration for each operation , repeate the process for repetitions value.

Phase hours,minutes help calculate last operation in a phase duration.

# How to use Api
SchedulerGenerator expose one endpoint
post:< host >/Scheduler 
The request assept  body - array of trays 

```typescript
[{
      trayNumber: 1,
      recipeName: "Basil",
      startDate: "2022-01-24T12:30:00.0000000Z"
    },
    {
      trayNumber: 2,
      recipeName: "Strawberries",
      startDate: "2021-13-08T17:33:00.0000000Z"
    }]
```

# How to setup
The app Scheduler Generator depends on external service RecipeApi.
Before running Scheduler Generator please check and adjust if needed connection settings for RecipeApi. 


## Option 1: Run Scheduler Generator from IDE 
  Check in AppSettings.<Environment>.Json ExternalServices.RecipeApi settins. Please edit it if RecipeApi has another connection parameters.
  Run RecipeAPI , Run Scheduler Generator with IDE
  
## Option 2: Run both applications in Docker-Compose
  To communicate with another application or container both containers must be in the same network.
### Assumptions: 
	RecipeAPI runs in a contaner with 
	_name_: tech-test-software-engineer_recipeapi_1 and 
	_network_: tech-test-software-engineer_default
  
  Docker-Compose.yml configured to put SchedulerGenerator container to the network tech-test-software-engineer_default. 
  
  If the assumptions right then no editing required just build and start Scheduler Generator container
#### To start container please execute in cmd from solution folder "Scheduler Generator" 
		docker-compose build 
		docker-compose up
  if RecipeAPI has another  container name or network
  please check RecipeApi container settings 
	and edit Docker-Compose.yaml set environment:
	
```typescript
environment:
      - ExternalServices__RecipeApi__Host=<RecipeApi container name>
      - ExternalServices__RecipeApi__Port=80
      - ExternalServices__RecipeApi__UseHttps=false
networks:
  default:
    external:
      name: <RecipeApi network>
```

#### To start container please execute in cmd from solution folder "Scheduler Generator" 
		docker-compose build 
		docker-compose up
		
		
## Option 3: Run Scheduler GeneratorRecipeAPI in container and RecipeApi is external service.
  Please edit Docker-Compose.YML and set 
```typescript 
environment:
      - ExternalServices__RecipeApi__Host
	  - ExternalServices__RecipeApi__Port
	  - ExternalServices__RecipeApi__UseHttps	  
```
#### To start container please execute in cmd from solution folder "Scheduler Generator" 
		docker-compose build 
		docker-compose up


  