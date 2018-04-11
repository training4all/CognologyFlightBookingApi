# CognologyFlightBookingApi
This is the Web API Created to display and update data regarding flights and bookings.

Solution Architecture
Solution CognologyFlightBooking API consists of 2 projects
1. CognologyFlightBooking.Api - This is the main API project which has all the model classes, data transfer objects (Dto), controllers, configuration files and so on.
2. CognologyFlightBooking.Api.Data - This is the data project which provide and seed data to database using entity framework core and content extension method. It contains all the entities, 

context, repositories and so on. Repository pattern is followed to inject data dependency at run time.
3. Configurations - connection string is stored in project environment variables rather  than in app config to deny common access to all.
Connection string key -  connectionString:flightInfoDBConnectionString
connection string value- Server=(localdb)\mssqllocaldb;Database=FlightInfo;Trusted_Connection=True;
It will install and seed local sql database installed with visual studio which can be viewed using sql server explorer window in visual studio.

Technology Stack
1. IDE - Visual Studio 2017
2. Languages - C# 6, EF Core, LINQ
3. Logging - Console, Debug and NLog
4. Mapping Mechanism - AUtomapper extension of microsoft

-----------------------------------------------------------------------------------------------------
All operations Related to Passengers are inside PassengerController
1. Get All passengers
ApI Url - http://localhost:49257/api/passengers/
HTTP Method- GET

2. Get Passenger By ID
API Url - http://localhost:49257/api/passengers/{passengerID}
HTTP Method- GET
For current project since we have some  dummy data already seeded to DB, you can try fetching Passenger with id=1
API Url - http://localhost:49257/api/passengers/1

3. Create Passenger
API Url - http://localhost:49257/api/passengers/
HTTP Method - POST
Request JSON - 
{
"Name" : "Alexa",
"Mobile" : "0876 123 456"
}
Expected Response Body - 
{
"Name" : "Alexa",
"Mobile" : "0876 123 456"
}
Expected Response Header- It will contain location of newly created resource like
Location → http://localhost:49257/api/passengers/4


4. Try to create passenger which already exists,
API Url - http://localhost:49257/api/passengers/
HTTP Method - POST
Request JSON - 
{
"Name" : "Alexa",
"Mobile" : "0876 123 456"
}
Expected Response Body - 
{
    "Passenger": ["Passenger Already Exists."]

}
--------------------------------------------------------------------------------------------------
All operations Related to flights are inside FlightController

1. Get All Flights
API Url - http://localhost:49257/api/flights
HTTP Method- GET

2. Get Flight by Flight Number - from above get flights you will have flight number which you can use to search for a particular flight based on that flight number
API Url - http://localhost:49257/api/flights/{flightNumber}
HTTP Method- GET
For current project since we have some  dummy data already seeded to DB, you can try fetching flight with flight number = F100
API Url - http://localhost:49257/api/flights/F100

3. Create Flight
API Url - http://localhost:49257/api/flights/
HTTP Method - POST
Request JSON - 
{
    "flightNumber": "F400",
    "startTime": "13:00:00",
    "endTime": "15:00:00",
    "passengerCapacity": 23,
    "departerCity": "MEL",
    "arrivalCity": "SYD"
}
--------------------------------------------------------------------------------------------------
All operations Related to bookings are inside FlightBookingController

1. Get All bookings
API Url - http://localhost:49257/api/bookings/
HTTP Method- GET
Since, we are not adding any bookings on start of application so you will see an empty array. Hence you need to create some bookings.

2. Make Booking
API Url - http://localhost:49257/api/bookings/
HTTP Method - POST
Request JSON 1 -
{
"Date" : "2018-01-10",
"ArrivalCity" : "MEL",
"DeparterCity" : "SYD",
"NumberOfPassengers" : 10,
"FlightNumber" : "F100",
"Passenger": {
"Name" : "Tim",
"Mobile" : "0420 123 456"
}
}
Once you execute above you will notice that In response header you will get location of newly created resource/booking.Since this is the first booking you are creating so resource id is 1.
Response Header -
Location →http://localhost:49257/api/bookings/1

To test, Check Avaialability fucntionality we need some test data to be inserted into booking table. Hence make below additional 3 create booking call in addition to above create booking.
Request JSOn 2:
{
"Date" : "2018-01-02",
"ArrivalCity" : "SYD",
"DeparterCity" : "MEL",
"NumberOfPassengers" : 10,
"FlightNumber" : "F100",
"Passenger": {
"Name" : "Michelle",
"Mobile" : "0876 123 456"
}
}

Request JSOn 3:
{
"Date" : "2018-01-08",
"ArrivalCity" : "MEL",
"DeparterCity" : "SYD",
"NumberOfPassengers" : 10,
"FlightNumber" : "F100",
"Passenger": {
"Name" : "Michelle",
"Mobile" : "0876 123 456"
}
}

Request JSOn 4:
{
"Date" : "2018-01-07",
"ArrivalCity" : "MEL",
"DeparterCity" : "SYD",
"NumberOfPassengers" : 20,
"FlightNumber" : "F200",
"Passenger": {
"Name" : "Michelle",
"Mobile" : "0876 123 456"
}
}

--------------------------------------------------------------------------
To search for flight, use SearchBookingController.

1.Search booking based on passengerName, flightNumber, arrivalCity, departerCity and bookingDate
API Url - http://localhost:49257/api/searchbookings/Tim?flightNumber=F100&arrivalCity=MEL&departerCity=SYD&bookingDate=2018-01-10
HTTP Method - POST

If we proceeded as per the request json data provided in above steps then you will see 1 booking 
Sample Response JSON-
[
    {
        "date": "2018-01-10T00:00:00",
        "arrivalCity": "MEL",
        "departerCity": "SYD",
        "numberOfPassengers": 10,
        "flight": {
            "flightNumber": "F100",
        

    "startTime": "13:00:00",
            "endTime": "15:00:00",
            "passengerCapacity": 23,
            "departerCity": "MEL",
            "arrivalCity": "SYD"
        },
        "passenger": {
  

          "name": "Tim",
            "mobile": "0420 123 456"
        }
    }
]

-------------------------------------------------------------------------
To check availability of flights based on start date, end date and number of passengers
API Url - http://localhost:49257/api/availableflights/3?startDate=2018-01-01&endDate=2018-01-05
HTTP Method - GET

If we proceeded as per the request json data provided in above steps then you will see 2 flights available
Sample Response JSON-
[
    {
        "flightNumber": "F100",
        "startTime": "13:00:00",
        "endTime": "15:00:00",
        "passengerCapacity": 23,
        "departerCity": "MEL",
        "arrivalCity": "SYD"
    },
   

 {
        "flightNumber": "F300",
        "startTime": "06:00:00",
        "endTime": "12:00:00",
        "passengerCapacity": 20,
        "departerCity": "MEL",
        "arrivalCity": "QLD"
    }
]
--------------------------------------------------------------------------
