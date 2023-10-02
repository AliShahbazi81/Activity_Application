import {Button, Container, Image, Menu, MenuItem} from "semantic-ui-react";
import React from "react";
import {useStore} from "../stores/store";
import {NavLink} from "react-router-dom";

export default function NavBar()
{
	  const {activityStore} = useStore()
	  return(
			<Menu inverted fixed="top">
				  <Container>
						<MenuItem as={NavLink} to={"/"} header>
							  <Image src={"/assets/activity-assessment.png"} alt="Logo" style={{width: "35px", marginRight:"5px"}}/>
							  Reactivities
						</MenuItem>
						<MenuItem as={NavLink} to={"/activities"} name={"activities"}/>
						<MenuItem>
							  <Button 
									as={NavLink}
									to={"/createActivity"}
									positive 
									content={"Create Activity"}
							  />
						</MenuItem>
				  </Container>
				  
			</Menu>
	  )
}