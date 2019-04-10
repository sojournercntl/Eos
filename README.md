# Eos
The Eos Framework offers unique functionality to send push notifications to real-time applications that work without a connection.

Because the Eos Framework is based on Reactor, it is easily extensible and offers full security coverage. Three step authentication and subsequent synchronous encryption are included with no additional requirements.

Using Eos, Push Notification Server and Client can be created in minutes.

### Patterns

* Singleton - LighthouseServer/LighthouseServer.cs
* Singleton - LighthouseEnabledClient/Client.cs
* Observer - LighthouseEnabledClient/Client.cs
* Proxy - Reactor.dll
* Factory - LighthouseServer/Factory + LighthouseServer.cs
* MVC - LighthouseClientExample
* Command - Undo/Redo of Push Notification Receives
