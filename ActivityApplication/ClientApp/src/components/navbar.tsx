import {Button, Container, Image, Menu, MenuItem} from "semantic-ui-react";
import React from "react";

interface Props{
	  openForm: () => void
}
export default function NavBar({openForm}: Props)
{
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
									onClick={openForm}
									positive 
									content={"Create Activity"}
							  />
						</MenuItem>
				  </Container>
				  
			</Menu>
	  )
}