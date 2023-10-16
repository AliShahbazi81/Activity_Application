import {observer} from 'mobx-react-lite';
import React from 'react'
import {Button, Header, Image, Item, Segment} from 'semantic-ui-react'
import {Activity} from "../../../types/activity";
import {Link} from "react-router-dom";
import {format} from "date-fns";

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
	  return (
			<Segment.Group>
				  <Segment basic attached='top' style={{padding: '0'}}>
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
							  <Button
									color='orange'
									floated='right'
									as={Link}
									to={`/manage/${activity.id}`}
							  >
									Manage Event
							  </Button>
						) : activity.isGoing ? (
							  <Button>Cancel attendance</Button>
						) : (
							  <Button color='teal'>Join Activity</Button>
						)}
				  </Segment>
			</Segment.Group>
	  )
})