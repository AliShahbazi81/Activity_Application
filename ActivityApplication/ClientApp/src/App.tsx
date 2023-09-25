import React, { useEffect, useState } from 'react';
import axios from 'axios';
import {Header, List} from "semantic-ui-react";

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
          <Header as={"h2"} icon={"users"} content={"Reactivities"}/>
          <List>
            {activities.map(activity => (
                  <List.Item key={activity.id}>
                    {activity.title}
                  </List.Item>
            ))}
          </List>
        </div>
  );
};

export default App;
