import React, { useEffect, useState } from 'react';
import {Container} from "semantic-ui-react";
import {Activity} from "./types/activity";
import NavBar from "./components/navbar";
import ActivityDashboard from "./features/activity/ActivityDashboard";
import agent from "./api/agent";
import LoadingComponent from "./components/LoadingComponent";
import { v4 as uuid } from 'uuid';
import {useStore} from "./stores/store";
import {observer} from "mobx-react-lite";

const App: React.FC = () => {
      const {activityStore} = useStore()
      
  const [activities, setActivities] = useState<Activity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
  const [editMode, setEditMode] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  
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
  
  // If the activity has id, it means that we are going to edit it,
      // Otherwise, if it does not have any id, then we are creating a new activity
  function handleEditOrCreateActivity(activity: Activity)
  {
        setSubmitting(true)
        // If there is an id, it means we are updating the data
        if (activity.id)
        {
              agent.Activities.update(activity)
                    .then(() => {
                          setActivities([...activities.filter(x => x.id !== activity.id), activity])
                          setSelectedActivity(activity)
                          setEditMode(false)
                          setSubmitting(false)
                    })
        }
        // If there is no id, then we are creating a data
        else
        {
              activity.id = uuid();
              agent.Activities.create(activity)
                    .then(() => {
                          setActivities([...activities, activity])
                          setSelectedActivity(activity)
                          setEditMode(false)
                          setSubmitting(false)
                    })
        }
  }
  
  function handleDeleteActivity(id: string)
  {
        setSubmitting(true)
        // Delete the activity from the DataBase
        agent.Activities.delete(id)
              .then(() => {
                    // Also delete the activity from the client side so that we will not require to recall Get function
                    setActivities([...activities.filter(x => x.id !== id)])
                    setSubmitting(false)
              })
  }

  useEffect(() => {
    activityStore.loadActivities()
  }, [activityStore]);
  
  // Display loading component if the data is not retrieved yet
  if (activityStore.loadingInitial) return <LoadingComponent content={"Loading App..."} />

  return (
        <>
          <NavBar openForm={handleFormOpen}/>
              <Container style={{marginTop: "7rem"}}>
                   <ActivityDashboard 
                         activities={activityStore.activities} 
                         selectedActivity = {selectedActivity}
                         selectActivity =  {handleSelectActivity}
                         cancelSelectActivity = {handleCancelSelectActivity}
                         editMode={editMode}
                         openForm = {handleFormOpen}
                         closeForm={handleFormClose}
                         createOrEditActivity={handleEditOrCreateActivity}
                         deleteActivity={handleDeleteActivity}
                         submitting={submitting}
                   />
              </Container>
          
        </>
  );
};

export default observer(App);
