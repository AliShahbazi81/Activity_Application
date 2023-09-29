import React, { useEffect, useState } from 'react';
import axios from 'axios';
import {Container, Header, List} from "semantic-ui-react";
import {Activity} from "./types/activity";
import NavBar from "./components/navbar";
import ActivityDashboard from "./features/activity/ActivityDashboard";
import agent from "./api/agent";

const App: React.FC = () => {
  const [activities, setActivities] = useState<Activity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
  const [editMode, setEditMode] = useState(false);
  
  // Functionality: Select an activity
  function handleSelectActivity(id: string)
  {
        setSelectedActivity(activities.find(x => x.id === id))
  }
  // Functionality: Cancel selection of an activity
  function handleCancelSelectActivity()
  {
        setSelectedActivity(undefined);
  }
  // Functionality: Open the form which means that the edit mode is true 
  function handleFormOpen(id?: string)
  {
        id ? handleSelectActivity(id) : handleCancelSelectActivity();
        setEditMode(true);
  }
  
  function handleFormClose()
  {
        setEditMode(false);
  }
  
  // If the activity has id, it means that we are gonna edit it,
      // Otherwise, if it does not have any id, then we are creating a new activity
  function handleEditOrCreateActivity(activity: Activity)
  {
        activity.id 
              ? setActivities([...activities.filter(x => x.id !== activity.id), activity])
              : setActivities([...activities, activity])
        setEditMode(false)
        setSelectedActivity(activity)
  }
  
  function handleDeleteActivity(id: string)
  {
        setActivities([...activities.filter(x => x.id !== id)])
  }

  useEffect(() => {
    agent.Activities.list()
          .then(response => {
            setActivities(response);
          });
  }, []);

  return (
        <>
          <NavBar openForm={handleFormOpen}/>
              <Container style={{marginTop: "7rem"}}>
                   <ActivityDashboard 
                         activities={activities} 
                         selectedActivity = {selectedActivity}
                         selectActivity =  {handleSelectActivity}
                         cancelSelectActivity = {handleCancelSelectActivity}
                         editMode={editMode}
                         openForm = {handleFormOpen}
                         closeForm={handleFormClose}
                         createOrEditActivity={handleEditOrCreateActivity}
                         deleteActivity={handleDeleteActivity}
                   />
              </Container>
          
        </>
  );
};

export default App;
