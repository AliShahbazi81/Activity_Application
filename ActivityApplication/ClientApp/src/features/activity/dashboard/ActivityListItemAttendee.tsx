import {observer} from "mobx-react-lite";
import React from "react";
import {Image, List} from "semantic-ui-react";
import { Profile } from "../../../types/profile";
import {Link} from "react-router-dom";

interface Props{
	  attendees: Profile[]
}
export default observer(function ActivityListItemAttendee({attendees}: Props)
{
	  return(
			<List horizontal>
				  {attendees.map(attendee => (
						<List.Item 
							  key={attendee.username} 
							  as={Link} 
							  to={`profiles/${attendee.username}`}
						>
							  <Image
									circular
									size={"mini"}
									src={attendee.image || "assets/user.png"}
							  />
						</List.Item>
				  ))}
			</List>
	  )
})