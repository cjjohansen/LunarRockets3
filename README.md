# Lunar Rockets Code Challenge

# 🪐 Backend Engineer Challenge: Rockets - Solution 🚀


## Notice
1. Rockets.exe will probably not work with this implementation. I could probably fix this quite easily. But don't waste time trying to run it.  

## Initial thoughts and steps 👋

1. Make sure that .exe works. Done! (Needs to be executed from command prompt --help works)
2. Meta data allows for generic implementation of message deduplication and securing message order. Could be done with Decorator pattern.
3. Two tactics to solve out of order problem. 
    1. Simply reject out of order messages and fall back on retry mechanism
    2. Store out of order message, but ignore in replay until missing messages has been received.

4. A TDD way of solving the problem could be a nice way to go. Have to keep track of time though.
5. Finding a good example to work from could save tremendous amount of time while illustrating knowledge to select the right sample
6. Creating an event model. Would be a nice way to analyze the problem with out code first.
7. Readmodel projections needs to be created.


## Steps taken
 1. Decided to go with example at https://github.com/oskardudycz/EventSourcing.NetCore/tree/main/Sample/EventStoreDB/ECommerce
 2. Converted sample to match Rockets solution.
     1. This was probably a bad choice since it took to long time.
     2. After finishing, I cannot get the rockes.exe to match my endpoints at least an error prevents my endpoint to be hit. 
     3. TRying to install fiddler but I can still not investigate what the rockets.exe file sends. 
 3. using a more simpler approach using tdd, an maybe an in memory dictionaty to store event streams and in memory repsository to store projections could be used
 
 ## Evaluation
 1. The advantage of starting out with a full example is that I use docker and use postgres Db and event store Db. 
     1. I also use an opensource framework and show that I can produce alot of code in the given time. using concepts like projections.
     
 2. Disadvantage is that i didn't get to show how to solve some of the given problems like handling message duplicates and out of order messages.
 3. Disadvantage that the coding style will reflect the used sample more than my own code style.
 4. Accomplishments
     1. exaple Api tests added
     2. example tests added
     3. Swagger UI is functional
     4. Events are stored in EventStoreDb
     5. Projections stored in PostGres
     6. rocket history end point implemented 

Sample is showing the typical flow of the Event Sourcing app with [EventStoreDB](https://developers.eventstore.com).

## Prerequisities

1. Install git - https://git-scm.com/downloads.
2. Install .NET Core 6.0 - https://dotnet.microsoft.com/download/dotnet/6.0.
3. Install Visual Studio 2022, Rider or VSCode.
4. Install docker - https://docs.docker.com/docker-for-windows/install/.
5. Open `LunarRockets.sln` solution.

## Running

1. Go to [docker](./docker) and run: `docker-compose up`.
2. Wait until all dockers got are downloaded and running.
3. You should automatically get:
    - EventStoreDB UI (for event store): http://localhost:2113/
    - Postgres DB running (for read models)
    - PG Admin - IDE for postgres. Available at: http://localhost:5050.
        - Login: `admin@pgadmin.org`, Password: `admin`
        - To connect to server Use host: `postgres`, user: `postgres`, password: `Password12!`
4. Open, build and run `LunarRockets.sln` solution.
    - Swagger should be available at: http://localhost:5000/index.html
5.  The example uses optimistic concurrency checking, so the rockets.exe might not work out of the box since it shold send expected version in if-match request header. 
I Didnt have time to fix it. I Could offcause get the expected version send from the meta-data part.
6. It makes sense to test using swagger. Use json snippets from the TestSnippets.json file
7. After sending initial LaunchRocket message, notice the Etag W/"0" in the response headers. 
8. Send subsequent messages but remember to set W/"0" in the if match header input field. And W/"1" and so on..
9. if You want to test with Rockets.exe any way; you should use
        
   > rockets.exe launch "http://localhost:5000/api/rockets" --message-delay=500ms --concurrency-level=1 







## Trivia

1. Docker useful commands
    - `docker-compose up` - start dockers
    - `docker-compose kill` - to stop running dockers.
    - `docker-compose down -v` - to clean stopped dockers.
    - `docker ps` - for showing running dockers
    - `docker ps -a` - to show all dockers (also stopped)

