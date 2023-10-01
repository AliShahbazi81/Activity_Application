import React from "react";
import {Grid, List} from "semantic-ui-react";
import {Activity} from "../../types/activity";
import ActivityList from "./ActivityList";
import ActivityDetails from "./ActivityDetails";
import ActivityForm from "./form/ActivityForm";
import {useStore} from "../../stores/store";

interface Props{
	  activities: Activity[]
	  createOrEditActivity: (activity: Activity) => void
	  deleteActivity:(id: string) => void
	  submitting: boolean
}
export default function ActivityDashboard(
	  {activities,
	  		createOrEditActivity,
	  		deleteActivity,
			submitting}: Props) {
	  
	  const {activityStore} = useStore();
	  const {editMode, selectedActivity} = activityStore
	  
	  return (
			 <Grid >
				   <Grid.Column width={"10"}>
						 <List>
							   <ActivityList 
									 activities={activities} 
									 deleteActivity={deleteActivity}
									 submitting={submitting}
							   />
						 </List>
				   </Grid.Column>
				   <Grid.Column width={"6"}>
						 { selectedActivity && !editMode &&
							 < ActivityDetails />}
						 {editMode &&
						 <ActivityForm
							   createOrEditActivity={createOrEditActivity}
                               submitting={submitting}
						 />}
				   </Grid.Column>
			 </Grid>
	  );
}