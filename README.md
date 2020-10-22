# Checkout.com

### Run application using dotnet

git clone https://github.com/Ruilomba/Checkout.com.git

##### Development
dotnet run Presentation.Api.csproj

Access api http://localhost:5000/swagger

##### End2EndTests
Start Presentation.Api.csproj using ASPNETCORE_ENVIRONMENT as Tests
Navigate to Tests folder and use dotnet test on checkout.com.Tests.csproj
This will run both integration and end2endTests

### Run application using docker

docker build -t checkout.com-api checkout.com/. --no-cache
This will run unit tests so if build is successfull it means the tests have passed

##### Development
docker run --rm -it -p 5000:80 checkout.com-api 

Access application on http://localhost:5000/swagger

##### End2EndTests
docker run --rm -it -p 5000:80 -e "ASPNETCORE_ENVIRONMENT=Tests" checkout.com-api
Navigate to Tests folder and use dotnet test on checkout.com.Tests.csproj
This will run both integration and end2endTests
