import React, { useState, useEffect } from 'react';

interface GeoLocation {
  lat: number;
  lon: number;
}

interface DayForecast {
  date: string;
  highTemp: number;
  lowTemp: number;
}

const Weather: React.FC = () => {
  const [location, setLocation] = useState<string>('');
  const [geoLocation, setGeoLocation] = useState<GeoLocation | null>(null);
  const [forecast, setForecast] = useState<DayForecast[] | null>(null);
  const [recentSearches, setRecentSearches] = useState<string[]>([]);
  const [errorMsg, setErrorMsg] = useState<string>("");

  useEffect(() => {
    navigator.geolocation.getCurrentPosition(position => {
      setGeoLocation({
        lat: position.coords.latitude,
        lon: position.coords.longitude
      });
    });
  }, []);

  useEffect(() => {
    if (geoLocation) {
      setLocation(`${geoLocation.lat}, ${geoLocation.lon}`);
    }
  }, [geoLocation]);

  const handleSearch = () => {
    if (location) {
      fetchWeather(location);
      setRecentSearches(prev => ([location, ...prev.slice(0, 9)]));
    }
  };

  const fetchWeather = (loc: string) => {
    const BASE_URL = 'http://localhost:5188/api/weather';
    const url = `${BASE_URL}?location=${loc}`;
    setErrorMsg("")

    fetch(url)
    .then(response => {
        if (!response.ok) {
            return response.json().then(err => { 
                throw new Error(JSON.parse(err.apiError).message); 
            });
        }
        return response.json();
    })
    .then(data => {
        const dailyData: DayForecast[] = data.timelines.daily.map((day: any) => ({
            date: day.time,
            highTemp: day.values.temperatureMax,
            lowTemp: day.values.temperatureMin,
        }));

        setForecast(dailyData);
    })
    .catch(error => {
        setErrorMsg(error.toString())
    });
  };

  return (
    <div className="container">
      <div>Enter your location (latlong or city)</div>
      <input 
        type="text"
        value={location}
        placeholder={geoLocation ? `${geoLocation.lat}, ${geoLocation.lon}` : 'Enter location...'}
        onChange={e => setLocation(e.target.value)}
        className="form-control"
      />
      <button onClick={handleSearch} className="btn btn-primary mt-3">Search</button>
      <p style={{color:'red'}}>{errorMsg ? errorMsg : ``}</p>

      {forecast && (
        <div className="mt-4">
            <h5>7-day Forecast for {location}:</h5>
            <div className="row">
            {forecast.map((day, index) => (
                <div key={index} className="col-md-4 mb-3">
                <div className="card">
                    <div className="card-body">
                    <h5 className="card-title">{new Date(day.date).toLocaleDateString()}</h5>
                    <p className="card-text">High: {day.highTemp}°F</p>
                    <p className="card-text">Low: {day.lowTemp}°F</p>
                    </div>
                </div>
                </div>
            ))}
            </div>
        </div>
        )}

      {recentSearches.length > 0 && (
        <div className="mt-4">
          <h5>Recent Searches:</h5>
          <ul>
            {recentSearches.map((search, index) => (
              <li key={index}>{search}</li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
}

export default Weather;
