import {Button, Container, Image, Menu, MenuItem} from "semantic-ui-react";
import React from "react";


export default function NavBar()
{
	  return(
			<Menu inverted fixed="top">
				  <Container>
						<MenuItem header>
							  <Image src={"../assets/activity-assessment.png"} alt="Logo"/>
							  Reactivities
						</MenuItem>
						<MenuItem name={"activities"}/>
						<MenuItem>
							  <Button positive content={"Create Activity"}/>
						</MenuItem>
				  </Container>
				  
			</Menu>
	  )
}