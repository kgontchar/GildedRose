# GildedRose
I used a JWT token for authorization, because it's the most comfortable way to authorize, in my opinion.

Examples:
You can import queries in postman via File -> Import -> Open file "postman.json" -> You are ready

In this application basicly you have 5 users:
	- user1 - admin 	(username: user1, password: user1)
	- user2 - just user (username: user2, password: user2)
	- user3 - just user (username: user3, password: user3)
	- user4 - just user (username: user4, password: user4)
	- user5 - just user (username: user5, password: user5)

Admin can create new items, modify and delete them
Users and unathorized people can get list of items and one item
Every authorized user can buy item

Steps to test application:

Build and run application, authorize and get token (example in postman queries). Url - http://localhost:54809/api/auth/token 
To buy item you should authorize by any user (user1 - user5) using Bearer token authorization

Admin
To modify item http://localhost:54809/api/items/ - Post query
To create item http://localhost:54809/api/items/ - Put query
To delete item http://localhost:54809/api/items/1 - Delete query

To modify user http://localhost:54809/api/users/ - Post query
To create user http://localhost:54809/api/users/ - Put query
To delete user http://localhost:54809/api/users/1 - Delete query

Anybody can watch list of users or specific user, but it's not problem to add necessity to be authorized

- How do we know if a user is authenticated?
- You can send query to address: "http://localhost:54809/api/claims" and if you authenticated, then you will get info about account

- Is it always possible to buy an item?
- Only authenticated can buy item, and only items, that can be bought

- In this project I chose JSON format of data, because he interacts best with UI frameworks (for example Angular or React)

P.S. I used AutoMapper in this project, because for me it is the most comfortable way of communication between entities.
P.P.S I'm sorry, that I didn't described api methods