import {Button, Container, Image, Menu, MenuItem} from "semantic-ui-react";
import React from "react";
import {useStore} from "../stores/store";

export default function NavBar()
{
	  const {activityStore} = useStore()
	  return(
			<Menu inverted fixed="top">
				  <Container>
						<MenuItem header>
							  <Image src={"/assets/activity-assessment.png"} alt="Logo" style={{width: "35px", marginRight:"5px"}}/>
							  Reactivities
						</MenuItem>
						<MenuItem name={"activities"}/>
						<MenuItem>
							  <Button 
									onClick={() => activityStore.openForm()}
									positive 
									content={"Create Activity"}
							  />
						</MenuItem>
				  </Container>
				  
			</Menu>
	  )
}