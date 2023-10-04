import React, {useEffect} from "react";
import {Grid, List} from "semantic-ui-react";
import ActivityList from "./ActivityList";
import {useStore} from "../../stores/store";
import {observer} from "mobx-react-lite";
import LoadingComponent from "../../components/LoadingComponent";

export default observer(function ActivityDashboard() {

	  const {activityStore} = useStore();
	  
	  useEffect(() => {
			activityStore.loadActivities()
	  }, [activityStore]);

	  // Display loading component if the data is not retrieved yet
	  if (activityStore.loadingInitial) return <LoadingComponent content={"Loading App..."} />
	  
	  return (
			 <Grid >
				   <Grid.Column width={"10"}>
						 <List>
							   <ActivityList />
						 </List>
				   </Grid.Column>
				   <Grid.Column width={"6"}>
						 <h2>Activity Filters</h2>
				   </Grid.Column>
			 </Grid>
	  );
})