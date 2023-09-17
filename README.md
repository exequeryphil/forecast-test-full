Simple weather forecast app.

Backend (dotnet app) lives in `.\WeatherBackend`

NOTE: 
- appsettings.json requires a missing parameter in order for the API to function.
- The frontend app is expecting a backend at http://localhost:5188

Frontend (React app) lives in `.\weather-frontend-ts`

A reminder that React apps have an `npm` dependency. To initialize and run the app locally:
```
npm install
npm run start
```

A rough Github workflow for example deployment/devops to Azure exists in `.\github-wf-sketch.yml`