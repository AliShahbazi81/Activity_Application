import React from "react";
import {Grid, List} from "semantic-ui-react";
import {Activity} from "../../types/activity";
import ActivityList from "./ActivityList";
import ActivityDetails from "./ActivityDetails";

interface Props{
	  activities: Activity[]
}
export default function ActivityDashboard({activities}: Props) {
	  return (
			 <Grid >
				   <Grid.Column width={"10"}>
						 <List>
							   <ActivityList activities={activities} />
						 </List>
				   </Grid.Column>
				   <Grid.Column width={"6"}>
						 { activities[0] && 
							 < ActivityDetails  activity={activities[0]}/>}
				   </Grid.Column>
			 </Grid>
	  );
}