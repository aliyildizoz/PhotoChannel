
# PhotoChannel
 - [Purpose of the application](#purpose-of-the-application)
 - [Features](#features)
 - [Used technologies](#used-technologies) 
 - [Setup](#setup)
 - [Database Diagram](#database-diagram)
 - [Swagger](#swagger)

## English
### Purpose of the application
The application tried to combine some features of youtube and instagram. It is inspired by creating and subscribing channels on Youtube, sharing , liking and commenting photos on Instagram. In this direction, users can create channels and share photos, comment or like photos in these channels. We can also tag these channels and filter the channels accordingly and find channels in the areas we are interested in.
### Features

 1. **Creating an account**
 2. **Creating channels**
 3. **Profile page**
	- Viewing shared photos
	- Viewing liked photos
	- Viewing comments
	- Viewing subscriptions
	- Viewing the user's channels
 4. **Channel page**
	- Sharing photos on the channel
	- Viewing channel gallery
	- Viewing channel subscribers and channel owner
 5. **Photo**
	- Liking a photo
	- Doing comment on the photo
	- Viewing likes
	- Viewing comments
 6. **Home page**
	- Viewing current user's stream
	- Viewing the most liked photos
	- Viewing photos with the most comments
	- Viewing channels with the most subscribers
	- Filtering channels by category

### Setup
- **Install with Docker;**
   If docker is installed on your computer, open the terminal in the location where the `docker-compose.yml` file is located and run the `docker compose up` command.
- **To create the database by migrating;**
Open the terminal in the PhotoChannelWebApi folder and run the commands below.
	1. `dotnet ef migrations add InitialCreate`
	2. `dotnet ef database update`
- **To create database with sql script;**
	- Run the [CreateDatabase.sql](https://github.com/AliYildizoz909/PhotoChannel/blob/master/CreateDatabase.sql) file in ms sql database.

> The front of the project was written in react js. You can look from  [here](https://github.com/AliYildizoz909/photo-channel-spa).

## Used technologies

 - **Backend**
	 - Asp.Net Core Web Api
	 - .Net 8
	 - Entity Framework Core
	 - Fluent Validation
	 - Cloudinary
	 - InMemory Cache
	 - NLog
	 - Filtering
	 - SwaggerUI
	 - N-Tier Architecture
- **FrontEnd**
	- reactjs
	- react-bootstrap
	- bootstrap
	- filepond
	- redux
	- redux-thunk
	- cloudinary-react
	- react-router-dom
	- connected-react-router
	- simple-react-validator
	- react-photo-gallery
	- font-awesome
## Database Diagram
![Database Diagram](https://raw.githubusercontent.com/AliYildizoz909/PhotoChannel/master/Gifs/DatabaseDiagram.png)

## Swagger
![Swagger](https://raw.githubusercontent.com/AliYildizoz909/PhotoChannel/master/Gifs/Swagger.jpeg)

