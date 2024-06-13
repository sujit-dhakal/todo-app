
# base image for build environment and build-env is is the name for the build process which can be reffered later
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

#working directory COPY RUN command will be executed within this directory inside the docker container
WORKDIR /app

#container will listen on port 80 at run time: used to map container ports to host ports
# 80 is for http 443 for https 
#just for documentation
EXPOSE 80
EXPOSE 443	

#copies the project file into the container i.e to /app
COPY *.csproj ./

#restore the dependencies specified in the .csproj i.e downloads and installs nuget packages
RUN dotnet restore 

#copies the remaining files into the container
COPY . ./

#build the application and publishes it: compiles the application in Release configuration ouputs the result to out dir
# Release and debug 2 ta hunxa debug is used for development Release is used for production builds
# using release docker image contains optimized version of our application
RUN dotnet publish -c Release -o out

#base image for runtime environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final-env
WORKDIR /app

#copies the published output of the build stage to the final stage
COPY --from=build-env /app/out .

# command to run when the container starts: runs the dotnet Todoapp.dll which starts the application
ENTRYPOINT ["dotnet", "TodoApp.dll"]