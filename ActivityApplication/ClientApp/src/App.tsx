import React, { useEffect, useState } from 'react';
import axios from 'axios';

const App: React.FC = () => {
  const [activities, setActivities] = useState<any[]>([]);

  useEffect(() => {
    axios.get('https://localhost:7290/api/Activity/GetActivities')
          .then(response => {
            setActivities(response.data);
          });
  }, []);

  return (
        <div>
          <h1>Reactivities</h1>
          <ul>
            {activities.map(activity => (
                  <li key={activity.id}>
                    {activity.title}
                  </li>
            ))}
          </ul>
        </div>
  );
};

export default App;
