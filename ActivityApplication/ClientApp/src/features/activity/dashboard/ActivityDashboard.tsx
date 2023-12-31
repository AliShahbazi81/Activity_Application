import React, {useEffect} from "react";
import {Grid, List} from "semantic-ui-react";
import ActivityList from "./ActivityList";
import {useStore} from "../../../stores/store";
import {observer} from "mobx-react-lite";
import LoadingComponent from "../../../components/loading/LoadingComponent";
import ActivityFilters from "./ActivityFilters";

export default observer(function ActivityDashboard() {

	  const {activityStore} = useStore();
	  const {loadActivities, activityRegistry} = activityStore;
	  
	  useEffect(() => {
			if (activityRegistry.size <= 1) loadActivities()
	  }, [loadActivities, activityRegistry.size]);

	  // Display loading component if the data is not retrieved yet
	  if (activityStore.loadingInitial) return <LoadingComponent content={"Loading Activities..."} />
	  
	  return (
			 <Grid >
				   <Grid.Column width={"10"}>
						 <List>
							   <ActivityList />
						 </List>
				   </Grid.Column>
				   <Grid.Column width={"6"}>
						 <ActivityFilters />
				   </Grid.Column>
			 </Grid>
	  );
})