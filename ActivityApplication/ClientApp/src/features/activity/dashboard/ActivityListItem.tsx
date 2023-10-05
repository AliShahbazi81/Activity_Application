import React, {SyntheticEvent, useState} from "react";
import {Button, Icon, Item, Segment} from "semantic-ui-react";
import {Link} from "react-router-dom";
import {Activity} from "../../../types/activity";
import {useStore} from "../../../stores/store";

interface Props {
	  activity: Activity
}

export default function ActivityListItem({activity}: Props)
{
	  const [target, setTarget] = useState("")
	  const {activityStore} = useStore()
	  const {deleteActivity, loading} = activityStore

	  // In order to prevent the app from activating all Delete buttons
	  // 1. Create a function with e as event and id of the data as the second parameter
	  // 2. Use setTarget for triggering the name of event
	  // 3. Recall the function which used to be called in the button as onClick in function 
	  // 4. Replace the previous function with the new function, which is called handleDeleteButton in this case.
	  // 5. Add "name" attribute to the button in which we want to add this feature
	  // 6. In "loading" attribute also, we have to add "target" attribute and set it to the id of the data
	  //! Almost all the mouse events are driven from SyntheticEvent. Hence, the e which has been inherited from SyntheticEvent<HTMLButtonElement>
	  //! It would be almost the same for most of the situations in which we want to trigger the mouse events
	  function handleDeleteButton(e: SyntheticEvent<HTMLButtonElement>, id: string)
	  {
			setTarget(e.currentTarget.name)
			deleteActivity(id)
	  }
	  
	  return(
			<Segment.Group>
				  
				  <Segment>
						<Item.Group>
							  <Item>
									<Item.Image
										circular
										size={"tiny"}
										src={"assets/user.png"}
										alt={"User Avatar"}
									/>
									<Item.Content>
										  <Item.Header 
												as={Link} 
												to={`/activities/${activity.id}`
										  }>
												{activity.title}
										  </Item.Header>
										  <Item.Description>
												Hosted by Bob
										  </Item.Description>
									</Item.Content>
							  </Item>
						</Item.Group>
				  </Segment>
				  
				  <Segment>
						<Icon name={"clock"}/> {activity.date}
						<Icon name={"marker"}/> {activity.venue}
				  </Segment>
				  
				  <Segment secondary>
						Attendees go here
				  </Segment>
				  
				  {/*! When we use floated in any of the components, we do have to use clearing attr in the Segment*/}
				  <Segment clearing>
						<span>{activity.description}</span>
						<Button 
							as={Link}
							to={`/activities/${activity.id}`}
							color={"teal"}
							floated={"right"}
							content={"View"}
						/>
				  </Segment>
				  
			</Segment.Group>
	  )
}