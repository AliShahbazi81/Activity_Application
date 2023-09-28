import React from "react";
import {Grid, List} from "semantic-ui-react";
import {Activity} from "../../types/activity";
import ActivityList from "./ActivityList";
import ActivityDetails from "./ActivityDetails";
import ActivityForm from "./form/ActivityForm";

interface Props{
	  activities: Activity[]
	  selectedActivity: Activity | undefined
	  selectActivity: (id: string) => void
	  cancelSelectActivity: () => void
	  editMode: boolean
	  openForm: (id: string) => void
	  closeForm: () => void;
}
export default function ActivityDashboard(
	  {activities, 
			selectActivity, 
			selectedActivity, 
			cancelSelectActivity, 
			editMode, 
			openForm, 
			closeForm}: Props) {
	  return (
			 <Grid >
				   <Grid.Column width={"10"}>
						 <List>
							   <ActivityList 
									 activities={activities} 
									 selectActivity={selectActivity}
							   />
						 </List>
				   </Grid.Column>
				   <Grid.Column width={"6"}>
						 { selectedActivity && !editMode &&
							 < ActivityDetails 
								 activity={selectedActivity} 
								 cancelSelectActivity={cancelSelectActivity}
								 openForm={openForm}
								 closeForm={closeForm}
							 />}
						 {editMode &&
						 <ActivityForm 
							   activity={selectedActivity}
							   closeForm={closeForm}
							   editMode={editMode}
						 />}
				   </Grid.Column>
			 </Grid>
	  );
}