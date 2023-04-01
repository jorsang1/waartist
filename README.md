# Waartist
This is a project boilerplate to query artists on a MongoDB database.  

It shows a bit how to work with a MongoDb database.

## Features

* It uses Clean Architecture-ish organization.
* Minimal API.
* CQRS.  
Commands goes through the application layer using MediatR.   
Queries are using the db client directly from the API layer.
* 'Migrations'  
This is no-sql so you can see how it doesn't need migrations.  
There is a `DatabaseInicialization` class to setup some demo data on app start. 

## Setup

You can quickly run a mongodb database using this single docker command:  
```bash
docker run -d -p 27017:27017 --name example-mongo mongo:latest
```

Then just run the solution and if you go to the `/artists` endpoint you should be able to see a couple of artists already. 

The solution also includes how to handle registration of 'artists' + login using JWT + endpoint with require authentication.

## Status

Keep in mind this is just to be used in the 'inception' of your next project.

## Takeaways

It can be a good starting point for something more complex.  

The best of using a no-sql database like MongoDB is that you can work with your complete agregates without needing to deal with complex mappings from the domain to the model.

## How to contribute

Share your opinion or leave a comment.  
PR's are also welcomed.