import {Activity} from "../../types/activity";
import React, {SyntheticEvent, useState} from "react";
import {Button, Item, Label, Segment} from "semantic-ui-react";
import {useStore} from "../../stores/store";

interface Props{
	  activities: Activity[]
	  submitting: boolean
	  deleteActivity: (id: string) => void
}
export default function ActivityList({activities, submitting, deleteActivity}: Props)
{
	  const {activityStore} = useStore()
	  
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
	  const [target, setTarget] = useState("");
	  return (
			<Segment>
				  <Item.Group divided>
						{activities.map((activity) => (
							  <Item key={activity.id}>
								<Item.Content>
									  <Item.Header as={"a"}>{activity.title}</Item.Header>
									  <Item.Meta>{activity.date}</Item.Meta>
									  <Item.Description>
											<div>{activity.description}</div>
											<div>{activity.city}, {activity.venue}</div>
									  </Item.Description>
									  <Item.Extra>
											<Button 
												  onClick={() => activityStore.selectActivity(activity.id)}
												  floated={"right"} 
												  content={"View"} 
												  color={"facebook"}/>
											<Button
												  // In order to prevent the application to activate all the deleting buttons
												  name={activity.id}
												  //! && target === activity.id is being added for preventing the app from triggering all the delete buttons
												  loading={submitting && target === activity.id}
												  onClick={(e) => handleDeleteButton(e, activity.id)}
												  floated={"right"}
												  content={"Delete"}
												  color={"red"}/>
											<Label basic content={activity.category}/>
									  </Item.Extra>
								</Item.Content>
							  </Item>
						))}
				  </Item.Group>
			</Segment>
	  )
}