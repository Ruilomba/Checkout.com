# Checkout.com

### Run application using dotnet

git clone https://github.com/Ruilomba/Checkout.com.git

##### Development
```powershell
dotnet run Presentation.Api.csproj
```

Access api http://localhost:5000/swagger

##### End2EndTests
Start Presentation.Api.csproj using ASPNETCORE_ENVIRONMENT as Tests

Navigate to Tests folder and use 
```powershell
dotnet test
```  
on checkout.com.Tests.csproj
This will run both integration and end2endTests

### Run application using docker

```powershell
docker build -t checkout.com-api checkout.com/. --no-cache
```

This will run unit tests so if build is successfull it means the tests have passed

##### Development
```powershell
docker run --rm -it -p 5000:80 checkout.com-api 
```
Access application on http://localhost:5000/swagger

##### End2EndTests
```powershell
docker run --rm -it -p 5000:80 -e "ASPNETCORE_ENVIRONMENT=Tests" checkout.com-api
```
Navigate to Tests folder and use 
```powershell
dotnet test
``` 
on checkout.com.Tests.csproj
This will run both integration and end2endTests

## Documentation


### Use cases

![Process Payment](Documentation/Use%20Cases/ProcessPayment.png)

![Search Payment](Documentation/Use%20Cases/SearchPayments.png)

![Search Payment](Documentation/Use%20Cases/GetPaymentById.png)

### Component Diagram
![Search Payment](Documentation/Component%20Diagram/PaymentGateway.png)

### Implementation details

I have added tests to the application not to have all the coverage but just as an exercise for you to be able to see how I write tests.

I like to keep them small and concise.

For that I only tested a small portion of the application.

* Unit Tests -> Only added unit tests to PaymentService class
* end2end Tests -> Added one end2endTest for each endpoint
* Integration Tests -> ShouldSaveAndFetchPaymentById and ShouldSaveAndFetchPaymentUsingSearch where I test all methods of the payment repository

#### Business Assumptions and Solutions

* Assumption -> Payment gateway has different contracts for each merchant
* Solution -> payment gateway service will fetch data on merchant service regarding the commission that this merchant has to pay.



* Assumption -> Payment gateway will redirect payments made with different cards to the appropriate acquiring bank capable of handling such request.
Those acquiring bank api's have different API's

* Solution -> Payment gateway will connect to an adapter, which is responsible for detecting what type of transaction needs to be done,
 transforming data accordingly and send the request to the appropriate acquiring bank.
If the card is Master Card the request will be redirected to the currently unique implementation 
that I have which is the Acquiring Bank. This acquiring bank receives two payments as input so I create a payment from shopper to
the payment gateway bank which holds the commission value and also a payment from shopper to merchant which holds the rest of the value.
It also exists an implementation for adapting a request if the card is VISA, however the workflow is not implemented.



* Assumption -> Payment gateway will store payments after receiving the response from the acquiring bank
* Solution -> payment gateway will wait for acquiring bank response and store the payment on the database with the current status of the payment



* Assumption -> We cannot store card number without encryption
* Solution -> We encrypt the data before storing on database



* Assumption -> We cannot send CCV and Card number to Acquiring bank without encryption
* Solution -> There is a shared key between payment gateway and acquiring bank so the data will be encrypted before sending and decrypted 
for reading the values.


#### Technical solution
I tried to decouple logic and dependencies as much as I could. For example created a client for calling our API but refering only the
DTO project on the API in order not to give extra ambiguity to our clients



