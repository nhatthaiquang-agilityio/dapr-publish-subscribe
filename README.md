# DAPR: publish and subscribe messages example
Example: Distributed Application Runtime(DAPR) Publish and Subscriber messages via RabbitMQ
    - Using .NET5
    - Using AzureServiceBus(branch:AzureServiceBus)

### Requirements
----------------
+ Install [Minikube](https://minikube.sigs.k8s.io/docs/start/)
+ Install [Dapr](https://docs.dapr.io/getting-started/)
+ Install .NET5

### Structures
----------------
+ Producer: using the API sends messages
+ Consumer: receive messages from pubSubName and Topic
+ Messages: create message model
+ Components: `pubsub.yaml` is config your message bus (RabbitMQ)


### Usages
+ Start RabbitMQ:
    ```
    cd DemoMicroservices
    docker-compose up
    ```

+ Start Consumer(app-id: checkout)
    ```
    cd DemoMicroservices\Consumer
    dapr run --app-id checkout --components-path ../../Components --app-port 5000 --dapr-http-port 3500 --dapr-grpc-port 60002 dotnet run
    ```
    The /checkout endpoint matches the route defined in the subscriptions and this is where Dapr will send all topic messages to.
    It will be created a queue with name: checkout-orders

+ Start Producer(app-id: orderprocessing)
    ```
    cd DemoMicroservices\Producer
    dapr run --app-id orderprocessing --app-port 5011 --dapr-http-port 3501 --dapr-grpc-port 60001 --components-path ../../Components dotnet run
    ```

+ Send a message from Producer to Consumer
    ```
    http://localhost:5011/order
    {
        "OrderAmount":12.7,
        "OrderNumber":"Num18291",
        "OrderDate":"2021-12-21"
    }
    ```

+ Publish message
    ![Publisher](./Images/Dapr-Publish.png)


+ Consumer message
    ![Consumer](./Images/Dapr-Consumer.png)

### References
--------------
+ [Dapr Pub Sub ASP Net Core integration](https://yourazurecoach.com/2019/12/27/exploring-dapr-pub-sub-part-2-asp-net-core-integration/)
+ [Dapr publish subcriber for net developers](https://docs.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/publish-subscribe)
+ [Publish a message and subscribe to a topic](https://docs.dapr.io/developing-applications/building-blocks/pubsub/howto-publish-subscribe/)