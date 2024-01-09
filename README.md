<h2 align="center">Online store using Blazor and .NET</h2>
<h3 align="left">About the project</h3>

The site user can go through the entire process from selecting a product to viewing the 
order in the order history.The user can filter products and compare characteristics. 
After the final selection, products can be added to the cart.Items in the cart will be 
saved in the database, and items from the cart can be accessed outside the current browser.
You can use a discount coupon in your shopping cart to reduce the cost of your order.
After placing an order, the order validation process occurs. In any case, within 5 minutes 
the order and its status will be displayed in your personal account.

The admin panel allows you to perform all CRUD operations with goods, remaining goods 
in warehouses, and promotional codes.You can also use the admin panel to find an order 
by customer phone number.


The web application consists of 5 APIs:
- Authentication API(JWT bearer).
- Product API, it is where the interaction of products and their residues occurs..
- Promocode API, well, these are just promotional codes...
- ShoppingCart API, storing products, storing promotional codes inside the cart, etc.
- Order API, all validated orders are stored and processed here..
- Orchestrator, a service that validates the user’s order. Initially, it receives a 
  “dirty” order, validates it using other APIs, and returns a “clean” order to the queue.

![scheme](https://imagess.hb.ru-msk.vkcs.cloud/Пустой%20диаграммой.png)

<h3 align="left">Technology stack used on the server side</h3>

- .NET 8, EF 8.
- MSSQL + Redis (cache).
- AutoMapper.
- RabbitMQ.
- Serilog.

<h3 align="left">Usage and Testing</h3>

- Clone the repository to your device.
- Download the missing ones json files {download link}.
- Change your MSSQL, RabbitMQ, Redis connection strings (inside json files).
- Now you can run applications.

<h5 align="left">Thank you for reading to this line! Have a nice day 🥰</h5>

