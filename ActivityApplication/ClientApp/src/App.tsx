import React, { useEffect } from 'react';
import {Container} from "semantic-ui-react";
import NavBar from "./components/navbar";
import ActivityDashboard from "./features/activity/ActivityDashboard";
import LoadingComponent from "./components/LoadingComponent";
import {useStore} from "./stores/store";
import {observer} from "mobx-react-lite";

const App: React.FC = () => {
      const {activityStore} = useStore()
  
  useEffect(() => {
    activityStore.loadActivities()
  }, [activityStore]);
  
  // Display loading component if the data is not retrieved yet
  if (activityStore.loadingInitial) return <LoadingComponent content={"Loading App..."} />

  return (
        <>
          <NavBar />
              <Container style={{marginTop: "7rem"}}>
                   <ActivityDashboard />
              </Container>
          
        </>
  );
};

export default observer(App);
