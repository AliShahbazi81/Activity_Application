import {observer} from "mobx-react-lite";
import React from "react";
import {Image, List, Popup} from "semantic-ui-react";
import {Profile} from "../../../types/profile";
import {Link} from "react-router-dom";
import UserProfile from "../../profiles/UserProfile";

interface Props {
	  attendees: Profile[]
}

export default observer(function ActivityListItemAttendee({attendees}: Props) {
	  return (
			<List horizontal>
				  {attendees.map(attendee => (
						<Popup
							  hoverable
							  key={attendee.username}
							  trigger={
									<List.Item
										  as={Link}
										  to={`profiles/${attendee.username}`}
									>
										  <Image
												circular
												size={"mini"}
												src={attendee.image || "assets/user.png"}
										  />
									</List.Item>
							  }
						>
							  <Popup.Content>
									<UserProfile profile={attendee}/>
							  </Popup.Content>
						</Popup>

				  ))}
			</List>
	  )
})