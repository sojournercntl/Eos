
![Logo](https://raw.githubusercontent.com/sojournercntl/Eos/master/Doc/eos_logo.svg?sanitize=true)


#### Connectionless Push Notifications

The Eos Framework offers unique functionality to send push notifications to real-time applications that work without a connection.

Because the Eos Framework is based on Reactor, it is easily extensible and offers full security coverage. Three step authentication and subsequent synchronous encryption are included with no additional requirements.

Using Eos, Push Notification Server and Client can be created in minutes.

#### Functionality

The EOS Framework works as described below:

The server is waiting for an incoming registration that takes place over a TCP connection. Once the client's registration is accepted, a UDP service port is enabled on the client side and the client listens to push messages. The server remembers the clients, but does not maintain an open connection to the client. This makes it possible to save server resources and create connectionless push notification services.

If the Time To Live expires, the client is contacted and asked if he wants to renew the registration, otherwise the client is no longer ready to receive Push Notifications.

Advantages of this new method are:
Resource intensive connections are no longer necessary
None of the methods such as long polling or SSE are used. This also eliminates their disadvantages.

#### Interactive Examples

In Addition, the project structure contains a few examples which show how to implement EOS into existing applications. The Lighthouse Server/Client also shows how to use different patterns.

* Singleton - LighthouseServer/LighthouseServer.cs
* Singleton - LighthouseClientExample/Client.cs
* Observer - LighthouseClientExample/Client.cs
* Proxy - Reactor.dll
* Factory - LighthouseServer/Factory + LighthouseServer.cs
* MVC - LighthouseClientExample
* Command - Undo/Redo of Push Notification Receives