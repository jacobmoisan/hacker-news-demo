
This is just an example application for demonstration purposes of loading from the hacker news API https://github.com/HackerNews/API.  It is a .net 8 core WEB API backend with an Angular 20 frontend 

<h3>Local development instructions</h3>

* install .net 8.0 SDK https://dotnet.microsoft.com/en-us/download/dotnet/8.0
* install Node.js and npm https://nodejs.org/en/download  
  * tested with node v22.20.0 and npm 10.9.3

Open up a terminal and run the following command in /frontend/ directory
* npm install

Go to /backend/hacker-news.Api/ directory in terminal and run this command
* dotnet run

While that is running checkout what port it runs at, and take a look at /frontend/proxy.conf.json file to ensure it matches.  
Now you can open a new terminal and go to the /frontend/ directory
* ng serve --proxy-config proxy.conf.json

🥳🎆🥳 You did it! the project can be tested in your browser at localhost:4200/

For testing backend go to hacker-news-demo base directory and run:  
* dotnet test

For testing front end go to /frontend/ directory and run:
* ng test
