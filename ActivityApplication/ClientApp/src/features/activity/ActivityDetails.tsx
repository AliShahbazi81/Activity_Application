import {Button, Card, Image} from "semantic-ui-react";
import React, {useEffect} from "react";
import {useStore} from "../../stores/store";
import LoadingComponent from "../../components/LoadingComponent";
import {observer} from "mobx-react-lite";
import {useParams} from "react-router-dom";


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
			<Card fluid>
				  <Image src={`/assets/categoryImages/${activity.category}.jpg`} wrapped ui={false}/>
				  <Card.Content>
						<Card.Header>
							  {activity.title}
						</Card.Header>
						<Card.Meta>
							  <span>{activity.date}</span>
						</Card.Meta>
						<Card.Description>
							  {activity.description}
						</Card.Description>
				  </Card.Content>
				  <Card.Content extra>
						<Button.Group widths={"2"}>
							  <Button 
									basic color={"blue"} 
									content={"Edit"}
							  />
							  <Button 
									basic 
									color={"grey"} 
									content={"Cancel"}/>
						</Button.Group>
				  </Card.Content>
			</Card>
	  )
})