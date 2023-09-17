import 'bootstrap/dist/css/bootstrap.min.css';
import Weather from './Components/Weather';

function App() {
  return (
    <div className="App">
    <div className="container mt-5">
      <div className="text-center">
        <h1>Weather Forecasts</h1>
      </div>
      <Weather />
      </div>
    </div>
  );
}


export default App;
