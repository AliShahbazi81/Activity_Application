import {Grid} from "semantic-ui-react";
import React, {useEffect} from "react";
import {useStore} from "../../../stores/store";
import LoadingComponent from "../../../components/loading/LoadingComponent";
import {observer} from "mobx-react-lite";
import {useParams} from "react-router-dom";
import ActivityDetailedHeader from "./ActivityDetailedHeader";
import ActivityDetailedChat from "./ActivityDetailedChat";
import ActivityDetailedInfo from "./ActivityDetailedInfo";
import ActivityDetailedSidebar from "./ActivityDetailedSidebar";


export default observer(function ActivityDetails() {
	  const {activityStore} = useStore()
	  const {
			selectedActivity: activity,
			loadingInitial,
			loadActivity
	  } = activityStore
	  
	  // we use useParams hooks to get the id from the route
	  const {id} = useParams();
	  
	  // Then we are using the ID above to get the desired activity
	  //! NOTE: useEffect hook works once the component is loaded.
	  useEffect(() => {
			if (id) loadActivity(id)
	  }, [id, loadActivity])
	  
	  // If we forget to inject loadingInitial, Loading indicator will never disappear once we reload the page.
	  // Do not forget to add it up 
	  if (loadingInitial || !activity) return <LoadingComponent />;
	  
	  return (
			<Grid>
				 <Grid.Column width={10}>
					   <ActivityDetailedHeader activity={activity} />
					   <ActivityDetailedInfo activity={activity} />
					   <ActivityDetailedChat />
				 </Grid.Column>
				  
				  <Grid.Column width={6}>
						<ActivityDetailedSidebar activity={activity} />
				  </Grid.Column>
			</Grid>
	  )
})