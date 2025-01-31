##### RidePal Playlist Generator of Team 3

##### Team 3 members are Maria Topchieva and Georgi R. Georgiev, part of Telerik Academy Alpha .NET 23.

##### Our Trello Board: https://trello.com/b/NswUvxXO/ridepal-playlist-generator-team-3

<br />

# **RidePal - Playlist Generator project**

##### **The RidePal project consists of web application and REST API.**
##### **The RidePal web application enables users to create playlists for specific travel duration periods based on their preferred music genres.**
##### **The _mission_ of RidePal is to help users have great trips in the company of their favorite music artists and tracks.** 

<br />

## **Functionalities of the RidePal web application**

#### **Each unauthenticated _visitor_ of the RidePal web app can**:

✅ See the **three top playlists** on the Home page of the app 

✅ See some of the featured artists and app statistics about the total number of tracks, artists, playlists and users

✅ **See all playlists** on the app’s database with their rank and playtime

✅ **Search** through the playlists and filter the results by preferred music genre, playlist duration or keywords. By default the playlists are sorted them in descending order by their rank.

✅ See **details of a specific playlist**, including its music genres, as well as **play any song** from it

✅ **Register** with an account for the web application, **login** and **logout**

<br />

#### **After registration on the RidePal web application, an authenticated _user_ can**:

✅ **Create a playlist** by setting start location and destination for the trip, preferred music genres and their percentage, and other preferences like repetition of the same artist and including top rated tracks. All created playlists undergo a generation algorithm, which does not allow identical playlists to be created, even if the users' input is identical.

✅ Have a page containing **all their created playlists**, so that they can **listen to them** any time they want

✅ **Edit or delete** their own playlists. Edits include playlist title and music genres but not the included tracks

✅ **Add playlists to** their **Favorite** playlists collection, where they can add any playlist from the app

✅ **Remove playlists from** their **Favorite** playlists collection

<br />

#### **Further to the functionalities available for registered users, the _admin_ user on the RidePal web application can**:

✅ Create, edit and delete any **playlist** on the app’s database

✅ Edit, ban and delete **users** and other administrators

<br />

### The **REST API** of RidePal project supports the following functionalities:

- **Access playlists**, both as a single item as well as a collection

- **Create new playlists** based on the same options as registered users on the app **through user authentication**

- User can **edit or delete** his/her own **existing playlists** through user authentication

- API documentation through **Swagger** at https://localhost:5001/swagger

<br />

### **Technologies used in the RidePal project**:

ASP.NET Core 3.1 and Visual Studio 2019

REST API

Database back-end: MS SQL Server

Entity Framework Core, EF InMemory

Standard ASP.NET Identity System

Unit test framework: MSTest

Continuous Integration Server (at GitLab) running unit tests at each commit to the master branch

Mocking framework: Moq

External API services: Deezer API, Microsoft Bing Maps, Pixabay REST service

Frontend: HTML, CSS, Bootstrap, DataTables, JavaScript

Team work: Git (GitLab) with separate branches, Trello, MS Teams

Best programming practices and principles used: OOP, SOLID, KISS and DRY principles, client-side and server-side data validation, exception handling, unit testing of the "business" functionality, etc.

<br />

### **To build and run the project, one needs to:**

1. Clone the project's repository locally

2. Open the project's RidePal.sln with Visual Studio

3. Go to Package Manager Console, choose RidePal.Data as Default Project and add PM> update-database

4. Run the RidePal application

5. Go to https://localhost:5001/home/seeddatabase

6. Explore the web app, register an account, create a playlist upon your preferences and enjoy your music

<br />

## Views from the RidePal web application

#### Home page 

![Text that reveals a missing image](/Images/Home.PNG)

#### Home page Featured and App Statistics

![Text that reveals a missing image](/Images/Statistics.PNG)

#### All Playlists

![Text that reveals a missing image](/Images/AllPlaylists.PNG)

#### Create New Playlist form

![Text that reveals a missing image](/Images/Create.PNG)

#### Playlist's details view

![Text that reveals a missing image](/Images/Details.PNG)

#### Admin's Users panel

![Text that reveals a missing image](/Images/AdminUsers.PNG)

#### About us

![Text that reveals a missing image](/Images/About_us.PNG)

#### Swagger documentation for the REST API

![Text that reveals a missing image](/Images/Swagger.PNG)

##### Additional views are available in the Images folder above
