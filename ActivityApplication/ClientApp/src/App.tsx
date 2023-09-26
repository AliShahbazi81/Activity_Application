import React, { useEffect, useState } from 'react';
import axios from 'axios';
import {Container, Header, List} from "semantic-ui-react";
import {Activity} from "./types/activity";
import NavBar from "./components/navbar";
import ActivityDashboard from "./features/activity/ActivityDashboard";

const App: React.FC = () => {
  const [activities, setActivities] = useState<Activity[]>([]);

  useEffect(() => {
    axios.get<Activity[]>('https://localhost:7290/api/Activity/Get')
          .then(response => {
            setActivities(response.data);
          });
  }, []);

  return (
        <>
          <NavBar />
              <Container style={{marginTop: "7rem"}}>
                   <ActivityDashboard activities={activities} />
              </Container>
          
        </>
  );
};

export default App;
