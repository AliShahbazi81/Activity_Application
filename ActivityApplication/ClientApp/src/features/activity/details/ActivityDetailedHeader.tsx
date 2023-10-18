import {observer} from 'mobx-react-lite';
import React from 'react'
import {Button, Header, Image, Item, Label, Segment} from 'semantic-ui-react'
import {Activity} from "../../../types/activity";
import {Link} from "react-router-dom";
import {format} from "date-fns";
import {useStore} from "../../../stores/store";

const activityImageStyle = {
	  filter: 'brightness(30%)'
};

const activityImageTextStyle = {
	  position: 'absolute',
	  bottom: '5%',
	  left: '5%',
	  width: '100%',
	  height: 'auto',
	  color: 'white'
};

interface Props {
	  activity: Activity
}

export default observer(function ActivityDetailedHeader({activity}: Props) {
	  const {activityStore: {updateAttendance, cancelActivityToggle, loading}} = useStore();
	  return (
			<Segment.Group>
				  <Segment basic attached='top' style={{padding: '0'}}>
						{activity.isCancelled &&
                            <Label
                                ribbon
                                color={"red"}
                                content={"Cancelled"}
                                style={{position: "absolute", zIndex: 1000, left: -14, top: 20}}
                            />
						}
						<Image src={`/assets/categoryImages/${activity.category}.jpg`} fluid style={activityImageStyle}/>
						<Segment style={activityImageTextStyle} basic>
							  <Item.Group>
									<Item>
										  <Item.Content>
												<Header
													  size='huge'
													  content={activity.title}
													  style={{color: 'white'}}
												/>
												<p>{format(activity.date!, "dd MMM yyyy")}</p>
												<p>
													  Hosted by
													  <Link to={`/profiles/${activity.host?.username}`}>
															<strong>{" " + activity.host?.displayName.toUpperCase()}</strong>
													  </Link>
												</p>
										  </Item.Content>
									</Item>
							  </Item.Group>
						</Segment>
				  </Segment>
				  <Segment clearing attached='bottom'>
						{/*
							1. If user is host -> They can manage the activity
							2. If they are not host, and joined -> They can cancel their attendance
							3. If they are not host and not joined the activity -> They can join the activity
						*/}
						{activity.isHost ? (
							  <>
									<Button
										  basic
										  color={activity.isCancelled ? "green" : "red"}
										  floated={"left"}
										  content={activity.isCancelled ? "Re-activate Activity" : "Cancel the Activity"}
										  onClick={cancelActivityToggle}
										  loading={loading}
									/>
									<Button
										  disabled={activity.isCancelled}
										  color='orange'
										  floated='right'
										  as={Link}
										  to={`/manage/${activity.id}`}
									>
										  Manage Event
									</Button>
							  </>
						) : activity.isGoing ? (
							  /* Since we do not have any parameters for the updateAttendance, we do not have to make the onClick as a fallback function */
							  <Button
									onClick={updateAttendance}
									loading={loading}>
									Cancel attendance
							  </Button>
						) : (
							  <Button
									disabled={activity.isCancelled}
									color='teal'
									onClick={updateAttendance}
									loading={loading}>
									Join Activity
							  </Button>
						)}
				  </Segment>
			</Segment.Group>
	  )
})